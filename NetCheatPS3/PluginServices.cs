/*
 * By "Redth"
 * http://www.codeproject.com/Articles/6334/Plug-ins-in-C
 */

using System;
using System.IO;
using System.Reflection;
using PluginInterface;
using NCAppInterface;

namespace NetCheatPS3 
{
	/// <summary>
	/// Summary description for PluginServices.
	/// </summary>
	public class PluginServices : IPluginHost   //<--- Notice how it inherits IPluginHost interface!
	{
        /* Set Memory */
        public void SetMemory(ulong addr, byte[] val)
        {
            Form1.apiSetMem(addr, val);
        }

        /* Get Memory */
        public byte[] GetMemory(ulong addr, int size)
        {
            byte[] ret = new byte[size];
            Form1.apiGetMem(addr, ref ret);
            return ret;
        }
        public void GetMemory(ulong addr, ref byte[] ret)
        {
            Form1.apiGetMem(addr, ref ret);
        }

        /* Byte Array to ulong */
        public ulong ByteAToULong(byte[] val, int index, int size)
        {
            return misc.ByteArrayToLong(val, index, size);
        }

        /* Byte Array to String */
        public string ByteAToString(byte[] val, string split)
        {
            return misc.ByteAToString(val, split);
        }

        /* String to Byte Array */
        public byte[] StringToByteA(string text)
        {
            return misc.StringToByteArray(text);
        }

        /* Equivalent to VB6's Left function (grabs length many left most characters in text) */
        public string sLeft(string text, int length)
        {
            return misc.sLeft(text, length);
        }

        /* Equivalent to VB6's Right function (grabs length many right most characters in text) */
        public string sRight(string text, int length)
        {
            return misc.sRight(text, length);
        }

        /* Equivalent to VB6's Mid function */
        public string sMid(string text, int off, int length)
        {
            return misc.sMid(text, off, length);
        }

        /* Adds a code to the ConstCodes array */
        public uint ConstCodeAdd(string code, bool state)
        {
            return codes.ConstCodeAdd(code, state);
        }

        /* Removes a code to the ConstCodes array */
        public void ConstCodeRemove(uint ID)
        {
            codes.ConstCodeRemove(ID);
        }

        /* Sets a codes state to state */
        public void ConstCodeSetState(uint ID, bool state)
        {
            codes.ConstCodeSetState(ID, state);
        }

        /* Gets a codes state */
        public bool ConstCodeGetState(uint ID)
        {
            return codes.ConstCodeGetState(ID);
        }

        /* Connects and Attaches to PS3 */
        public bool ConnectAndAttach()
        {
            int connect = codes.ConnectPS3();
            if (connect == 1)
                connect = codes.AttachPS3();

            if (connect != 2)
                return false;
            else
                return true;
        }

        /* Returns the state of the main forms connection */
        public int ConnectionState()
        {
            if (Form1.isClosing)
                return -1;

            if (!Form1.connected)
                return 0;

            byte[] b = new byte[1];
            bool isConnectedAttached = Form1.apiGetMem(0x10000, ref b);

            return (isConnectedAttached) ? 2 : 1;
        }

        public bool isUsingTMAPI()
        {
            if (Form1.curAPI.Instance.Name == "Target Manager API")
                return true;
            return false;
        }

        public bool NotifyPS3(NotifyIcon icon, string message)
        {
            //if (!isUsingTMAPI())
            //{
            //    int val = (int)Enum.Parse(typeof(PluginInterface.NotifyIcon), icon.ToString());
            //    Form1.PS3.CCAPI.Notify(val, message);
            //}
            //else
                return false;

            //return true;
        }

        public bool RingBuzzerPS3(BuzzerMode flag)
        {
            //if (!isUsingTMAPI())
            //{
            //    int val = (int)Enum.Parse(typeof(PluginInterface.BuzzerMode), flag.ToString());
            //    Form1.PS3.CCAPI.RingBuzzer((PS3Lib.CCAPI.BuzzerMode)val);
            //}
            //else
                return false;

            //return true;
        }

        public string GetTemperatureCELL()
        {
            //if (!isUsingTMAPI())
            //{
            //    return Form1.PS3.CCAPI.GetTemperatureCELL();
            //}

            return "";
        }

        public string GetTemperatureRSX()
        {
            //if (!isUsingTMAPI())
            //{
            //    return Form1.PS3.CCAPI.GetTemperatureRSX();
            //}

            return "";
        }

