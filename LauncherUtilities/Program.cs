using LauncherCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LauncherUtilities
{
    class Program
    {
        const string API_KEY = "$2a$10$1HxEu67mkjMNTvOMzhL4pOvCXRpLA.S.EUMR2gkBt9LSNd/p6eyMS";
        public static HttpClient Client
        {
            get; set;
        } = new HttpClient();

        static string GetOptionalModPath(string filename)
        {
            return Path.Combine(SharedData.ResourcePath, $"{filename}.jar");
        }

        static string RequestFiles(string modName)
        {
            var res = Client.GetAsync($"https://api.curseforge.com/v1/mods/{modName}/files").Result;
            return res.Content.ReadAsStringAsync().Result;
        }

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

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("x-api-key", API_KEY);

            Scanner.ScanConfig(SharedData.WorkingPath + "\\BasicSettings.xml");
            Console.WriteLine("Modpack version: " + SharedData.Version);
            Core.CheckInstallation();
            Core.CheckAvailability();

            Console.WriteLine("1. Clean up resources");
            Console.WriteLine("2. Check update of optional mods");
            Console.WriteLine("3. Check update of builtin mods");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine();
                    Console.WriteLine("Cleaning up...");
                    Core.CleanUp();
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine('\n');
                    var entries = Scanner.ScanCurseforgeLinks(SharedData.WorkingPath + "\\BasicSettings.xml");
                    int index = 1;
                    foreach (var i in entries)
                    {
                        Console.WriteLine("{0}. {1}", index, i.Item1);
                        try
                        {
                            var data = new ModData(GetOptionalModPath(i.Item1));
                            if (data.Version != null)
                            {
                                Console.WriteLine("Version: {0}", data.Version);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        index++;
                    }

                    Console.WriteLine(RequestFiles("293426"));
                    /*
                    if (index > 0)
                    {
                        index--;
                    }
                    for (int i = index; i < entries.Length; i++)
                    {
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.WriteLine("Current file: {0}", entries[i].Item1);
                        if (i != entries.Length - 1)
                        {
                            Console.WriteLine("Pending file: {0}", entries[i + 1].Item1);
                        }
                        Process.Start("explorer.exe", $"https://www.curseforge.com/minecraft/mc-mods/{entries[i].Item2}");
                    }
                    */
                    break;
                default:
                    break;
            }

            Console.WriteLine("Complete! ");
            Console.ReadLine();
        }
    }
}