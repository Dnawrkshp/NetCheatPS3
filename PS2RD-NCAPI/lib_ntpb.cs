using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace PS2RD_NCAPI
{
    class lib_ntpb
    {

        const int SERVER_TCP_PORT = 4234;
        const int SERVER_UDP_PORT = 4244;

        static System.Net.Sockets.TcpClient tcpClient;

        static int ClientConnected = 0;
        static int haltState = 0;
        static int remote_cmd;

        // NTPB header magic
        const int ntpb_MagicSize = 6;
        readonly byte[] ntpb_hdrMagic = new byte[] { 0xff, 0x00, (byte)'N', (byte)'T', (byte)'P', (byte)'B' };
        const int ntpb_hdrSize = 10;

        const int NTPBCMD_SEND_DUMP =               0x0300;
        const int NTPBCMD_END_TRANSMIT =            0xffff;

        const int REMOTE_CMD_NONE =                 0x0000;
        const int REMOTE_CMD_DUMP =                 0x0100;
        const int REMOTE_CMD_HALT =                 0x0201;
        const int REMOTE_CMD_RESUME =               0x0202;
        const int REMOTE_CMD_ADDMEMPATCHES =        0x0501;
        const int REMOTE_CMD_CLEARMEMPATCHES =      0x0502;
        const int REMOTE_CMD_ADDRAWCODES =          0x0601;
        const int REMOTE_CMD_CLEARRAWCODES =        0x0602;

        const int MAX_PATCHES =                     256;
        const int MAX_CODES =                       256;

        struct NTPB_IO {
	        public int RemoteCMD;		//remote cmd to send - data thread keeps the global updated, hopefully
	        public int cmdSize;		//size of cmd to be sent
	        public byte[] cmdBuf;	//cmd buffer
	        public byte[] outBuffer; //buffer for returned data
        };

        /****************************************************************************/
        int check_ntpb_header(byte[] pbuf) // sanity check to see if the packet have the format we expect
        {
	        int i;

	        for (i=0; i<ntpb_MagicSize; i++)
            {
		        if (pbuf[i] != ntpb_hdrMagic[i])
			        break;
	        }

	        if (i == ntpb_MagicSize)
		        return 1;

	        return 0;
        }

        public bool ConnectToPS2rd(string ip)
        {
            if (tcpClient != null && tcpClient.Connected)
                tcpClient.Close();

            ClientConnected = 0;
            tcpClient = new System.Net.Sockets.TcpClient();
            tcpClient.Connect(ip, SERVER_TCP_PORT);

            if (tcpClient.Connected)
            {
                ClientConnected = 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        /****************************************************************************
        Send/Receive Data Thread

        Can also be called as a function. lParam is a pointer to an NTPB_IO struct.
        *****************************************************************************/
        long SendReceiveThread(NTPB_IO cmdInfo) // retrieving datas sent by server
        {
	        uint dump_size = 0, dump_wpos = 0;
	        int rcvSize = 0, sndSize = 0, packetSize = 0, ntpbpktSize = 0, ntpbCmd = 0, ln = 0, recv_size = 0;
	        byte[] pbuf;
	        int endTransmit = 0;
            //tcpClient.GetStream().ReadTimeout = 5000;

	        //Send Remote CMD
            byte[] pktbuffer = new byte[65536];
            Array.Copy(ntpb_hdrMagic, 0, pktbuffer, 0, ntpb_MagicSize); //copying NTPB Magic

            WriteUInt16(pktbuffer, ntpb_MagicSize, (ushort)cmdInfo.cmdSize);
            WriteUInt16(pktbuffer, ntpb_MagicSize + 2, (ushort)cmdInfo.RemoteCMD); 
            //Array.Copy(BitConverter.GetBytes((short)cmdInfo.cmdSize), 0, pktbuffer, ntpb_MagicSize, 2);
            //Array.Copy(BitConverter.GetBytes((short)cmdInfo.RemoteCMD), 0, pktbuffer, ntpb_MagicSize + 2, 2);
	        //*((unsigned short *)&pktbuffer[ntpb_MagicSize]) = cmdInfo->cmdSize;
	        //*((unsigned short *)&pktbuffer[ntpb_MagicSize+2]) = cmdInfo->RemoteCMD;

	        if ((ClientConnected == 0) || (remote_cmd != REMOTE_CMD_NONE)) {
		        MessageBox.Show("Client busy or not connected. Please check your request and hack again.","NTPB Client Error");
		        goto error;
	        }
	        remote_cmd = cmdInfo.RemoteCMD;

	        if ((cmdInfo.cmdBuf != null) && (cmdInfo.cmdSize > 0)) {
                Array.Copy(cmdInfo.cmdBuf, 0, pktbuffer, ntpb_hdrSize, cmdInfo.cmdSize);
		        //memcpy(&pktbuffer[ntpb_hdrSize], cmdInfo->cmdBuf, cmdInfo->cmdSize);
	        }

	        ntpbpktSize = ntpb_hdrSize + cmdInfo.cmdSize;

	        switch(remote_cmd)
	        {
		        case REMOTE_CMD_DUMP:
		        {
                    dump_size = ReadUInt32(pktbuffer, ntpb_hdrSize + 4) - ReadUInt32(pktbuffer, ntpb_hdrSize);
			        //dump_size = *((uint *)&pktbuffer[ntpb_hdrSize + 4]) - *((uint *)&pktbuffer[ntpb_hdrSize]);
		        } break;
		        case REMOTE_CMD_NONE: goto error;
	        }

	        // send the ntpb packet
            tcpClient.GetStream().Write(pktbuffer, 0, ntpbpktSize);
	        //sndSize = send(main_socket, &pktbuffer[0], ntpbpktSize, 0);
	        //if (sndSize <= 0) {
		    //    MessageBox.Show("Error: send failed !","ntpbclient");
		    //    goto error;
	        //}
	        //I'm guessing the server sends a packet back just to acknowledge the request?
	        //This looked redundant, but it wasn't working without it.
	        //rcvSize = recv(main_socket, &pktbuffer[0], sizeof(pktbuffer), 0);
            //rcvSize = tcpClient.GetStream().Read(pktbuffer, 0, pktbuffer.Length);
            //if (rcvSize <= 0) {
            //    MessageBox.Show("Error: recv failed !", "ntpbclient");
            //    goto error;
            //}


	        //Receive Data/Reply
	        while (true) {

		        pbuf = pktbuffer;

		        // receive the first packet
		        //rcvSize = recv(main_socket, &pktbuffer[0], sizeof(pktbuffer), 0);
                rcvSize = tcpClient.GetStream().Read(pktbuffer, 0, pktbuffer.Length);
		        if (rcvSize < 0) {
                    MessageBox.Show("Error: recv failed !", "ntpbclient");
			        goto error;
		        }

		        // packet sanity check
		        if (check_ntpb_header(pbuf) == 0) {
                    MessageBox.Show("Error: not ntpb packet !", "ntpbclient");
			        goto error;
		        }

		        ntpbpktSize = ReadUInt16(pbuf, 6);
		        packetSize = ntpbpktSize + ntpb_hdrSize;

		        recv_size = rcvSize;

		        // fragmented packet handling
		        while (recv_size < packetSize) {
                    rcvSize = tcpClient.GetStream().Read(pktbuffer, recv_size, pktbuffer.Length - recv_size);
			        //rcvSize = recv(main_socket, &pktbuffer[recv_size], sizeof(pktbuffer) - recv_size, 0);
			        if (rcvSize < 0) {
                        MessageBox.Show("Error: recv failed !", "ntpbclient");
				        goto error;
			        }
			        else {
				        recv_size += rcvSize;
			        }
		        }

		        // parses packet
		        if (check_ntpb_header(pbuf) != 0) {
			        ntpbCmd = ReadUInt16(pbuf, 8);

			        switch(ntpbCmd) { // treat Client Request here

				        case NTPBCMD_SEND_DUMP:
					        if ((dump_wpos + ntpbpktSize) > dump_size) {
                                MessageBox.Show("Error: dump size exeeded !", "ntpbclient");
						        goto error;
					        }

                            Array.Copy(pktbuffer, ntpb_hdrSize, cmdInfo.outBuffer, dump_wpos, ntpbpktSize);

					        //if (cmdInfo->fh_dump) { fwrite(&pktbuffer[ntpb_hdrSize], 1, ntpbpktSize, cmdInfo->fh_dump); }
					        //else { memcpy(cmdInfo->outBuffer, &pktbuffer[ntpb_hdrSize], ntpbpktSize); }
					        dump_wpos += (uint)ntpbpktSize;

					        // stepping progress bar
					        //UpdateProgressBar(PBM_STEPIT, 0, 0);
					        break;

				        case NTPBCMD_END_TRANSMIT:
					        System.Threading.Thread.Sleep(100);
					        //if(cmdInfo->fh_dump) fclose(cmdInfo->fh_dump);
					        endTransmit = 1;
					        break;
			        }

                    System.Threading.Thread.Sleep(1000);

			        //*((unsigned short *)&pktbuffer[ntpb_hdrSize]) = 1;
                    WriteUInt16(pktbuffer, ntpb_hdrSize, 1);
                    WriteUInt16(pktbuffer, 6, 0);
			        //*((unsigned short *)&pktbuffer[6]) = 0;
			        packetSize = ntpb_hdrSize + 2;

			        // send the response packet
			        //sndSize = send(main_socket, &pktbuffer[0], packetSize, 0);
                    tcpClient.GetStream().Write(pktbuffer, 0, packetSize);
			        //if (sndSize <= 0) {
                    //    MessageBox.Show("Error: send failed !", "ntpbclient");
				    //    goto error;
			        //}

			        if (endTransmit != 0)
				        break;
		        }
	        }

	        // resetting progress bar
	        //UpdateProgressBar(PBM_SETPOS, 0, 0);
	        //UpdateStatusBar("Idle", 0, 0);

	        remote_cmd = REMOTE_CMD_NONE;
	        //Notify main thread (1 on success, 0 on failure)
	        //if (cmdInfo->NotifyId) SendMessage(cmdInfo->NotifyHwnd, WM_COMMAND, cmdInfo->NotifyId, 1);
	        return 1;

        error:
	        //Connection failed
	        ClientConnected = 0;
	
	        return 0;
        }

        public byte[] DumpRAM(uint dump_start, uint dump_end)
        {
	        NTPB_IO cmdInfo = new NTPB_IO();
	        uint dump_size;

            cmdInfo.RemoteCMD = REMOTE_CMD_NONE;
            if ((dump_start >= 00100000) && (dump_end <= 0x02000000))
            { //EE Dump
                cmdInfo.RemoteCMD = REMOTE_CMD_DUMP;
            }
            else if ((dump_start >= 0) && (dump_end <= 0x00200000))
            { //IOP Dump
                cmdInfo.RemoteCMD = REMOTE_CMD_DUMP;
            }
            else if ((dump_start >= 0x80000000) && (dump_end <= 0x82000000))
            { //Kernel Dump
                cmdInfo.RemoteCMD = REMOTE_CMD_DUMP;
            }
            else if ((dump_start >= 0x70000000) && (dump_end <= 0x70004000))
            { //ScratchPad Dump
                cmdInfo.RemoteCMD = REMOTE_CMD_DUMP;
            }

	        //checking address range to determine course of action
	        //cmdInfo.RemoteCMD = REMOTE_CMD_NONE;
	        if (dump_start > dump_end) {
		        //sprintf(ErrTxt, "Search area start (%08X) is higher thand end (%08X). THINK about it. (DumpRAM)", dump_start, dump_end);
		        return null;
	        }
	        //cmdInfo.RemoteCMD = REMOTE_CMD_DUMP;
	        //Check that we're trying to dump a valid area
	        if (cmdInfo.RemoteCMD == REMOTE_CMD_NONE) {
		        //sprintf(ErrTxt, "Invalid search/dump area specifed. (DumpRAM)");
		        return null;
	        }

	        // create the dump file
            //cmdInfo.fh_dump = fopen(dump_file, "wb");
            //if (!cmdInfo.fh_dump) {
            //    sprintf(ErrTxt, "Failed to create dump file! (DumpRAM");
            //    return 0;
            //}

	        // fill remote cmd buffer
	        //*((unsigned int *)&cmdInfo.cmdBuf[0]) = dump_start;
	        //*((unsigned int *)&cmdInfo.cmdBuf[4]) = dump_end;
            cmdInfo.cmdSize = 8;
            cmdInfo.cmdBuf = new byte[64];
            WriteUInt32(cmdInfo.cmdBuf, 0, dump_start);
            WriteUInt32(cmdInfo.cmdBuf, 4, dump_end);

	        //init progress bar
	        dump_size = dump_end - dump_start;
	        //UpdateProgressBar(PBM_SETRANGE, 0, MAKELPARAM(0, dump_size/8192));
	        //UpdateProgressBar(PBM_SETSTEP, 1, 0);
	        //UpdateStatusBar("Dumping Memory...", 0, 0);

            cmdInfo.outBuffer = new byte[dump_size];

	        // send remote cmd
            SendReceiveThread(cmdInfo);
	        //HANDLE ioThread = CreateThread(NULL, 0, SendReceiveThread, &cmdInfo, 0, NULL); // no stack, 1MB by default

            return cmdInfo.outBuffer;
}










        void WriteUInt32(byte[] a, int off, uint val)
        {
            byte[] b = BitConverter.GetBytes(val);
            //Array.Reverse(b);
            Array.Copy(b, 0, a, off, b.Length);
        }

        void WriteUInt16(byte[] a, int off, ushort val)
        {
            byte[] b = BitConverter.GetBytes(val);
            //Array.Reverse(b);
            Array.Copy(b, 0, a, off, b.Length);
        }

        ushort ReadUInt16(byte[] a, int off)
        {
            return (ushort)((a[off] << 0) | (a[off + 1] << 8));
        }

        uint ReadUInt32(byte[] a, int off)
        {
            return (uint)((a[off] << 0) | (a[off + 1] << 8) | (a[off + 2] << 16) | (a[off + 3] << 24));
        }
    }
}
