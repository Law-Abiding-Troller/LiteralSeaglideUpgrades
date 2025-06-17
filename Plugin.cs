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
     * Todo List:
     * Remove fields currentspeed and _currentefficiency
     * Remove methods IncreaseSeaglideSpeed, DecreaseSeaglideSpeed, IncreaseSeaglideEfficiency, DecreaseSeaglideEfficiency, UpdateSeaglideSpeed, and UpdateSeaglideEfficiency.
     * Move method UpdateExtraUpgrades to ModOptions.cs as a OnChange event
     * Remove unnecessary method in Plugin.cs (move to other classes)
     */
    [BepInPlugin("com.lawabidingtroller.literalseaglideupgrades", "Literal Seaglide Upgrades", "0.2.5")]
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
            
            // Initialize mod options ASAP
            ModOptions = OptionsPanelHandler.RegisterModOptions<ModOptions>();
            // Create the Seaglide Upgrade Storage for the Seaglides
            StartCoroutine(UpgradesLIB.Plugin.CreateUpgradesContainer(TechType.Seaglide, "SeaglideUpgradeStorage", "SeaglideUpgradeStorageChild", 2, 2));
            InitializePrefabs();
            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, "Literal Seaglide Upgrades");
            Logger.LogInfo($"Plugin Literal Seaglide Upgrades is loaded!");
            Nautilus.Handlers.CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "SeaglideTab", "Seaglide",
                SpriteManager.Get(TechType.Seaglide), "Personal", "Tools");
            Nautilus.Handlers.CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, TechType.Seaglide, "Personal",
                "Tools", "SeaglideTab");
            Nautilus.Handlers.CraftTreeHandler.RemoveNode(CraftTree.Type.Fabricator, "Machines", "Seaglide");
            Nautilus.Handlers.CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "ExtraUpgrades",
                    "Speed Upgrades (Extras)",
                    SpriteManager.Get(TechType.Seaglide), "Personal", "Tools", "SeaglideTab");
            

            if (ModOptions.debugmode)
            {
                Logger.LogInfo("Debug mode is enabled");
            }

        }

        public static TechType[] allowedtech;
        public int timer = 0;
        public bool defineallowedtech;
        public int currentspeed;
        int _currentefficiency;
        public Seaglide instance;
        int _numticks = 0;
        public StorageContainer[] seaglidestorage; //to patch into...i dislike that i have to do this
        public static bool CollectedDefaultValues;
        public bool subscribed;
        

        public void Update()
        {
            timer += 1;
            if (ModOptions.debugmode)
            {
                Logger.LogDebug($"Current Timer: {timer}");
                Logger.LogDebug($"Current Speed: {currentspeed}");
                Logger.LogDebug($"Current Efficiency: {_currentefficiency}");
            }

            if (timer == 3000 && hasalreadyran == false)
            {
                timer = 0;

                Logger.LogInfo("Intitial Update ran! Timer Reset.");
                hasalreadyran = true;

            }
            else if (timer == 500)
            {
                timer = 0;
                Logger.LogInfo("Timer reset.");
                Logger.LogInfo($"The number of ticks that have passed is: {_numticks}");
            }
            
            

            if (Inventory.main != null)
            {

                PlayerTool heldtool = Inventory.main.GetHeldTool();
                //seaglide tool is usually SeaGlide(Clone) (Seaglide)
                if (heldtool != null)
                {
                    if (heldtool is Seaglide)
                    {
                        instance = heldtool as Seaglide;
                        seaglidestorage = heldtool.gameObject.GetComponents<StorageContainer>();
                        if (timer == 100)
                        {
                            Logger.LogInfo(
                                $"The storage containers for {heldtool} are {seaglidestorage[0].storageRoot}, and {seaglidestorage[1].storageRoot}");
                        }

                        if (seaglidestorage[0] == null)
                        {
                            return;
                        }

                        if (!defineallowedtech)
                        {
                            allowedtech = new TechType[17]
                            {
                                TechType.SeaTreaderPoop, SeaglideSpeedModulePrefab.Info.TechType,
                                SeaglideSpeedModuleMk2.Info.TechType, SeaglideSpeedModuleMk3.Info.TechType,
                                SeaglideEfficiencyModuleMk1.Info.TechType, SeaglideEfficiencyModuleMk2.Info.TechType,
                                SeaglideEfficiencyModuleMk3.Info.TechType, Prefabinfo[0].TechType, 
                                Prefabinfo[1].TechType, Prefabinfo[2].TechType, Prefabinfo[3].TechType, 
                                Prefabinfo[4].TechType, Prefabinfo[5].TechType, Prefabinfo[6].TechType, 
                                Prefabinfo[7].TechType, Prefabinfo[8].TechType, Prefabinfo[9].TechType
                            };

                        }

                        seaglidestorage[0].container.SetAllowedTechTypes(allowedtech);
                        defineallowedtech = true;
                        if (!subscribed) //subscribe to the onAddItem/onRemoveItem delegate
                        {
                            subscribed = true;
                        }

                        bool keyDown = Input.GetKeyDown(ModOptions.OpenUpgradesContainerKey);

                        if (keyDown)
                        {
                            Logger.LogInfo($"Open Storage Container Key Pressed for {heldtool}");
                            if (seaglidestorage[0].open != true)
                            {
                                seaglidestorage[0].container._label = "SEAGLIDE";
                                seaglidestorage[0].Open();
                            }

                        }

                        instance = gameObject.GetComponent<Seaglide>();

                    }
                }
                else
                {
                    if (timer == 10)
                    {
                        Logger.LogWarning("Held Tool is null! It probably doesnt have a selected tool yet.");
                    }
                }
            }
            else
            {
                if (timer == 10)
                {
                    Logger.LogInfo("Load a save to use Literal Seaglide Upgrades");
                }
            }

            if (Player.main == null)
            {
                if (timer == 0)
                {
                    Logger.LogInfo(
                        "Player.main is null! It is likely that a save has not been loaded yet, if not, further debugging required");
                }

                return;
            }

            if (!CollectedDefaultValues)
            {
                var playerController = Player.main.GetComponent<PlayerController>();

                _default1 = playerController.seaglideForwardMaxSpeed;
                _default2 = playerController.seaglideBackwardMaxSpeed;
                _default3 = playerController.seaglideStrafeMaxSpeed;
                _default4 = playerController.seaglideVerticalMaxSpeed;
                _default5 = playerController.seaglideWaterAcceleration;
                if (_default1 == 0 || _default2 == 0 || _default3 == 0 || _default4 == 0 || _default5 == 0)
                {
                    return;
                }

                CollectedDefaultValues = true;
            }
        }

        bool _enablereset;
        float _default1;
        float _default2;
        float _default3;
        float _default4;
        float _default5;

        public void IncreaseSeaglideSpeed(float speed)
        {
            var playerController = Player.main.GetComponent<PlayerController>();
            playerController.seaglideForwardMaxSpeed = _default1 * speed;
            playerController.seaglideBackwardMaxSpeed = _default2 * speed;
            playerController.seaglideStrafeMaxSpeed = _default3 * speed;
            playerController.seaglideVerticalMaxSpeed = _default4 * speed;
            playerController.seaglideWaterAcceleration = _default5 * speed;
            _enablereset = true;
            if (timer == 10)
            {
                Logger.LogInfo($"Seaglid speed increased by a factor of {speed}");
            }
        }

        public void ResetSeaglideSpeed()
        {
            if (!_enablereset)
            {
                if (timer == 100)
                {
                    Logger.LogWarning("Seaglide Speed was never increased! It likely didnt even start yet.");
                }

                return;
            }

            if (_default1 == 0 || _default2 == 0 || _default3 == 0 || _default4 == 0 || _default5 == 0)
            {
                return;
            }

            var playerController = Player.main.GetComponent<PlayerController>();
            playerController.seaglideForwardMaxSpeed = _default1;
            playerController.seaglideBackwardMaxSpeed = _default2;
            playerController.seaglideStrafeMaxSpeed = _default3;
            playerController.seaglideVerticalMaxSpeed = _default4;
            playerController.seaglideWaterAcceleration = _default5;
            if (timer == 100)
            {
                Logger.LogInfo("Seaglide speed Reset");
            }
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
                    Upgradedata[Index].speedmultiplier = Speedmultiplier[Index];
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
