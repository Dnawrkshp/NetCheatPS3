using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAppInterface;

namespace PCAPI_NCAPI
{
    public class API : IAPI
    {
        private MemMan _memman = new MemMan();

        public API()
        {

        }

        //Declarations of all our internal API variables
        string myName = "PC NCAPI 64BIT";
        string myDescription = "NetCheat API for your PC";
        string myAuthor = "Dnawrkshp";
        string myVersion = "1.0.0";
        string myPlatform = "PC";
        string myContactLink = "";

        //If you want an Icon, use resources to load an image
        //System.Drawing.Image myIcon = Properties.Resources.ico;
        System.Drawing.Image myIcon = null;

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
            get { return BitConverter.IsLittleEndian; }
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
            return _memman.ReadMemory((ulong)address, ref bytes);
        }

        /// <summary>
        /// Write bytes to the memory of target process.
        /// </summary>
        public void SetBytes(ulong address, byte[] bytes)
        {
            _memman.WriteMemory((ulong)address, bytes);
        }

        /// <summary>
        /// Shutdown game or platform
        /// </summary>
        public void Shutdown()
        {
            //Shutdown game or platform

            _memman.KillProcess();
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

            return true;
        }

        /// <summary>
        /// Disconnects from target.
        /// </summary>
        public void Disconnect()
        {
            //Disconnect code
            //Reset API (for connect and attach user input)

            _memman = new MemMan();
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

            if (_memman.processId <= 0)
            { //not attached
                AttachForm af = new AttachForm();
                af.ShowDialog();

                if (af.returnProcessID > 0)
                {
                    return _memman.Attach(af.returnProcessID);
                }
                else
                    return false;
            }
            else //is attached
            {
                return true;
            }

            return true;
        }

        /// <summary>
        /// Pauses the attached process (return false if not available feature)
        /// </summary>
        public bool PauseProcess()
        {
            return _memman.PauseProcess();
        }

        /// <summary>
        /// Continues the attached process (return false if not available feature)
        /// </summary>
        public bool ContinueProcess()
        {
            return _memman.ContinueProcess();
        }

        /// <summary>
        /// Tells NetCheat if the process is currently stopped (return false if not available feature)
        /// </summary>
        public bool isProcessStopped()
        {
            return _memman.isSuspended();
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
