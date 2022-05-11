using System;
using PluginInterface;

namespace NCMemBrowser
{
	/// <summary>
	/// Plugin
	/// </summary>
	public class Plugin : IPlugin  // <-- See how we inherited the IPlugin interface?
	{
		public Plugin()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		//Declarations of all our internal plugin variables
		string myName = "NetCheat Memory Browser";
		string myDescription = "A NetCheat plugin to browse and edit the memory";
		string myAuthor = "Dnawrkshp";
		string myVersion = "1.0.0";
		string myTabText = "Memory Browser";
        static IPluginHost myHost = null;
        //User Control to print
        System.Windows.Forms.UserControl myMainInterface = new ctlMain();
        System.Windows.Forms.UserControl myMainIcon;

        /// <summary>
        /// Description of the Plugin's purpose
        /// </summary>
        public string Description
        {
            get { return myDescription; }
        }

        /// <summary>
        /// Author of the plugin
        /// </summary>
        public string Author
        {
            get { return myAuthor; }

        }

        /// <summary>
        /// Text of the tab the plugin belongs to
        /// </summary>
        public string TabText
        {
            get { return myTabText; }
        }

        /// <summary>
        /// Host of the plugin.
        /// </summary>
        public IPluginHost Host
        {
            get { return myHost; }
            set
            {
                myHost = value;
                ctlMain.NCInterface = myHost;
            }
        }

        public string Name
        {
            get { return myName; }
        }

        public System.Windows.Forms.UserControl MainInterface
        {
            get { return myMainInterface; }
        }

        public System.Windows.Forms.UserControl MainIcon
        {
            get { return myMainIcon; }
        }

        public string Version
        {
            get { return myVersion; }
        }

        public void Initialize()
        {
            //This is the first Function called by the host...
            //Put anything needed to start with here first
        }

        public void Dispose()
        {
            //Put any cleanup code in here for when the program is stopped
        }
	}
}
