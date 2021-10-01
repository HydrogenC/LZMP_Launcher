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
                default:
                    break;
            }

            Console.WriteLine("Complete! ");
            Console.ReadLine();
        }
    }
}