        public string GetFirmwareVersion()
        {
            //if (!isUsingTMAPI())
            //{
            //    return Form1.PS3.CCAPI.GetFirmwareVersion();
            //}

            return "";
        }
        
        public string GetFirmwareType()
        {
            //if (!isUsingTMAPI())
            //{
            //    return Form1.PS3.CCAPI.GetFirmwareType();
            //}
            
            return "";
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, bool input)
        {
            byte[] arg4 = new byte[] { (byte)(input ? 1 : 0) };
            return SearchMemory(start, stop, align, arg4);
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, byte input)
        {
            return SearchMemory(start, stop, align, new byte[] { input });
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, byte[] input)
        {
            if (align <= 0)
                align = 1;

            ulong[] ret = new ulong[0];
            ulong size = 0x10000;

            for (ulong x = start; x < stop; x += size)
            {
                //Get address based off of range
                x = misc.ParseSchAddr(x);

                byte[] mem = GetMemory(x, (int)size);
                for (int y = 0; y < (int)size; y += align)
                {
                    byte[] temp = new byte[input.Length];
                    Array.Copy(mem, y, temp, 0, input.Length);
                    if (misc.ArrayCompare(temp, input, null, Form1.compEq))
                    {
                        Array.Resize(ref ret, ret.Length + 1);
                        ret[ret.Length - 1] = x + (ulong)y;
                    }
                }

                //System.Windows.Forms.Application.DoEvents();
            }

            return ret;
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, short input)
        {
            byte[] arg4 = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(arg4);

            return SearchMemory(start, stop, align, arg4);
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, int input)
        {
            byte[] arg4 = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(arg4);

            return SearchMemory(start, stop, align, arg4);
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, long input)
        {
            byte[] arg4 = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(arg4);

            return SearchMemory(start, stop, align, arg4);
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, char input)
        {
            return SearchMemory(start, stop, align, new byte[] { (byte)input });
        }

        public ulong[] SearchMemory(ulong start, ulong stop, int align, string input)
        {
            byte[] arg4 = StringToByteA(input);
            return SearchMemory(start, stop, align, arg4);
        }

        public NCRange[] GetUserRange()
        {
            if (misc.MemArray == null || misc.MemArray.Length == 0)
            {
                NCRange t = new NCRange();
                t.start = 0;
                t.end = 0xFFFFFFFC;
                return new NCRange[] { t };
            }

            NCRange[] ret = new NCRange[misc.MemArray.Length / 2];
            for (int x = 0; x < misc.MemArray.Length; x += 2)
            {
                ret[x / 2].end = misc.MemArray[x + 1];
                ret[x / 2].start = misc.MemArray[x];
            }

            return ret;
        }

        public void SetAllowColoring(bool allowColoring)
        {
            Form1.PluginAllowColoring = allowColoring;
        }

        public sbyte PS3Lib_ReadSByte(uint offset)
        {
            return Extension.ReadSByte(offset);
        }

        public bool PS3Lib_ReadBool(uint offset)
        {
            return Extension.ReadBool(offset);
        }

        public short PS3Lib_ReadInt16(uint offset)
        {
            return Extension.ReadInt16(offset);
        }

        public int PS3Lib_ReadInt32(uint offset)
        {
            return Extension.ReadInt32(offset);
        }

        public long PS3Lib_ReadInt64(uint offset)
        {
            return Extension.ReadInt64(offset);
        }

        public byte PS3Lib_ReadByte(uint offset)
        {
            return Extension.ReadByte(offset);
        }

        public byte[] PS3Lib_ReadBytes(uint offset, int length)
        {
            return Extension.ReadBytes(offset, length);
        }

        public ushort PS3Lib_ReadUInt16(uint offset)
        {
            return Extension.ReadUInt16(offset);
        }

        public uint PS3Lib_ReadUInt32(uint offset)
        {
            return Extension.ReadUInt32(offset);
        }

        public ulong PS3Lib_ReadUInt64(uint offset)
        {
            return Extension.ReadUInt64(offset);
        }

        public float PS3Lib_ReadFloat(uint offset)
        {
            return Extension.ReadFloat(offset);
        }

        public string PS3Lib_ReadString(uint offset)
        {
            return Extension.ReadString(offset);
        }

        public void PS3Lib_WriteSByte(uint offset, sbyte input)
        {
            Extension.WriteSByte(offset, input);
        }

