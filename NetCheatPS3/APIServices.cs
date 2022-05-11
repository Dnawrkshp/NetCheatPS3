using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using NCAppInterface;

namespace NetCheatPS3
{
    public class Extension //From PS3Lib (by iMCSx)
    {
        /// <summary>Read a signed byte.</summary>
        public static sbyte ReadSByte(ulong offset)
        {
            byte[] buffer = new byte[1];
            GetMem(offset, buffer);
            return (sbyte)buffer[0];
        }

        /// <summary>Read a byte a check if his value. This return a bool according the byte detected.</summary>
        public static bool ReadBool(ulong offset)
        {
            byte[] buffer = new byte[1];
            GetMem(offset, buffer);
            return buffer[0] != 0;
        }

        /// <summary>Read and return an integer 16 bits.</summary>
        public static short ReadInt16(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 2);
            buffer = misc.revif(buffer);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>Read and return an integer 32 bits.</summary>
        public static int ReadInt32(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 4);
            buffer = misc.revif(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>Read and return an integer 64 bits.</summary>
        public static long ReadInt64(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 8);
            buffer = misc.revif(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>Read and return a byte.</summary>
        public static byte ReadByte(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 1);
            return buffer[0];
        }

        /// <summary>Read a string with a length to the first byte equal to an value null (0x00).</summary>
        public static byte[] ReadBytes(ulong offset, int length)
        {
            byte[] buffer = GetBytes(offset, (uint)length);
            return buffer;
        }

        /// <summary>Read and return an unsigned integer 16 bits.</summary>
        public static ushort ReadUInt16(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 2);
            buffer = misc.revif(buffer);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>Read and return an unsigned integer 32 bits.</summary>
        public static uint ReadUInt32(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 4);
            buffer = misc.revif(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>Read and return an unsigned integer 64 bits.</summary>
        public static ulong ReadUInt64(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 8);
            buffer = misc.revif(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>Read and return a Float.</summary>
        public static float ReadFloat(ulong offset)
        {
            byte[] buffer = GetBytes(offset, 4);
            buffer = misc.revif(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>Read a string very fast and stop only when a byte null is detected (0x00).</summary>
        public static string ReadString(ulong offset)
        {
            int block = 40;
            int addOffset = 0;
            string str = "";
        repeat:
            byte[] buffer = ReadBytes(offset + (uint)addOffset, block);
            buffer = misc.notrevif(buffer);
            str += Encoding.UTF8.GetString(buffer);
            addOffset += block;
            if (str.Contains('\0'))
            {
                int index = str.IndexOf('\0');
                string final = str.Substring(0, index);
                str = String.Empty;
                return final;
            }
            else
                goto repeat;
        }

        /// <summary>Write a signed byte.</summary>
        public static void WriteSByte(ulong offset, sbyte input)
        {
            byte[] buff = new byte[1];
            buff[0] = (byte)input;
            SetMem(offset, buff);
        }

        /// <summary>Write a boolean.</summary>
        public static void WriteBool(ulong offset, bool input)
        {
            byte[] buff = new byte[1];
            buff[0] = input ? (byte)1 : (byte)0;
            SetMem(offset, buff);
        }

        /// <summary>Write an interger 16 bits.</summary>
        public static void WriteInt16(ulong offset, short input)
        {
            byte[] buff = new byte[2];
            misc.revif(BitConverter.GetBytes(input)).CopyTo(buff, 0);
            SetMem(offset, buff);
        }

        /// <summary>Write an integer 32 bits.</summary>
        public static void WriteInt32(ulong offset, int input)
        {
            byte[] buff = new byte[4];
            misc.revif(BitConverter.GetBytes(input)).CopyTo(buff, 0);
            
            SetMem(offset, buff);
        }

        /// <summary>Write an integer 64 bits.</summary>
        public static void WriteInt64(ulong offset, long input)
        {
            byte[] buff = new byte[8];
            misc.revif(BitConverter.GetBytes(input)).CopyTo(buff, 0);
            SetMem(offset, buff);
        }

        /// <summary>Write a byte.</summary>
        public static void WriteByte(ulong offset, byte input)
        {
            byte[] buff = new byte[1];
            buff[0] = input;
            SetMem(offset, buff);
        }

        /// <summary>Write a byte array.</summary>
        public static void WriteBytes(ulong offset, byte[] input)
        {
            byte[] buff = input;
            SetMem(offset, buff);
        }

        /// <summary>Write a string.</summary>
        public static void WriteString(ulong offset, string input)
        {
            byte[] buff = Encoding.UTF8.GetBytes(input);
            Array.Resize(ref buff, buff.Length + 1);
            SetMem(offset, buff);
        }

        /// <summary>Write an unsigned integer 16 bits.</summary>
        public static void WriteUInt16(ulong offset, ushort input)
        {
            byte[] buff = new byte[2];
            BitConverter.GetBytes(input).CopyTo(buff, 0);
            buff = misc.revif(buff);
            SetMem(offset, buff);
        }

        /// <summary>Write an unsigned integer 32 bits.</summary>
        public static void WriteUInt32(ulong offset, uint input)
        {
            byte[] buff = new byte[4];
            misc.revif(BitConverter.GetBytes(input)).CopyTo(buff, 0);
            SetMem(offset, buff);
        }

        /// <summary>Write an unsigned integer 64 bits.</summary>
        public static void WriteUInt64(ulong offset, ulong input)
        {
            byte[] buff = new byte[8];
            misc.revif(BitConverter.GetBytes(input)).CopyTo(buff, 0);
            SetMem(offset, buff);
        }

        /// <summary>Write a Float.</summary>
        public static void WriteFloat(ulong offset, float input)
        {
            byte[] buff = new byte[4];
            misc.revif(BitConverter.GetBytes(input)).CopyTo(buff, 0);
            SetMem(offset, buff);
        }

        private static void SetMem(ulong Address, byte[] buffer)
        {
            Form1.curAPI.Instance.SetBytes(Address, buffer);
        }

        private static void GetMem(ulong offset, byte[] buffer)
        {
            Form1.curAPI.Instance.GetBytes(offset, ref buffer);
        }

        private static byte[] GetBytes(ulong offset, uint length)
        {
            byte[] buffer = new byte[length];
            Form1.curAPI.Instance.GetBytes(offset, ref buffer);
            return buffer;
        }
    }


    public class APIServices
    {

        /// <summary>
		/// Constructor of the Class
		/// </summary>
        public APIServices()
		{
		}

        private Types.AvailableAPIs colAvailableAPIs = new Types.AvailableAPIs();

        /// <summary>
        /// A Collection of all Plugins Found and Loaded by the FindPlugins() Method
        /// </summary>
        public Types.AvailableAPIs AvailableAPIs
        {
            get { return colAvailableAPIs; }
            set { colAvailableAPIs = value; }
        }

        /// <summary>
        /// Searches the Application's Startup Directory for Plugins
        /// </summary>
        public void FindAPIs()
        {
            FindAPIs(AppDomain.CurrentDomain.BaseDirectory);
        }
        /// <summary>
        /// Searches the passed Path for Plugins
        /// </summary>
        /// <param name="Path">Directory to search for Plugins in</param>
        public void FindAPIs(string Path)
        {
            //First empty the collection, we're reloading them all
            colAvailableAPIs.Clear();

            string[] fileNames = new string[0];
            //Go through all the files in the plugin directory
            foreach (string fileOn in Directory.GetFiles(Path, "*.dll", SearchOption.AllDirectories))
            {
                bool pass = true;
                for (int x = 0; x < fileNames.Length; x++)
                {
                    if (fileNames[x] == misc.FileOf(fileOn) || misc.FileOf(fileOn).Equals("NCAppInterface.dll"))
                        pass = false;
                }
                //Add the 'plugin'
                if (pass)
                {
                    this.AddAPI(fileOn);
                    Array.Resize(ref fileNames, fileNames.Length + 1);
                    fileNames[fileNames.Length - 1] = misc.FileOf(fileOn);
                }
            }
        }

        private void AddAPI(string FileName)
        {
            try
            {
                //Create a new assembly from the plugin file we're adding..
                Assembly apiAssembly = Assembly.LoadFrom(FileName);

                //Next we'll loop through all the Types found in the assembly
                foreach (Type apiType in apiAssembly.GetTypes())
                {
                    if (apiType.IsPublic) //Only look at public types
                    {
                        if (!apiType.IsAbstract)  //Only look at non-abstract types
                        {
                            //Gets a type object of the interface we need the plugins to match
                            Type typeInterface = apiType.GetInterface("NCAppInterface.IAPI", true);

                            //Make sure the interface we want to use actually exists
                            if (typeInterface != null)
                            {
                                //Create a new available plugin since the type implements the IPlugin interface
                                Types.AvailableAPI newAPI = new Types.AvailableAPI();

                                //Set the filename where we found it
                                newAPI.AssemblyPath = FileName;

                                //Create a new instance and store the instance in the collection for later use
                                //We could change this later on to not load an instance.. we have 2 options
                                //1- Make one instance, and use it whenever we need it.. it's always there
                                //2- Don't make an instance, and instead make an instance whenever we use it, then close it
                                //For now we'll just make an instance of all the plugins
                                newAPI.Instance = (IAPI)Activator.CreateInstance(apiAssembly.GetType(apiType.ToString()));


                                //Add the new plugin to our collection here
                                this.colAvailableAPIs.Add(newAPI);

                                //cleanup a bit
                                newAPI = null;
                            }

                            typeInterface = null; //Mr. Clean			
                        }
                    }
                }

                apiAssembly = null; //more cleanup
            }
            catch (Exception e)
            {

            }
        }
        
    }
}
