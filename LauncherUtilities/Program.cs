using LauncherCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LauncherUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            SharedData.DisplayMessage = (string content, string caption, MessageType type) =>
            {
                Console.WriteLine("[{0}] {1}", caption, content);
                if (type == MessageType.Error)
                {
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
                return MessageResult.OK;
            };
            Scanner.ScanConfig(SharedData.WorkingPath + "\\BasicSettings.xml");
            Console.WriteLine("Modpack version: " + SharedData.Version);
            Core.CheckInstallation();
            Core.CheckAvailability();

            Console.WriteLine("Press any key to clean up resources...");
            Console.ReadKey();
            Console.WriteLine("Cleaning up...");
            Core.CleanUp();
            Console.WriteLine("Complete! ");
            Console.ReadLine();
        }
    }
}