        public void PS3Lib_WriteBool(uint offset, bool input)
        {
            Extension.WriteBool(offset, input);
        }

        public void PS3Lib_WriteInt16(uint offset, short input)
        {
            Extension.WriteInt16(offset, input);
        }

        public void PS3Lib_WriteInt32(uint offset, int input)
        {
            Extension.WriteInt32(offset, input);
        }

        public void PS3Lib_WriteInt64(uint offset, long input)
        {
            Extension.WriteInt64(offset, input);
        }

        public void PS3Lib_WriteByte(uint offset, byte input)
        {
            Extension.WriteByte(offset, input);
        }

        public void PS3Lib_WriteBytes(uint offset, byte[] input)
        {
            Extension.WriteBytes(offset, input);
        }

        public void PS3Lib_WriteString(uint offset, string input)
        {
            Extension.WriteString(offset, input);
        }

        public void PS3Lib_WriteUInt16(uint offset, ushort input)
        {
            Extension.WriteUInt16(offset, input);
        }

        public void PS3Lib_WriteUInt32(uint offset, uint input)
        {
            Extension.WriteUInt32(offset, input);
        }

        public void PS3Lib_WriteUInt64(uint offset, ulong input)
        {
            Extension.WriteUInt64(offset, input);
        }

        public void PS3Lib_WriteFloat(uint offset, float input)
        {
            Extension.WriteFloat(offset, input);
        }

		/// <summary>
		/// Constructor of the Class
		/// </summary>
		public PluginServices()
		{
		}
	
		private Types.AvailablePlugins colAvailablePlugins = new Types.AvailablePlugins();
		
		/// <summary>
		/// A Collection of all Plugins Found and Loaded by the FindPlugins() Method
		/// </summary>
		public Types.AvailablePlugins AvailablePlugins
		{
			get {return colAvailablePlugins;}
			set {colAvailablePlugins = value;}
		}
		
		/// <summary>
		/// Searches the Application's Startup Directory for Plugins
		/// </summary>
		public void FindPlugins()
		{
			FindPlugins(AppDomain.CurrentDomain.BaseDirectory);
		}
		/// <summary>
		/// Searches the passed Path for Plugins
		/// </summary>
		/// <param name="Path">Directory to search for Plugins in</param>
		public void FindPlugins(string Path)
		{
			//First empty the collection, we're reloading them all
			colAvailablePlugins.Clear();

            string[] fileNames = new string[0];
			//Go through all the files in the plugin directory
            foreach (string fileOn in Directory.GetFiles(Path, "*.dll", SearchOption.AllDirectories))
			{
                bool pass = true;
                for (int x = 0; x < fileNames.Length; x++)
                {
                    if (fileNames[x] == misc.FileOf(fileOn) || misc.FileOf(fileOn).Equals("PluginInterface.dll"))
                        pass = false;
                }
				//Add the 'plugin'
                if (pass) {
                    try
                    {
                        this.AddPlugin(fileOn);
                        Array.Resize(ref fileNames, fileNames.Length + 1);
                        fileNames[fileNames.Length - 1] = misc.FileOf(fileOn);
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.Message, e.Source);
                    }
                }
			}
		}
		
		/// <summary>
		/// Unloads and Closes all AvailablePlugins
		/// </summary>
		public void ClosePlugins()
		{
			foreach (Types.AvailablePlugin pluginOn in colAvailablePlugins)
			{
				//Close all plugin instances
				//We call the plugins Dispose sub first incase it has to do 
				//Its own cleanup stuff
				pluginOn.Instance.Dispose(); 
				
				//After we give the plugin a chance to tidy up, get rid of it
				pluginOn.Instance = null;
			}
			
			//Finally, clear our collection of available plugins
			colAvailablePlugins.Clear();

            //Form1.pluginDomain = AppDomain.CreateDomain("NC Plugin Domain");
		}

