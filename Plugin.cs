using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Nautilus.Utility;
using System.Collections;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;
using LawAbidingTroller.LiteralSeaglideUpgrades;
using Nautilus.Assets;
using Nautilus.Handlers;
using UnityEngine.Serialization;

namespace LawAbidingTroller.SeaglideModConcept //Will credit any coders contributing to the mod
{
    /*
     * Todo List: Complete for now
     */
    [BepInPlugin("com.lawabidingtroller.literalseaglideupgrades", "Literal Seaglide Upgrades", "0.2.9")]
    [BepInDependency("com.snmodding.nautilus")]
    [BepInDependency("com.lawabidingmodder.upgradeslib")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public bool hasalreadyran;

        public static ModOptions ModOptions;
        
        public static TechCategory LiteralSeaglideUpgrades = EnumHandler.AddEntry<TechCategory>("LiteralSeaglideUpgrades").WithPdaInfo("Literal Seaglide Upgrades").RegisterToTechGroup(UpgradesLIB.Plugin.toolupgrademodules);
        
        private void Awake()
        {
            
            // set project-scoped logger instance
            Logger = base.Logger;
            Logger.LogInfo("Awake method is running. Dependencies exist. Completing plugin load...");
            
            // Initialize mod options ASAP
            ModOptions = OptionsPanelHandler.RegisterModOptions<ModOptions>();
            // Create the Seaglide Upgrade Storage for the Seaglides
            StartCoroutine(UpgradesLIB.Plugin.CreateUpgradesContainer(TechType.Seaglide, "SeaglideUpgradeStorage", "SeaglideUpgradeStorageChild", 2, 2));
            
            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, "Literal Seaglide Upgrades");
            Nautilus.Handlers.CraftTreeHandler.AddTabNode(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType, "SeaglideTab", "Seaglide",
                SpriteManager.Get(TechType.Seaglide), "Tools");
            Nautilus.Handlers.CraftTreeHandler.AddTabNode(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType, "ExtraUpgrades",
                    "Speed Upgrades (Extras)",
                    SpriteManager.Get(TechType.Seaglide), "Tools", "SeaglideTab");
            
            InitializePrefabs();
            Logger.LogInfo("Plugin fully loaded successfully!");
        }
        
        public static float[] Speedmultiplier = { 8, 12, 17, 23, 30, 38, 47, 57, 68, 80 };
        public static CustomPrefab[] Speedprefab = new CustomPrefab[10];
        public static PrefabInfo[] Prefabinfo = new PrefabInfo[10];
        public static UpgradeData[] Upgradedata = new UpgradeData[10];
        public static int Index;
        public static float Changespeedmultiplier = 4;
        public static bool InitializedPrefabs = false;
        private void InitializePrefabs()
        {
            SeaglideSpeedModulePrefab.Register();
            SeaglideSpeedModuleMk2.Register();
            SeaglideSpeedModuleMk3.Register();
            SeaglideEfficiencyModuleMk1.Register();
            SeaglideEfficiencyModuleMk2.Register();
            SeaglideEfficiencyModuleMk3.Register();
                for (Index = 0; Index < 10; Index++)
                {
                    Speedmultiplier[Index] += Changespeedmultiplier;
                    SeaglideSpeedModuleMk4.Register();
                    Upgradedata[Index] = new UpgradeData(Speedmultiplier[Index]);
                    ModOptions.upgradeValues.Add(Prefabinfo[Index].TechType, Upgradedata[Index]);
                    Logger.LogInfo(Index.ToString());
                    if (Index == 8)
                    {
                        Changespeedmultiplier = 20;
                    }
                    else
                    {
                        Changespeedmultiplier++;
                    }
                }
            Logger.LogInfo("All (or most) Prefabs successfully initialized!");
            InitializedPrefabs = true;
        }
    }
}
