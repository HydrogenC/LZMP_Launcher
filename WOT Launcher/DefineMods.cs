using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOT_Launcher
{
    public class DefineMods
    {
        static List<Mod> mods = new List<Mod>();
        public static List<Mod> ReturnMods()
        {
            String[] fileTmp;

            AddMod("AgriCraft", "AgriCraft-1.7.10-1.5.0", 0);

            AddMod("Applied Energetics", "appliedenergistics2-rv3-beta-6", 0);

            fileTmp = new String[2]
            {
                "buildcraft-7.1.23",
                "buildcraft-compat-7.1.7"
            };
            AddMod("Buildcraft", fileTmp, 0);

            fileTmp = new String[2]
            {
                "SonarCore-1.7.10-1.1.3",
                "Calculator-1.7.10-1.9.11"
            };
            AddMod("Calculator", fileTmp, 0);



            fileTmp = new String[2]
            {
                "Draconic-Evolution-1.7.10-1.0.2h",
                "BrandonsCore-1.0.0.12"
            };
            AddMod("Draconic Evolution", fileTmp, 0);

            fileTmp = new String[2]
            {
                "EnderCore-1.7.10-0.2.0.39_beta",
                "EnderIO-1.7.10-2.3.0.429_beta"
            };
            AddMod("Ender IO", fileTmp, 0);

            AddMod("Ex Nihilo", "Ex-Nihilo-1.38-53", 0);

            AddMod("Fission Warefare", "FissionWarfare-1.1.0", 0);

            fileTmp = new String[3]
            {
                "GalacticraftCore-1.7-3.0.12.503",
                "Galacticraft-Planets-1.7-3.0.12.503",
                "MicdoodleCore-1.7-3.0.12.503"
            };
            AddMod("Galacticraft", fileTmp, 0);

            AddMod("Growable Ores", "B0bGary's Growable Ores-2.5.4 for 1.7.10", 0);

            AddMod("Immersive Engineering", "ImmersiveEngineering-0.7.7", 0);

            fileTmp = new String[2]
            {
                "industrialcraft-2-2.2.828-experimental",
                "PowerUtils-1.7.10-1.0.4"
            };
            AddMod("Industrialcraft", fileTmp, 0);

            AddMod("Nuclearcraft", "NuclearCraft-1.9f--1.7.10", 0);

            AddMod("ProjectE", "ProjectE-1.7.10-PE1.10.1", 0);

            fileTmp = new String[7]
            {
                "ProjectRed-1.7.10-4.7.0pre12.95-Base",
                "ProjectRed-1.7.10-4.7.0pre12.95-Compat",
                "ProjectRed-1.7.10-4.7.0pre12.95-Fabrication",
                "ProjectRed-1.7.10-4.7.0pre12.95-Integration",
                "ProjectRed-1.7.10-4.7.0pre12.95-Lighting",
                "ProjectRed-1.7.10-4.7.0pre12.95-Mechanical",
                "ProjectRed-1.7.10-4.7.0pre12.95-World"
            };
            AddMod("ProjectRed", fileTmp, 0);

            AddMod("Railcraft", "Railcraft_1.7.10-9.12.2.0", 0);

            AddMod("Refined Relocation", "RefinedRelocation-mc1.7.10-1.1.34", 0);

            AddMod("Remote IO", "RemoteIO-1.7.10-2.4.0-universal", 0);

            AddMod("Router Reborn", "RouterReborn-1.7.10-1.2.0.43-universal", 0);

            AddMod("Traincraft", "Traincraft-4.3.5_014", 0);

            AddMod("Chisel", "Chisel-2.9.5.11", 1);

            AddMod("Custom NPCs", "CustomNPCs_1.7.10d(21feb16)", 1);

            AddMod("Defense Tech", "DefenseTech-1.7.10-1.0.1.46", 1);

            AddMod("Light Bridges", "Light Bridges and Doors V 2.0", 1);

            AddMod("Modern Warfare", "mw-1.11.6_mc1.7.10", 1);

            AddMod("Open Modular Turrents", "OpenModularTurrets-1.7.10-2.2.11-238", 1);

            AddMod("Stefinus Guns", "Stefinus Guns-0.5.2", 1);

            fileTmp = new String[2]
            {
                "aether-1.7.10-1.6",
                "gilded-games-util-1.7.10-1.9"
            };
            AddMod("Aether", fileTmp, 2);

            AddMod("NeverMine", "[1.7.10][ZH]Nevermine-2.5(S2)", 2);

            AddMod("TheCampingMod", "TheCampingMod_2.1f", 2);

            AddMod("Twilight Forest", "twilightforest-1.7.10-2.3.7", 2);

            AddMod("Biome O Plenty", "BiomesOPlenty-1.7.10-2.1.0.1889-universal", 3);

            AddMod("Carpenter's Blocks", "Carpenter's Blocks v3.3.8.1 - MC 1.7.10", 3);

            AddMod("ShadersMod", "ShadersModCore-2.3.31-mc1.7.10", 3);

            fileTmp = new String[2]{
                "weather2-1.7.10-2.3.10",
                "coroutil-1.7.10-1.1.5"
            };
            AddMod("Weather 2", fileTmp, 3);

            //Additions
            fileTmp = new String[3]
            {
                "AdvancedSolarPanel-1.7.10-3.5.1",
                "ASP&GS Patcher",
                "GraviSuite-1.7.10-2.0.3"
            };
            AddMod("ASP & GS", fileTmp, mods[11]);

            AddMod("IC2 NC", "IC2NuclearControl-2.4.3a", mods[11]);

            fileTmp = new String[2]
            {
                "immibis-core-59.0.4",
                "adv-repulsion-systems-59.0.2"
            };
            AddMod("Adv Repultion Systems", fileTmp, mods[11]);

            AddMod("AE2 Stuff", "ae2stuff-rv3-0.5.1.9-mc1.7.10", mods[1]);

            fileTmp = new String[2]
            {
                "WirelessCraftingTerminal-1.7.10-rv3-1.8.7.4b",
                "p455w0rdslib-1.7.10-1.0.4"
            };
            AddMod("Wireless Crafting Terminal", fileTmp, mods[1]);

            AddMod("Iron Tanks", "irontank-1.7.10-1.1.15.65", mods[2]);

            AddMod("Logistics Pipes", "logisticspipes-0.9.3.132", mods[2]);

            AddMod("Ender IO Addons", "EnderIOAddons-1.7.10-2.3.0.427_beta-0.10.13.56_beta", mods[5]);

            AddMod("Ex Artris", "Ex-Astris-MC1.7.10-1.16-36", mods[6]);

            AddMod("ExtraPlanets", "ExtraPlanets-1.7.10-2.0.0", mods[8]);

            AddMod("Immersive Integration", "immersiveintegration-0.6.8", mods[10]);

            AddMod("Project Blue", "ProjectBlue-1.1.6-mc1.7.10", mods[14]);

            AddMod("Techguns", "Techguns.beta.1.2_alphatest4.1", mods[20]);

            return mods;
        }

        private static void AddMod(String name, String file, Int16 category)
        {
            List<String> list = new List<String>();
            list.Add(file);
            mods.Add(new Mod(name, list, category));
        }

        private static void AddMod(String name, String[] files, Int16 category)
        {
            List<String> list = new List<String>(files);
            mods.Add(new Mod(name, list, category));
        }

        private static void AddMod(String name, String file, Mod father)
        {
            List<String> list = new List<String>();
            list.Add(file);
            mods.Add(new Mod(name, list, father));
        }

        private static void AddMod(String name, String[] files, Mod father)
        {
            List<String> list = new List<String>(files);
            mods.Add(new Mod(name, list, father));
        }
    }
}