		private void AddPlugin(string FileName)
		{
			//Create a new assembly from the plugin file we're adding..
			Assembly pluginAssembly = Assembly.LoadFrom(FileName);
			
			//Next we'll loop through all the Types found in the assembly
			foreach (Type pluginType in pluginAssembly.GetTypes())
			{
				if (pluginType.IsPublic) //Only look at public types
				{
					if (!pluginType.IsAbstract)  //Only look at non-abstract types
					{
						//Gets a type object of the interface we need the plugins to match
						Type typeInterface = pluginType.GetInterface("PluginInterface.IPlugin", true);
						
						//Make sure the interface we want to use actually exists
						if (typeInterface != null)
						{
							//Create a new available plugin since the type implements the IPlugin interface
							Types.AvailablePlugin newPlugin = new Types.AvailablePlugin();
							
							//Set the filename where we found it
							newPlugin.AssemblyPath = FileName;
							
							//Create a new instance and store the instance in the collection for later use
							//We could change this later on to not load an instance.. we have 2 options
							//1- Make one instance, and use it whenever we need it.. it's always there
							//2- Don't make an instance, and instead make an instance whenever we use it, then close it
							//For now we'll just make an instance of all the plugins
                            newPlugin.Instance = (IPlugin)Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString()));

							//Set the Plugin's host to this class which inherited IPluginHost
							newPlugin.Instance.Host = this;

							//Call the initialization sub of the plugin
							//newPlugin.Instance.Initialize();
							
							//Add the new plugin to our collection here
							this.colAvailablePlugins.Add(newPlugin);
							
							//cleanup a bit
							newPlugin = null;
						}	
						
						typeInterface = null; //Mr. Clean			
					}				
				}			
			}

			pluginAssembly = null; //more cleanup
		}

        /// <summary>
        /// Returns an array of PluginForms loaded from each plugin
        /// </summary>
        /// <param name="BackColor">Background Color of Tab</param>
        /// <param name="ForeColor">Foreground Color of Tab</param>
        public PluginForm[] GetPlugin(System.Drawing.Color BackColor, System.Drawing.Color ForeColor)
        {
            PluginForm tempTPage;
            PluginForm[] ret = new PluginForm[colAvailablePlugins.Count];
            int x = 0;
            foreach (Types.AvailablePlugin pluginOn in this.colAvailablePlugins)
            {
                tempTPage = new PluginForm();
                tempTPage.AutoScroll = true;
                tempTPage.Name = "pluginTab" + x.ToString();
                tempTPage.Text = pluginOn.Instance.TabText + " by " + pluginOn.Instance.Author;
                tempTPage.plugAuth = pluginOn.Instance.Author;
                tempTPage.plugDesc = pluginOn.Instance.Description;
                tempTPage.plugName = pluginOn.Instance.Name;
                tempTPage.plugVers = pluginOn.Instance.Version;
                tempTPage.plugText = pluginOn.Instance.TabText;
                tempTPage.Tag = x;

                Form1.PluginAllowColoring = Form1.DefaultPluginAllowColoring;
                pluginOn.Instance.Initialize();

                tempTPage.allowColoring = Form1.PluginAllowColoring;
                if (tempTPage.allowColoring)
                {
                    tempTPage.BackColor = BackColor;
                    tempTPage.ForeColor = ForeColor;
                }

                ret[x] = tempTPage;
                tempTPage = null;
                x++;
            }

            return ret;
        }

	}
	namespace Types
	{
		/// <summary>
		/// Collection for AvailablePlugin Type
		/// </summary>
		public class AvailablePlugins : System.Collections.CollectionBase
		{
			//A Simple Home-brew class to hold some info about our Available Plugins
			
			/// <summary>
			/// Add a Plugin to the collection of Available plugins
			/// </summary>
			/// <param name="pluginToAdd">The Plugin to Add</param>
			public void Add(Types.AvailablePlugin pluginToAdd)
			{
				this.List.Add(pluginToAdd); 
			}
		
			/// <summary>
			/// Remove a Plugin to the collection of Available plugins
			/// </summary>
			/// <param name="pluginToRemove">The Plugin to Remove</param>
			public void Remove(Types.AvailablePlugin pluginToRemove)
			{
				this.List.Remove(pluginToRemove);
			}

            /// <summary>
            /// Finds a plugin in the available Plugins
            /// </summary>
            /// <param name="pluginNameOrPath">The name or File path of the plugin to find</param>
            /// <returns>Available Plugin, or null if the plugin is not found</returns>
            public Types.AvailablePlugin Find(string pluginNameOrPath)
			{
				Types.AvailablePlugin toReturn = null;
			
				//Loop through all the plugins
				foreach (Types.AvailablePlugin pluginOn in this.List)
				{
					//Find the one with the matching name or filename
                    if ((pluginOn.Instance.Name.Equals(pluginNameOrPath))
                        || pluginOn.AssemblyPath.Equals(pluginNameOrPath))
                    {
                        toReturn = pluginOn;
                        break;
                    }
				}
				return toReturn;
			}

            /// <summary>
            /// Gets the plugin from the List at index
            /// </summary>
            /// <param name="index">The index of the plugin</param>
            /// <returns>Available Plugin</returns>
            public Types.AvailablePlugin GetIndex(int index)
            {
                if (index < List.Count)
                    return (Types.AvailablePlugin)this.List[index];
                return null;
            }
		}
		
		/// <summary>
		/// Data Class for Available Plugin.  Holds and instance of the loaded Plugin, as well as the Plugin's Assembly Path
		/// </summary>
		public class AvailablePlugin
		{
			//This is the actual AvailablePlugin object.. 
			//Holds an instance of the plugin to access
			//ALso holds assembly path... not really necessary
			private IPlugin myInstance = null;
			private string myAssemblyPath = "";
			
			public IPlugin Instance
			{
				get {return myInstance;}
				set	{myInstance = value;}
			}
			public string AssemblyPath
			{
				get {return myAssemblyPath;}
				set {myAssemblyPath = value;}
			}
		}

        /// <summary>
        /// Collection for AvailablePlugin Type
        /// </summary>
        public class AvailableAPIs : System.Collections.CollectionBase
        {
            //A Simple Home-brew class to hold some info about our Available Plugins

            /// <summary>
            /// Add a Plugin to the collection of Available plugins
            /// </summary>
            /// <param name="apiToAdd">The Plugin to Add</param>
            public void Add(Types.AvailableAPI apiToAdd)
            {
                this.List.Add(apiToAdd);
            }

            /// <summary>
            /// Remove a Plugin to the collection of Available plugins
            /// </summary>
            /// <param name="apiToRemove">The Plugin to Remove</param>
            public void Remove(Types.AvailableAPI apiToRemove)
            {
                this.List.Remove(apiToRemove);
            }

            /// <summary>
            /// Finds a plugin in the available Plugins
            /// </summary>
            /// <param name="apiNameOrPath">The name or File path of the plugin to find</param>
            /// <returns>Available Plugin, or null if the plugin is not found</returns>
            public Types.AvailableAPI Find(string apiNameOrPath)
            {
                Types.AvailableAPI toReturn = null;

                //Loop through all the plugins
                foreach (Types.AvailableAPI apiOn in this.List)
                {
                    //Find the one with the matching name or filename
                    if ((apiOn.Instance.Name.Equals(apiNameOrPath))
                        || apiOn.AssemblyPath.Equals(apiNameOrPath))
                    {
                        toReturn = apiOn;
                        break;
                    }
                }
                return toReturn;
            }

            /// <summary>
            /// Gets the plugin from the List at index
            /// </summary>
            /// <param name="index">The index of the plugin</param>
            /// <returns>Available Plugin</returns>
            public Types.AvailableAPI GetIndex(int index)
            {
                if (index < List.Count)
                    return (Types.AvailableAPI)this.List[index];
                return null;
            }

            /// <summary>
            /// Gets the plugin from the List at index
            /// </summary>
            /// <param name="index">The index of the plugin</param>
            /// <returns>Available Plugin</returns>
            public int GetIndex(Types.AvailableAPI api)
            {
                for (int x = 0; x < this.List.Count; x++)
                    if ((this.List  as Types.AvailableAPI) == api)
                        return x;
                return -1;
            }

            public int GetIndex(string name, string version)
            {
                string sss = name + " (" + version + ")";
                for (int x = 0; x < this.List.Count; x++)
                {
                    string ss = (this.List[x] as Types.AvailableAPI).Instance.Name + " (" + (this.List[x] as Types.AvailableAPI).Instance.Version + ")";
                    if (ss == sss)
                        return x;
                }
                return -1;
            }
        }

        /// <summary>
        /// Data Class for Available API.  Holds and instance of the loaded Plugin, as well as the Plugin's Assembly Path
        /// </summary>
        public class AvailableAPI
        {
            //This is the actual AvailableAPI object.. 
            //Holds an instance of the api to access
            private IAPI myInstance = null;
            private string myAssemblyPath = "";

            public IAPI Instance
            {
                get { return myInstance; }
                set { myInstance = value; }
            }
            public string AssemblyPath
            {
                get { return myAssemblyPath; }
                set { myAssemblyPath = value; }
            }
        }
	}	
}

