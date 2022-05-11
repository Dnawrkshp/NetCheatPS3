using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAppInterface;

namespace CCAPI_NCAPI
{
    public class API : IAPI
    {

        private bool enabled = false;
        public API()
        {

        }

        //Declarations of all our internal API variables
        string myName = "Control Console API";
        string myDescription = "NetCheat API for the Control Console API (PS3).\n\nCEX and DEX supported!";
        string myAuthor = "Enstone and iMCSx";
        string myVersion = "2.60";
        string myPlatform = "PS3";
        string myContactLink = "http://www.nextgenupdate.com/forums/ps3-cheats-customization/693857-controlconsoleapi-2-60-rev3-ccapi-4-70-a-37.html";
        System.Drawing.Image myIcon = Properties.Resources.ico;

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
            if (!enabled)
                return false;
            if (_ccapi == null)
                _ccapi = new CCAPI();

            return _ccapi.GetMemory((uint)address, bytes) >= 0;
        }

        /// <summary>
        /// Write bytes to the memory of target process.
        /// </summary>
        public void SetBytes(ulong address, byte[] bytes)
        {
            if (!enabled)
                return;
            if (_ccapi == null)
                _ccapi = new CCAPI();

            _ccapi.SetMemory((uint)address, bytes);
        }

        /// <summary>
        /// Shutdown game or platform
        /// </summary>
        public void Shutdown()
        {
            if (!enabled)
                return;
            if (_ccapi == null)
                _ccapi = new CCAPI();

            _ccapi.ShutDown(CCAPI.RebootFlags.ShutDown);
        }

        private CCAPI _ccapi;
        private string ip = "";
        /// <summary>
        /// Connects to target.
        /// If platform doesn't require connection, just return true.
        /// </summary>
        public bool Connect()
        {
            if (!enabled)
                return false;

            if (_ccapi == null)
                _ccapi = new CCAPI();

            if (ip == "")
            {
                bool ret = _ccapi.ConnectTarget();
                ip = _ccapi.ps3api.GetConsoleIP();
                return ret;
            }
            else
                return _ccapi.ConnectTarget(ip) >= 0;
        }

        /// <summary>
        /// Disconnects from target.
        /// </summary>
        public void Disconnect()
        {
            if (!enabled)
                return;
            if (_ccapi == null)
                _ccapi = new CCAPI();

            _ccapi.DisconnectTarget();

            _ccapi = new CCAPI();
        }

        /// <summary>
        /// Attaches to target process.
        /// This should automatically continue the process if it is stopped.
        /// </summary>
        public bool Attach()
        {
            if (_ccapi == null)
                _ccapi = new CCAPI();

            return _ccapi.AttachProcess(CCAPI.ProcessType.CURRENTGAME) >= 0;
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
            if (_ccapi == null)
                _ccapi = new CCAPI();

            _ccapi.OpenManager();
        }

        /// <summary>
        /// Called on initialization
        /// </summary>
        public void Initialize()
        {
            if (IntPtr.Size == 8)
            {
                System.Windows.Forms.MessageBox.Show("CCAPI does not support 64 bit mode!\nPlease switch to 32 bit NetCheat.");
            }
            else
                enabled = true;
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
