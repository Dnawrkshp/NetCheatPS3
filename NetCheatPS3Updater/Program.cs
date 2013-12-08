using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NetCheatPS3Updater
{
    class Program
    {
        static bool ProcessExists(int id)
        {
            return Process.GetProcesses().Any(x => x.Id == id);
        }

        static void Main(string[] args)
        {
            if (args.Length < 4)
                return;

            int PID = int.Parse(args[0]);
            while (ProcessExists(PID)) { }

            Console.WriteLine("Process " + args[0] + " has exited!");

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(args[1], "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(args[1], args[2]));

            //Copy all the files
            foreach (string newPath in Directory.GetFiles(args[1], "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(args[1], args[2]), true);

            Directory.Delete(args[1], true);
            Process.Start(args[3]);

            //System.Threading.Thread.Sleep(10000);
        }
    }
}
