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
            XmlHelper.ReadDefinitions(MinecraftInstance.WorkingPath + "\\BasicSettings.xml");
            Core.CheckInstallation();
            Core.CheckAvailability();

        cls: Console.Clear();
            Console.WriteLine("1. Clean up unused resources");
            Console.WriteLine("2. Initialize modpack to uninstalled state");
            Console.Write("Enter number: ");
            ushort num = 0;

            if (!ushort.TryParse(Console.ReadLine(), out num))
            {
                goto cls;
            }

            switch (num)
            {
                case 1:
                    Console.WriteLine("Processing...");

                    Core.CleanUp();

                    Console.WriteLine("Complete! ");
                    Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Processing...");

                    Parallel.ForEach(SharedData.Mods, (KeyValuePair<string, Mod> pair) =>
                    {
                        if (pair.Value.Installed[SharedData.Client])
                        {
                            pair.Value.Uninstall(SharedData.Client);
                        }
                    });

                    Core.CopyDirectory(SharedData.Client.ModPath, MinecraftInstance.WorkingPath + "\\Mods");

                    if (Directory.Exists(SharedData.Client.ModPath))
                    {
                        Directory.Delete(SharedData.Client.ModPath, true);
                    }

                    if (Directory.Exists(SharedData.Server.ModPath))
                    {
                        Directory.Delete(SharedData.Server.ModPath, true);
                    }

                    Console.WriteLine("Complete! ");
                    Console.ReadLine();
                    break;
                default:
                    break;
            }
            goto cls;
        }
    }
}
