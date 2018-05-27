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

            AddMod("Applied Energetics", "appliedenergistics2-rv5-stable-11", 0);

            fileTmp = new String[2]
            {
                "buildcraft-7.99.17",
                "buildcraft-compat-7.99.14"
            };
            AddMod("Buildcraft", fileTmp, 0);

            fileTmp = new String[3]
            {
                "sonarcore-1.12.2-5.0.9",
                "calculator-1.12.2-5.0.4",
                "fluxnetworks-1.12.2-3.0.8"
            };
            AddMod("Calculator & Flux Networks", fileTmp, 0);

            fileTmp = new String[2]
            {
                "Draconic-Evolution-1.12-2.3.11.290-universal",
                "BrandonsCore-1.12-2.4.2.157-universal"
            };
            AddMod("Draconic Evolution", fileTmp, 0);

            fileTmp = new String[2]
            {
                "EnderIO-1.12.2-5.0.24",
                "EnderCore-1.12.2-0.5.22"
            };
            AddMod("Ender IO", fileTmp, 0);

            AddMod("Ex Nihilo", "exnihilocreatio-1.12-0.2.2", 0);

            fileTmp = new String[3]
            {
                "GalacticraftCore-1.12.2-4.0.1.177",
                "Galacticraft-Planets-1.12.2-4.0.1.177",
                "MicdoodleCore-1.12.2-4.0.1.177"
            };
            AddMod("Galacticraft", fileTmp, 0);

            AddMod("Immersive Engineering", "ImmersiveEngineering-0.12-82", 0);

            AddMod("Industrialcraft", "industrialcraft-2-2.8.76-ex112", 0);

            AddMod("Nuclearcraft", "NuclearCraft-2.10m--1.12.2", 0);

            AddMod("ProjectE", "ProjectE-1.12-PE1.3.0", 0);

            AddMod("Refined Relocation", "RefinedRelocation_1.12.2-4.2.20", 0);

            AddMod("Reborn Core", "RebornCore-1.12.2-3.8.3.279-universal", 0);

            AddMod("Modular Routers", "modular-routers-1.12.2-3.1.4", 0);

            AddMod("Custom NPCs", "CustomNPCs_1.12.2(20jan18)", 1);

            AddMod("ICBM", "ICBM-classic-1.12.2-3.0.0b29", 1);

            AddMod("Modern Warfare", "mw-1.11.7.1_mc1.12.2", 1);

            fileTmp = new String[2]
            {
                "omlib-1.12.2-3.0.0-143",
                "openmodularturrets-1.12.2-3.0.0-248"
            };
            AddMod("Open Modular Turrents", fileTmp, 1);

            AddMod("Techguns", "techguns-1.12.2-2.0.1.1", 1);

            AddMod("Biome O Plenty", "BiomesOPlenty-1.12.2-7.0.1.2384-universal", 2);

            AddMod("Carpenter's Blocks", "Carpenter's Blocks v3.4.004 POC - MC 1.12.2", 2);

            fileTmp = new String[3]{
                "malisiscore-1.12.2-6.4.0",
                "malisisdoors-1.12.2-7.3.0",
                "malisisblocks-1.12.2-6.1.0"
            };
            AddMod("Malisis Doors & Blocks",fileTmp,2);

            AddMod("MrCrayfish's Furniture", "cfm-4.2.0-mc1.12.2", 2);

            fileTmp = new String[2]{
                "OpenModsLib-1.12.2-0.11.5",
                "OpenBlocks-1.12.2-1.7.6"
            };
            AddMod("OpenBlocks", fileTmp, 2);

            fileTmp = new String[2]{
                "ScalingHealth-1.12-1.3.14-96",
                "SilentLib-1.12-2.2.18-100"
            };
            AddMod("Scaling Health", fileTmp, 2);

            AddMod("Tick Dynamic", "TickDynamic-1.12.2-1.0.1", 2);

            //Additions
            AddMod("ASP", "Advanced+Solar+Panels-4.2.0", mods[8]);

            AddMod("Compact Machines", "compactmachines3-1.12.2-3.0.11-b207", mods[8]);

            AddMod("AE2 Stuff", "ae2stuff-0.7.0.4-mc1.12.2", mods[0]);

            fileTmp = new String[2]
            {
                "WirelessCraftingTerminal-1.12.2-3.9.64",
                "p455w0rdslib-1.12-2.0.29"
            };
            AddMod("Wireless Crafting Terminal", fileTmp, mods[0]);

            fileTmp = new String[2]
            {
                "ExtraPlanets-1.12.2-0.3.8",
                "MJRLegendsLib-1.12.2-1.1.4"
            };
            AddMod("ExtraPlanets", fileTmp, mods[6]);

            AddMod("Immersive Petroleum", "immersivepetroleum-1.12.2-1.1.9", mods[7]);

            AddMod("Immersive Tech", "immersivetech-1.12-1.3.8", mods[7]);

            AddMod("Steve's Carts", "StevesCarts-1.12.2-2.4.19.95", mods[12]);

            AddMod("TechReborn", "TechReborn-1.12.2-2.15.9.663-universal", mods[12]);

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
