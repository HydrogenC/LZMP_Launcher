using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZMP_Launcher
{
    /* This file is deprecated and its data will be move to the xml */
    
    /*
    public class DefineMods
    {
        static Dictionary<String, Mod> mods = new Dictionary<String, Mod>();
        public static Dictionary<String, Mod> ReturnMods()
        {
            AddMod("Immersive Engineering", "ie", "ImmersiveEngineering-%v", 0);
            {
                AddMod("Immersive Cables", "imc", "ImmersiveCables-1.12.2-%v", mods["ie"]);

                AddMod("Immersive Petroleum", "imp", "immersivepetroleum-1.12.2-%v", mods["ie"]);

                AddMod("Immersive Tech", "imt", "immersivetech-1.12-%v", mods["ie"]);

                AddMod("Industrial Wires", "iw", "IndustrialWires-%v", mods["ie"]);
            }

            AddMod("Industrialcraft", "ic", "industrialcraft-2-%v-ex112", 0);
            {
                AddMod("ASP", "asp", "Advanced+Solar+Panels-%v", mods["ic"]);

                AddMod("Compact Machines", "cpm", "compactmachines3-1.12.2-%v", mods["ic"]);

                AddMod("Gravi Suite", "gsi", "Gravitation+Suite-%v", mods["ic"]);
            }

            fileTmp = new String[] {
                "Forgelin-%v",
                "Tesla-1.12.2-%v",
                "tesla-core-lib-1.12.2-%v",
                "industrialforegoing-1.12.2-%v",
                "IntegrationForegoing-1.12.2-%v"
            };
            AddMod("Industrial Foregoing", "idf", fileTmp, 0);

            fileTmp = new String[]
            {
                "CyclopsCore-1.12.2-%v",
                "CommonCapabilities-1.12.2-%v",
                "IntegratedDynamics-1.12.2-%v",
                "IntegratedTunnels-1.12.2-%v",
                "IntegratedCrafting-1.12.2-%v",
                "IntegratedTerminals-1.12.2-%v"
            };
            AddMod("Integrated Dynamics", "itd", fileTmp, 0);

            AddMod("Matter Overdrive", "mo", "MatterOverdrive-1.12.2-%v-universal", 0);

            AddMod("Modular Powersuits", "mps", "ModularPowersuits-1.12.2-%v", 0);

            AddMod("Modular Routers", "mr", "modular-routers-1.12.2-%v", 0);

            fileTmp = new String[]
            {
                "Cucumber-1.12.2-%v",
                "MysticalAgriculture-1.12.2-%v",
                "MysticalAgradditions-1.12.2-%v"
            };
            AddMod("Mystical Agriculture", "ma", fileTmp, 0);

            AddMod("Nuclearcraft", "nc", "NuclearCraft-%v-1.12.2", 0);

            fileTmp = new String[] {
                "ProjectE-1.12.2-%v",
                "ViewEMC-1.12.2-%v"
            };
            AddMod("ProjectE", "pe", fileTmp, 0);

            fileTmp = new String[]
            {
                "ProjectRed-1.12.2-%v-Base",
                "ProjectRed-1.12.2-%v-compat",
                "ProjectRed-1.12.2-%v-fabrication",
                "ProjectRed-1.12.2-%v-integration",
                "ProjectRed-1.12.2-%v-lighting",
                "ProjectRed-1.12.2-%v-mechanical",
                "ProjectRed-1.12.2-%v-world"
            };
            AddMod("ProjectRed", "prd", fileTmp, 0);

            AddMod("Railcraft", "rc", "railcraft-%v", 0);

            AddMod("Refined Relocation", "rel", "RefinedRelocation_1.12.2-%v", 0);

            fileTmp = new String[] {
                "refinedstorage-%v",
                "refinedstorageaddons-%v"
            };
            AddMod("Refined Storage", "rs", fileTmp, 0);
            {
                AddMod("Refined Exchange", "ref", "refinedexchange-%v", mods["rs"]);

                AddMod("Storage Tech", "str", "StorageTech+%v", mods["rs"]);
            }

            AddMod("Reborn Core", "rco", "RebornCore-1.12.2-%v-universal", 0);
            {
                AddMod("Steve's Carts", "car", "StevesCarts-1.12.2-%v", mods["rco"]);

                AddMod("TechReborn", "tr", "TechReborn-1.12.2-%v-universal", mods["rco"]);

                AddMod("Reborn Storage (Req RefinedStorage)", "reb", "RebornStorage-1.12.2-%v", mods["rco"]);
            }

            AddMod("RFTools", "rft", "rftools-1.12-%v", 0);
            {
                AddMod("RFTools Control", "rfc", "rftoolsctrl-1.12-%v", mods["rft"]);

                AddMod("RFTools Dimension", "rfd", "rftoolsdim-1.12-%v", mods["rft"]);

                AddMod("RFTools Power", "rfp", "rftoolspower-1.12-%v", mods["rft"]);
            }

            AddMod("Sonar Core", "sc", "sonarcore-1.12.2-%v", 0);
            {
                AddMod("Calculator", "cal", "calculator-1.12.2-%v", mods["sc"]);

                AddMod("Flux Networks", "net", "fluxnetworks-1.12.2-%v", mods["sc"]);

                AddMod("Practical Logistics", "plo", "practicallogistics2-1.12.2-%v", mods["sc"]);
            }

            AddMod("Wireless Utilities", "wlu", "wirelessutils-1.12.2-%v-universal", 0);

            AddMod("XNet", "xn", "xnet-1.12-%v", 0);

            AddMod("Zetta Industries", "zi", "zettaindustries-%v", 0);

            //1
            AddMod("Custom NPCs", "npc", "CustomNPCs_1.12.2(%v)", 1);

            AddMod("ICBM", "icb", "ICBM-classic-1.12.2-%v", 1);

            AddMod("Modern Warfare", "mw", "mw-%v_mc1.12.2", 1);

            fileTmp = new String[] {
                "omlib-1.12.2-%v",
                "openmodularturrets-1.12.2-%v"
            };
            AddMod("Open Modular Turrents", "omt", fileTmp, 1);

            AddMod("Techguns", "tg", "techguns-1.12.2-%v", 1);

            //2
            AddMod("Biomes O Plenty", "bop", "BiomesOPlenty-1.12.2-%v-universal", 2);

            AddMod("Carpenter's Blocks", "cb", "Carpenter's Blocks %v - MC 1.12.2", 2);

            fileTmp = new String[]
            {
                "ItemFilters-%v",
                "FTBQuests-%v"
            };
            AddMod("FTB Quests", "fq", fileTmp, 2);

            fileTmp = new String[] {
                "malisiscore-1.12.2-%v",
                "malisisblocks-1.12.2-%v",
                "malisisdoors-1.12.2-%v"
            };
            AddMod("Malisis Doors & Blocks", "mdb", fileTmp, 2);

            AddMod("MrCrayfish's Furniture", "fur", "furniture-%v-1.12.2", 2);

            fileTmp = new String[]
            {
                "OpenModsLib-1.12.2-%v",
                "OpenBlocks-1.12.2-%v"
            };
            AddMod("OpenBlocks", "ob", fileTmp, 2);

            fileTmp = new String[] {
                "SilentLib-1.12.2-%v",
                "ScalingHealth-1.12.2-%v"
            };
            AddMod("Scaling Health", "sca", fileTmp, 2);

            AddMod("Tiny Mob Farm", "tmf", "TinyMobFarm-1.12.2-%v", 2);

            return mods;
        }
    }
    */
}
