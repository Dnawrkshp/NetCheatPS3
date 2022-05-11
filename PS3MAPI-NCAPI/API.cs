using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAppInterface;

namespace PS3MAPI_NCAPI
{
    public class API : IAPI
    {

        public API()
        {

        }

        //Declarations of all our internal API variables
        string myName = "PS3 Manager API";
        string myDescription = "NetCheat API for the PS3 Manager API (PS3).\nThis does not support range finding! All memory is assumed mapped.\n\nCEX and DEX supported!";
        string myAuthor = "NzV";
        string myVersion = "1.0";
        string myPlatform = "PS3";
        string myContactLink = "http://www.nextgenupdate.com/forums/ps3-cheats-customization/788117-ps3-manager-api.html";
        System.Drawing.Image myIcon =  Properties.Resources.ico;

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
            get { return false; }
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
            if (_ps3mapi == null)
                _ps3mapi = new PS3MAPI();

            bytes[0] = 1;
            _ps3mapi.Process.Memory.Get(_ps3mapi.Process.Process_Pid, (uint)address, bytes);

            return true;
        }

        /// <summary>
        /// Write bytes to the memory of target process.
        /// </summary>
        public void SetBytes(ulong address, byte[] bytes)
        {
            if (_ps3mapi == null)
                _ps3mapi = new PS3MAPI();

            _ps3mapi.Process.Memory.Set(_ps3mapi.Process.Process_Pid, (uint)address, bytes);
        }

        /// <summary>
        /// Shutdown game or platform
        /// </summary>
        public void Shutdown()
        {
            if (_ps3mapi == null)
                _ps3mapi = new PS3MAPI();

            _ps3mapi.PS3.Power(PS3MAPI.PS3_CMD.PowerFlags.ShutDown);
        }

        private PS3MAPI _ps3mapi;
        /// <summary>
        /// Connects to target.
        /// If platform doesn't require connection, just return true.
        /// </summary>
        public bool Connect()
        {
            if (_ps3mapi == null)
                _ps3mapi = new PS3MAPI();


            bool ret = false;
            if (_ps3mapi.IPAddr == "127.0.0.1")
                ret = _ps3mapi.ConnectTarget();
            else
                ret = _ps3mapi.ConnectTarget(_ps3mapi.IPAddr);

            return ret;
        }

        /// <summary>
        /// Disconnects from target.
        /// </summary>
        public void Disconnect()
        {
            if (_ps3mapi == null)
                _ps3mapi = new PS3MAPI();

            _ps3mapi.DisconnectTarget();

            _ps3mapi = new PS3MAPI();
        }

        /// <summary>
        /// Attaches to target process.
        /// This should automatically continue the process if it is stopped.
        /// </summary>
        public bool Attach()
        {
            if (_ps3mapi == null)
                _ps3mapi = new PS3MAPI();

            if (_ps3mapi.Process.Process_Pid > 0)
                return _ps3mapi.AttachProcess(_ps3mapi.Process.Process_Pid);
            return _ps3mapi.AttachProcess();
        }

        /// <summary>
        /// Pauses the attached process (return false if not available feature)
        /// </summary>
        public bool PauseProcess()
        {
            return false;
        }

        /// <summary>
        /// Continues the attached process (return false if not available feature)
        /// </summary>
        public bool ContinueProcess()
        {
            return false;
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
            //To-do
            //Add form for all the extra functionality PS3MAPI offers
            //Including Temperature, disabling syscalls, notifications, ect
        }

        /// <summary>
        /// Called on initialization
        /// </summary>
        public void Initialize()
        {

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
