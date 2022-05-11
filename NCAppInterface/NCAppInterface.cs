using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCAppInterface
{
    /// <summary>
    /// Interface for the API.cs file in all NetCheat PS3 APIs.
    /// Contains all elements NetCheat needs for the API.
    /// </summary>
    public interface IAPI
    {

        /// <summary>
        /// Website link to contact info or download (leave "" if no link)
        /// </summary>
        string ContactLink { get; }

        /// <summary>
        /// Name of the API (displayed on title bar of NetCheat)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the API's purpose
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the API
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the API
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Name of platform (abbreviated, i.e. PC, PS3, XBOX, iOS)
        /// </summary>
        string Platform { get; }

        /// <summary>
        /// Returns whether the platform is little endian by default
        /// </summary>
        bool isPlatformLittleEndian { get; }

        /// <summary>
        /// Icon displayed along with the other data in the API tab, if null NetCheat icon is displayed
        /// </summary>
        System.Drawing.Image Icon { get; }

        /// <summary>
        /// Read bytes from memory of target process.
        /// Returns read bytes into bytes array.
        /// Returns false if failed.
        /// </summary>
        bool GetBytes(ulong address, ref byte[] bytes);

        /// <summary>
        /// Write bytes to the memory of target process.
        /// </summary>
        void SetBytes(ulong address, byte[] bytes);

        /// <summary>
        /// Shutdown game or platform
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Connects to target.
        /// If platform doesn't require connection, just return true.
        /// </summary>
        bool Connect();

        /// <summary>
        /// Disconnects from target.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Attaches to target process.
        /// This should automatically continue the process if it is stopped.
        /// </summary>
        bool Attach();

        /// <summary>
        /// Pauses the attached process (return false if not available feature)
        /// </summary>
        bool PauseProcess();

        /// <summary>
        /// Continues the attached process (return false if not available feature)
        /// </summary>
        bool ContinueProcess();

        /// <summary>
        /// Tells NetCheat if the process is currently stopped (return false if not available feature)
        /// </summary>
        bool isProcessStopped();

        /// <summary>
        /// Called by user.
        /// Should display options for the API.
        /// Can be used for other things.
        /// </summary>
        void Configure();

        /// <summary>
        /// Called on initialization
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when disposed
        /// </summary>
        void Dispose();
    }
}
