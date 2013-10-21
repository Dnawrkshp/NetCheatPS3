/*
 * By "Redth"
 * http://www.codeproject.com/Articles/6334/Plug-ins-in-C
 */

using System;

namespace PluginInterface
{
	public interface IPlugin
	{
		IPluginHost Host {
            get;
            set;
        }
		
		string Name {get;}
		string Description {get;}
		string Author {get;}
		string Version {get;}
        string TabText {get;}

		System.Windows.Forms.UserControl MainInterface {get;}
        System.Windows.Forms.UserControl MainIcon { get; }
		
		void Initialize();
		void Dispose();
	}
	
	public interface IPluginHost
	{
        /// <summary>
        /// Gets the memory at addr of length size from the PS3 and returns the result as a byte array
        /// </summary>
        /// <param name="addr">Address to read from</param>
        /// <param name="size">Number of bytes to read</param>
        byte[] GetMemory(ulong addr, int size);
        /// <summary>
        /// Gets the memory at addr of length ret.Length from the PS3 and returns the result in ret
        /// </summary>
        /// <param name="addr">Address to read from</param>
        /// <param name="ret">Read byte array</param>
        void GetMemory(ulong addr, ref byte[] ret);

        /// <summary>
        /// Sets the memory at addr to the contents of val
        /// </summary>
        /// <param name="addr">Address to read from</param>
        /// <param name="val">Bytes to write</param>
        void SetMemory(ulong addr, byte[] val);

        /// <summary>
        /// Converts a byte array to an unsigned long
        /// </summary>
        /// <param name="val">Byte array to convert</param>
        /// <param name="index">Index of array to start at</param>
        /// <param name="size">Number of bytes to convert</param>
        ulong ByteAToULong(byte[] val, int index, int size);

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="val">Byte array to convert</param>
        /// <param name="split">String to insert between each character</param>
        string ByteAToString(byte[] val, string split);

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="text">String to convert to a byte array</param>
        byte[] StringToByteA(string text);

        /// <summary>
        /// Returns the left most substring in text of length length
        /// </summary>
        /// <param name="text">String to grab left most substring from</param>
        /// <param name="length">Number of characters to grab</param>
        string sLeft(string text, int length);

        /// <summary>
        /// Returns the right most substring in text of length length
        /// </summary>
        /// <param name="text">String to grab right most substring from</param>
        /// <param name="length">Number of characters to grab</param>
        string sRight(string text, int length);

        /// <summary>
        /// Returns the substring at off of length length
        /// </summary>
        /// <param name="text">String to grab substring from</param>
        /// <param name="off">Substring offset</param>
        /// <param name="length">Number of characters to grab</param>
        string sMid(string text, int off, int length);

        /// <summary>
        /// Returns the ID of the Constant Code added
        /// </summary>
        /// <param name="code">NetCheat codes as a string</param>
        /// <param name="state">Whether to constantly write or not</param>
        uint ConstCodeAdd(string code, bool state);

        /// <summary>
        /// Removes the code with an ID of ID
        /// </summary>
        /// <param name="ID">Code identifier</param>
        void ConstCodeRemove(uint ID);

        /// <summary>
        /// Sets whether the code is constantly written or not
        /// </summary>
        /// <param name="ID">Code identifier</param>
        /// <param name="state">Whether to constantly write or not</param>
        void ConstCodeSetState(uint ID, bool state);

        /// <summary>
        /// Returns whether the code is constantly writting or not
        /// </summary>
        /// <param name="ID">Code identifier</param>
        bool ConstCodeGetState(uint ID);

        /// <summary>
        /// Attempts to connect and attack to the PS3.
        /// If it fails it will return false, otherwise it will return true.
        /// Only to be used if another thread is created.
        /// </summary>
        bool ConnectAndAttach();

        /// <summary>
        /// Returns the current connection state of NetCheat's main thread.
        /// </summary>
        int ConnectionState();
	}
}
