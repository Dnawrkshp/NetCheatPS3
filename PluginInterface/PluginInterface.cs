/*
 * By "Redth"
 * http://www.codeproject.com/Articles/6334/Plug-ins-in-C
 */

using System;

namespace PluginInterface
{
    /// <summary>
    /// Interface for the Plugin.cs file in all NetCheat PS3 plugins.
    /// Contains all elements NetCheat needs to gather information about the plugin.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Used by NetCheat to reference the current instance of NetCheat.
        /// This allows for plugins to use internal functions of NetCheat PS3.
        /// </summary>
        IPluginHost Host
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the plugin displayed in the plugin info window
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the Plugin's purpose
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author of the plugin
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the plugin
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Text displayed on the top of the plugin form and the listbox
        /// </summary>
        string TabText { get; }

        /// <summary>
        /// UserControl that gets displayed when the plugin is opened
        /// </summary>
        System.Windows.Forms.UserControl MainInterface { get; }

        /// <summary>
        /// Icon displayed along with the other data in the Plugins tab
        /// </summary>
        System.Windows.Forms.UserControl MainIcon { get; }

        /// <summary>
        /// Called on initialization
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when disposed
        /// </summary>
        void Dispose();
    }

    /* 
     * From PS3Lib
     * Credits to iMCSx
     */

    /// <summary>
    /// Copy of PS3Lib's NotifyIcon enum.
    /// Defines the options for NotifyPS3().
    /// </summary>
    public enum NotifyIcon
    {
        /// <summary>Info icon</summary>
        INFO,
        /// <summary>Caution icon</summary>
        CAUTION,
        /// <summary>Friend icon</summary>
        FRIEND,
        /// <summary>Slider icon</summary>
        SLIDER,
        /// <summary>Wrongway icon</summary>
        WRONGWAY,
        /// <summary>Dialog icon</summary>
        DIALOG,
        /// <summary>Dialog icon's shadow</summary>
        DIALOGSHADOW,
        /// <summary>Text icon</summary>
        TEXT,
        /// <summary>Pointer icon</summary>
        POINTER,
        /// <summary>Grab icon</summary>
        GRAB,
        /// <summary>Hand icon</summary>
        HAND,
        /// <summary>Pen icon</summary>
        PEN,
        /// <summary>Finger icon</summary>
        FINGER,
        /// <summary>Arrow icon</summary>
        ARROW,
        /// <summary>Right arrow icon</summary>
        ARROWRIGHT,
        /// <summary>Progress icon</summary>
        PROGRESS
    }

    /// <summary>
    /// Copy of PS3Lib's BuzzerMode enum.
    /// Defines the options for RingBuzzerPS3().
    /// </summary>
    public enum BuzzerMode
    {
        /// <summary>Call the buzzer continiously... Don't do it...</summary>
        Continuous,
        /// <summary>Call the buzzer once</summary>
        Single,
        /// <summary>Call the buzzer twice</summary>
        Double
    }

    /// <summary>
    /// Defines the start and end of a memory range
    /// </summary>
    public struct NCRange
    {
        /// <summary>Start of the memory range</summary>
        public ulong start;
        /// <summary>End of the memory range</summary>
        public ulong end;
    }

    /// <summary>
    /// Host Interface used by the plugin to access functions from within NetCheat PS3.
    /// </summary>
    public interface IPluginHost
    {
        /// <summary>
        /// Gets the memory at addr of length size from the PS3 and returns the result as a byte array.
        /// </summary>
        /// <param name="addr">Address to read from</param>
        /// <param name="size">Number of bytes to read</param>
        byte[] GetMemory(ulong addr, int size);
        /// <summary>
        /// Gets the memory at addr of length ret.Length from the PS3 and returns the result in ret.
        /// </summary>
        /// <param name="addr">Address to read from</param>
        /// <param name="ret">Read byte array</param>
        void GetMemory(ulong addr, ref byte[] ret);

        /// <summary>
        /// Sets the memory at addr to the contents of val.
        /// </summary>
        /// <param name="addr">Address to read from</param>
        /// <param name="val">Bytes to write</param>
        void SetMemory(ulong addr, byte[] val);

        /// <summary>
        /// Converts val to an unsigned long.
        /// </summary>
        /// <param name="val">Byte array to convert</param>
        /// <param name="index">Index of array to start at</param>
        /// <param name="size">Number of bytes to convert</param>
        ulong ByteAToULong(byte[] val, int index, int size);

        /// <summary>
        /// Converts val to a string. { 0x41, 0x42 } => "AB".
        /// </summary>
        /// <param name="val">Byte array to convert</param>
        /// <param name="split">String to insert between each character</param>
        string ByteAToString(byte[] val, string split);

        /// <summary>
        /// Converts text to a byte array. "AB" => { 0x41, 0x42 }.
        /// </summary>
        /// <param name="text">String to convert to a byte array</param>
        byte[] StringToByteA(string text);

        /// <summary>
        /// Returns the left most substring in text of length length.
        /// </summary>
        /// <param name="text">String to grab left most substring from</param>
        /// <param name="length">Number of characters to grab</param>
        string sLeft(string text, int length);

        /// <summary>
        /// Returns the right most substring in text of length length.
        /// </summary>
        /// <param name="text">String to grab right most substring from</param>
        /// <param name="length">Number of characters to grab</param>
        string sRight(string text, int length);

        /// <summary>
        /// Returns the substring at off of length length.
        /// </summary>
        /// <param name="text">String to grab substring from</param>
        /// <param name="off">Substring offset</param>
        /// <param name="length">Number of characters to grab</param>
        string sMid(string text, int off, int length);

        /// <summary>
        /// Returns the ID of the Constant Code added.
        /// </summary>
        /// <param name="code">NetCheat codes as a string</param>
        /// <param name="state">Whether to constantly write or not</param>
        uint ConstCodeAdd(string code, bool state);

        /// <summary>
        /// Removes the code with an ID of ID.
        /// </summary>
        /// <param name="ID">Code identifier</param>
        void ConstCodeRemove(uint ID);

        /// <summary>
        /// Sets whether the code is constantly written or not.
        /// </summary>
        /// <param name="ID">Code identifier</param>
        /// <param name="state">Whether to constantly write or not</param>
        void ConstCodeSetState(uint ID, bool state);

        /// <summary>
        /// Returns whether the code is constantly writing or not.
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

        /// <summary>
        /// Returns true if the current API is TM.
        /// </summary>
        bool isUsingTMAPI();

        /// <summary>
        /// If CCAPI is the current API, it displays a notify icon on the connected PS3.
        /// Returns true if successful.
        /// </summary>
        /// <param name="icon">Specified icon to display</param>
        /// <param name="message">String to be displayed in the notification</param>
        bool NotifyPS3(NotifyIcon icon, string message);

        /// <summary>
        /// If CCAPI is the current API, it rings the buzzer on the connected PS3.
        /// Returns true if successful.
        /// </summary>
        /// <param name="flag">Type of buzzer</param>
        bool RingBuzzerPS3(BuzzerMode flag);

        /// <summary>
        /// If CCAPI is the current API, it returns the temperature of the CPU (CELL).
        /// Returns "" if the current API is not CCAPI.
        /// </summary>
        string GetTemperatureCELL();

        /// <summary>
        /// If CCAPI is the current API, it returns the temperature of the GPU (RSX).
        /// Returns "" if the current API is not CCAPI.
        /// </summary>
        string GetTemperatureRSX();

        /// <summary>
        /// If CCAPI is the current API, it returns the firmware version of the connected PS3.
        /// Returns "" if the current API is not CCAPI.
        /// </summary>
        string GetFirmwareVersion();

        /// <summary>
        /// If CCAPI is the current API, it returns the firmware type of the connected PS3.
        /// Returns "" if the current API is not CCAPI.
        /// </summary>
        string GetFirmwareType();

        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, bool input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, byte input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, byte[] input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, short input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, int input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, long input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, char input);
        /// <summary>
        /// Searches the memory from start to stop checking if it is equal to input.
        /// Returns addresses that hold values equal to input.
        /// Bare in mind that it does take into account the users current range setup
        /// </summary>
        /// <param name="start">Address to start from</param>
        /// <param name="stop">Address to stop at</param>
        /// <param name="align">Alignment of searched value. Basically the addresses compared will be a multiple of this value. (0, 0x10000, 4, input) => 0, 4, 8, C, 10, ..., 0xFFFFC, 0x10000. </param>
        /// <param name="input">Value to compare with</param>
        ulong[] SearchMemory(ulong start, ulong stop, int align, string input);

        /// <summary>
        /// Returns the users current range setup
        /// </summary>
        NCRange[] GetUserRange();

        /// <summary>
        /// If true NetCheat will color the plugins controls to the users color setting, otherwise it will not color the plugin.
        /// Must be called in Initialize() function within Plugin.cs
        /// </summary>
        void SetAllowColoring(bool allowColoring);

        /* 
         * PS3Lib Extension functions
         * Copied summaries directly from the source
         * Credits to iMCSx
         */

        /// <summary>Read a signed byte.</summary>
        /// <param name="offset">Address to read the signed byte from</param>
        sbyte PS3Lib_ReadSByte(uint offset);

        /// <summary>Read a byte a check if his value. This return a bool according the byte detected.</summary>
        /// <param name="offset">Address to read the bool from</param>
        bool PS3Lib_ReadBool(uint offset);

        /// <summary>Read and return an integer 16 bits.</summary>
        /// <param name="offset">Address to read the short from</param>
        short PS3Lib_ReadInt16(uint offset);

        /// <summary>Read and return an integer 32 bits.</summary>
        /// <param name="offset">Address to read the int from</param>
        int PS3Lib_ReadInt32(uint offset);

        /// <summary>Read and return an integer 64 bits.</summary>
        /// <param name="offset">Address to read the long from</param>
        long PS3Lib_ReadInt64(uint offset);

        /// <summary>Read and return a byte.</summary>
        /// <param name="offset">Address to read the byte from</param>
        byte PS3Lib_ReadByte(uint offset);

        /// <summary>Read a string with a length to the first byte equal to an value null (0x00).</summary>
        /// <param name="offset">Address to read the byte[] from</param>
        /// <param name="length">How many bytes to read</param>
        byte[] PS3Lib_ReadBytes(uint offset, int length);

        /// <summary>Read and return an unsigned integer 16 bits.</summary>
        /// <param name="offset">Address to read the ushort from</param>
        ushort PS3Lib_ReadUInt16(uint offset);

        /// <summary>Read and return an unsigned integer 32 bits.</summary>
        /// <param name="offset">Address to read the uint from</param>
        uint PS3Lib_ReadUInt32(uint offset);

        /// <summary>Read and return an unsigned integer 64 bits.</summary>
        /// <param name="offset">Address to read the ulong from</param>
        ulong PS3Lib_ReadUInt64(uint offset);

        /// <summary>Read and return a Float.</summary>
        /// <param name="offset">Address to read the float from</param>
        float PS3Lib_ReadFloat(uint offset);

        /// <summary>Read a string very fast and stop only when a byte null is detected (0x00).</summary>
        /// <param name="offset">Address to read the string from</param>
        string PS3Lib_ReadString(uint offset);

        /// <summary>Write a signed byte.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Signed byte to write at the offset</param>
        void PS3Lib_WriteSByte(uint offset, sbyte input);

        /// <summary>Write a boolean.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Bool to write at the offset</param>
        void PS3Lib_WriteBool(uint offset, bool input);

        /// <summary>Write an integer 16 bits.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Short to write at the offset</param>
        void PS3Lib_WriteInt16(uint offset, short input);

        /// <summary>Write an integer 32 bits.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Int to write at the offset</param>
        void PS3Lib_WriteInt32(uint offset, int input);

        /// <summary>Write an integer 64 bits.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Long to write at the offset</param>
        void PS3Lib_WriteInt64(uint offset, long input);

        /// <summary>Write a byte.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Byte to write at the offset</param>
        void PS3Lib_WriteByte(uint offset, byte input);

        /// <summary>Write a byte array.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Byte[] to write at the offset</param>
        void PS3Lib_WriteBytes(uint offset, byte[] input);

        /// <summary>Write a string.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">String to write at the offset</param>
        void PS3Lib_WriteString(uint offset, string input);

        /// <summary>Write an unsigned integer 16 bits.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">UShort to write at the offset</param>
        void PS3Lib_WriteUInt16(uint offset, ushort input);

        /// <summary>Write an unsigned integer 32 bits.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">UInt to write at the offset</param>
        void PS3Lib_WriteUInt32(uint offset, uint input);

        /// <summary>Write an unsigned integer 64 bits.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">ULong to write at the offset</param>
        void PS3Lib_WriteUInt64(uint offset, ulong input);

        /// <summary>Write a Float.</summary>
        /// <param name="offset">Address to write the input to</param>
        /// <param name="input">Float to write at the offset</param>
        void PS3Lib_WriteFloat(uint offset, float input);

    }
}
