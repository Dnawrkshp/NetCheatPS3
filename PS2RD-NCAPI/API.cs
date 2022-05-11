using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAppInterface;

namespace PS2RD_NCAPI
{
    public class API : IAPI
    {
        public API()
        {

        }

        private string ipAddr = "127.0.0.1";
        private string curPath = "";
        private lib_ntpb ntpb = new lib_ntpb();

        //Declarations of all our internal API variables
        string myName = "PS2rd API";
        string myDescription = "API to use with the debugger part of PS2rd (developed by misfire).";
        string myAuthor = "Dnawrkshp";
        string myVersion = "1.0";
        string myPlatform = "PS2";
        string myContactLink = "";

        //If you want an Icon, use resources to load an image
        //System.Drawing.Image myIcon = Properties.Resources.ico;
        System.Drawing.Image myIcon = null;


        private void CallCommand(string arg)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo proc1 = new System.Diagnostics.ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = curPath;
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Arguments = "/c " + arg;
            proc1.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo = proc1;
            proc.Start();

            proc.WaitForExit();
        }


        /// <summary>
        /// Website link to contact info or download (leave "" if no link)
        /// </summary>
        public string ContactLink
        {
            get { return myContactLink; }
        }

        /// <summary>
        /// Name of the API (displayed on title bar of NetCheat)
        /// </summary>
        public string Name
        {
            get { return myName; }
        }

        /// <summary>
        /// Description of the API's purpose
        /// </summary>
        public string Description
        {
            get { return myDescription; }
        }

        /// <summary>
        /// Author(s) of the API
        /// </summary>
        public string Author
        {
            get { return myAuthor; }

        }

        /// <summary>
        /// Current version of the API
        /// </summary>
        public string Version
        {
            get { return myVersion; }
        }

        /// <summary>
        /// Name of platform (abbreviated, i.e. PC, PS3, XBOX, iOS)
        /// </summary>
        public string Platform
        {
            get { return myPlatform; }
        }

        /// <summary>
        /// Returns whether the platform is little endian by default
        /// </summary>
        public bool isPlatformLittleEndian
        {
            get { return true; }
        }

        /// <summary>
        /// Icon displayed along with the other data in the API tab, if null NetCheat icon is displayed
        /// </summary>
        public System.Drawing.Image Icon
        {
            get { return myIcon; }
        }

        /// <summary>
        /// Read bytes from memory of target process.
        /// Returns read bytes into bytes array.
        /// Returns false if failed.
        /// </summary>
        
        public bool GetBytes(ulong address, ref byte[] bytes)
        {
            //Only accept EE searches for now
            ulong endAddr = address + (ulong)bytes.Length;
            if (address < 0x00080000 || address >= 0x02000000)
                return false;
            if (endAddr < 0x00080000 || endAddr >= 0x02000000)
                return false;

            if (true)
            {
                ulong dSize = endAddr - address;
                if (dSize < 0x1000)
                {
                    endAddr = address + 0x1000;
                }

                string arg = "ntpbclient.exe -D " + address.ToString("X8") + " " + endAddr.ToString("X8") + " temp.bin -ip " + ipAddr;
                CallCommand(arg);

                if (System.IO.File.Exists(curPath + "\\temp.bin"))
                {
                    if (dSize < 0x1000)
                    {
                        System.IO.FileStream fs = System.IO.File.OpenRead(curPath + "\\temp.bin");

                        for (ulong x = 0; x < dSize; x++)
                        {
                            bytes[x] = (byte)fs.ReadByte();
                        }

                        fs.Close();
                    }
                    else
                        bytes = System.IO.File.ReadAllBytes(curPath + "\\temp.bin");
                    System.IO.File.Delete(curPath + "\\temp.bin");

                    return true;
                }
                else
                    return false;
            }
            else
            {
                bytes = ntpb.DumpRAM((uint)address, (uint)endAddr);
                if (bytes != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Write bytes to the memory of target process.
        /// </summary>
        public void SetBytes(ulong address, byte[] bytes)
        {
            if (address < 0x00080000 || address >= 0x02000000)
                return;
            if ((address + (ulong)bytes.Length) < 0x00080000 || (address + (ulong)bytes.Length) >= 0x02000000)
                return;

            string arg = "ntpbclient.exe -W " + address.ToString("X8") + " " + BitConverter.ToString(bytes).Replace("-", "") + " -ip " + ipAddr;
            CallCommand(arg);
        }

        /// <summary>
        /// Shutdown game or platform
        /// </summary>
        public void Shutdown()
        {
            //Shutdown game or platform
        }

        /// <summary>
        /// Connects to target.
        /// If platform doesn't require connection, just return true.
        /// IMPORTANT:
        /// Since NetCheat connects and attaches a few times after the user does (Constant write thread, searching, ect)
        /// You must have it automatically use the settings that the user input, instead of asking again
        /// This can be reset on Disconnect()
        /// </summary>
        public bool Connect()
        {
            //Connect code

            ipAddr = "192.168.137.100";

            return ntpb.ConnectToPS2rd(ipAddr);
        }

        /// <summary>
        /// Disconnects from target.
        /// </summary>
        public void Disconnect()
        {
            //Disconnect code
            //Reset API (for connect and attach user input)

            
        }

        /// <summary>
        /// Attaches to target process.
        /// This should automatically continue the process if it is stopped.
        /// IMPORTANT:
        /// Since NetCheat connects and attaches a few times after the user does (Constant write thread, searching, ect)
        /// You must have it automatically use the settings that the user input, instead of asking again
        /// This can be reset on Disconnect()
        /// </summary>
        public bool Attach()
        {
            //Put attach code here

            return true;
        }

        /// <summary>
        /// Pauses the attached process (return false if not available feature)
        /// </summary>
        public bool PauseProcess()
        {
            string arg = "ntpbclient.exe -H -ip " + ipAddr;
            CallCommand(arg);

            return true;
        }

        /// <summary>
        /// Continues the attached process (return false if not available feature)
        /// </summary>
        public bool ContinueProcess()
        {
            string arg = "ntpbclient.exe -R -ip " + ipAddr;
            CallCommand(arg);

            return true;
        }

        /// <summary>
        /// Tells NetCheat if the process is currently stopped (return false if not available feature)
        /// </summary>
        public bool isProcessStopped()
        {
            return false;
        }

        /// <summary>
        /// Called by user.
        /// Should display options for the API.
        /// Can be used for other things.
        /// </summary>
        public void Configure()
        {

        }

        /// <summary>
        /// Called on initialization
        /// </summary>
        public void Initialize()
        {
            curPath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }

        /// <summary>
        /// Called when disposed
        /// </summary>
        public void Dispose()
        {
            //Put any cleanup code in here for when the program is stopped
        }
    }
}
