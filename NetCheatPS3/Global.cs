/*
 * By "Redth"
 * http://www.codeproject.com/Articles/6334/Plug-ins-in-C
 */

using System;

namespace NetCheatPS3
{
	/// <summary>
	/// Holds A static instance of global program shtuff
	/// </summary>
	public class Global
	{
		public Global(){} //Constructor
		
		//What have we done here?	
		public static NetCheatPS3.PluginServices Plugins = new PluginServices();

        public static NetCheatPS3.APIServices APIs = new APIServices();

		/*
			instead of on the frmMain.cs having to declare a PluginService object
			what i've done here is created one in the Global Class.. i've also made
			it static, so we don't have to worry about the object.. It's always gonna
			be there for us and the same object will always be accessed by everything
			else in the program...
			
			So now, everywhere else in this project i can type:
			
				Global.Plugins .... > 
				
			and it will bring up the Plugins object created above.. peachy, eh?
		
		*/
	}
}
