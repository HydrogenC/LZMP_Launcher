using LauncherCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LauncherUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] clientOnlyMods =
            {
                "entityculling-forge-mc1.16.5-1.5.1",
                "OptiFine_1.16.5_HD_U_G8"
            };

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

            Console.WriteLine("1. Clean up resources");
            Console.WriteLine("2. Open curseforge pages of the mods");
            Console.WriteLine("3. Deploy server");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine();
                    Console.WriteLine("Cleaning up...");
                    Core.CleanUp();
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine('\n');
                    var links = Scanner.ScanCurseforgeLinks(SharedData.WorkingPath + "\\BasicSettings.xml");
                    int index = 1;
                    foreach (var i in links)
                    {
                        Console.WriteLine("{0}. {1}", index, i.Item1);
                        index++;
                    }
                    Console.WriteLine("Which link to start from? ");
                    int.TryParse(Console.ReadLine(), out index);
                    if (index > 0)
                    {
                        index--;
                    }
                    for (int i = index; i < links.Length; i++)
                    {
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.WriteLine("Current file: {0}", links[i].Item1);
                        if (i != links.Length - 1)
                        {
                            Console.WriteLine("Pending file: {0}", links[i + 1].Item1);
                        }
                        Process.Start("explorer.exe", $"https://www.curseforge.com/minecraft/mc-mods/{links[i].Item2}");
                    }
                    break;
                case ConsoleKey.D3:
                    string serverModPath = SharedData.WorkingPath + "\\Server\\server\\mods";
                    if (Directory.Exists(serverModPath))
                    {
                        Directory.Delete(serverModPath, true);
                    }
                    Core.CopyDirectory(SharedData.ModPath, serverModPath);

                    foreach (var i in Directory.EnumerateFiles(serverModPath))
                    {
                        if (Array.Find(clientOnlyMods, (e) => e == Path.GetFileNameWithoutExtension(i)) != null)
                        {
                            File.Delete(i);
                        }
                    }
                    break;
                default:
                    break;
            }

            Console.WriteLine("Complete! ");
            Console.ReadLine();
        }
    }
}
