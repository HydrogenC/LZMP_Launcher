using LauncherCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
        cls: Console.Clear();
            Console.WriteLine("1. Clean up unused resources");
            Console.WriteLine("2. Initialize modpack to uninstalled state");
            Console.Write("Enter number: ");
            ushort num = ushort.Parse(Console.ReadLine());
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
