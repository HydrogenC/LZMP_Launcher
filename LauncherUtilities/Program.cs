using LauncherCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Forms;

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

        static string RequestFiles(string modName, string versionTypeId, int pageSize = 4)
        {
            var res = Client.GetAsync($"https://api.curseforge.com/v1/mods/{modName}/files?pageSize={pageSize}&gameVersionTypeId={versionTypeId}").Result;
            return res.Content.ReadAsStringAsync().Result;
        }

        static string GetVersionTypeId()
        {
            var res = Client.GetAsync($"https://api.curseforge.com/v1/minecraft/version/{SharedData.MinecraftVersion}").Result;
            var jsonString = res.Content.ReadAsStringAsync().Result;

            var jsonData = JsonNode.Parse(jsonString)["data"];
            return jsonData["gameVersionTypeId"].ToString();
        }

        static string TranslateType(int type)
        {
            switch (type)
            {
                case 1:
                    return "Release";
                case 2:
                    return "Beta";
                default:
                    return "Unknown";
            }
        }

        static void Main(string[] args)
        {
            // Register logger
            SharedData.LogMessage = (string content, string caption, MessageType type) =>
            {
                Console.WriteLine("[{0}] {1}", caption, content);
                if (type == MessageType.Error)
                {
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
                return MessageResult.OK;
            };

            // Set HttpClient headers
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("x-api-key", API_KEY);

            Scanner.ScanConfig(SharedData.WorkingPath + "\\BasicSettings.xml");
            Console.WriteLine("Minecraft version: " + SharedData.MinecraftVersion);
            Console.WriteLine("Modpack version: " + SharedData.Version);
            Core.CheckInstallation();
            Core.CheckAvailability();

            var entries = Scanner.GetFileEntries(Path.Combine(SharedData.WorkingPath, "BasicSettings.xml"));

            Console.WriteLine("1. Clean up resources");
            Console.WriteLine("2. Check update of mods");
            Console.WriteLine("3. Print version of mods");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine();
                    Console.WriteLine("Cleaning up...");
                    Core.CleanUp();
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine();
                    string versionTypeId = GetVersionTypeId();

                    foreach (var i in entries)
                    {
                        string jsonString = RequestFiles(i.CurseforgeId, versionTypeId);
                        var dataNode = (JsonArray)JsonNode.Parse(jsonString)["data"];
                        foreach (var item in dataNode)
                        {
                            var releaseType = (int)item["releaseType"];
                            if (releaseType <= 2)
                            {
                                var remoteFileName = Path.GetFileNameWithoutExtension((string)item["fileName"]);
                                if (remoteFileName == i.FileName)
                                {
                                    Console.WriteLine("File {0} is up to date! ", i.FileName);
                                }
                                else
                                {
                                    Console.WriteLine("File {0} has a new version: ", i.FileName);
                                    Console.WriteLine("New file name: {0}, Date: {1}, Type: {2}", remoteFileName, (string)item["fileDate"], TranslateType(releaseType));
                                    Console.WriteLine("Link: {0}", (string)item["downloadUrl"]);
                                    Console.WriteLine("Press any key to continue...");
                                    Console.ReadKey();
                                }

                                break;
                            }
                        }

                        Console.WriteLine();
                    }
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine();

                    int index = 0;
                    foreach (var i in entries)
                    {
                        try
                        {
                            var data = new ModData(GetOptionalModPath(i.FileName));
                            if (data.Version != null)
                            {
                                Console.WriteLine("{0}. Mod: {1}, Version: {2}", index, data.DisplayName, data.Version);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0}. Filename: {1}, Exception: {2}", index, i.FileName, e.Message);
                        }

                        index++;
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