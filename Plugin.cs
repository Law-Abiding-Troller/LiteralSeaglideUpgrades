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
     * 
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
                            seaglidestorage[0].container.onAddItem += OnItemAdded;
                            seaglidestorage[0].container.onRemoveItem += OnItemRemoved;
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
                        UpdateExtraUpgrades(ModOptions.ExtraSpeedUpgrades);
                        UpdateSeaglideSpeed(currentspeed, instance);
                        UpdateEnergyEfficiency(_currentefficiency, instance);

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

        public void UpdateSeaglideSpeed(int select, Seaglide seaglideinstance)
        {
            switch (select)
            {
                case 0:
                    ResetSeaglideSpeed();
                    Player.main.UpdateMotorMode();
                    break;
                case 1:
                    IncreaseSeaglideSpeed(1 / 2f);
                    Player.main.UpdateMotorMode();
                    if (seaglideinstance.activeState && seaglideinstance.timeSinceUse >= 1f)
                    {
                        seaglideinstance.energyMixin.AddEnergy(0f - 0.1f);
                    }

                    break;
                case 2:
                    IncreaseSeaglideSpeed(SeaglideSpeedModulePrefab.Mk1Speedmultiplier);
                    Player.main.UpdateMotorMode();
                    break;
                case 3:
                    IncreaseSeaglideSpeed(SeaglideSpeedModuleMk2.Mk2Speedmultiplier);
                    Player.main.UpdateMotorMode();
                    break;
                case 4:
                    IncreaseSeaglideSpeed(SeaglideSpeedModuleMk3.Mk3Speedmultiplier);
                    Player.main.UpdateMotorMode();
                    break;
                case 5:
                    IncreaseSeaglideSpeed(12f);
                    Player.main.UpdateMotorMode();
                    break;
                case 6:
                    IncreaseSeaglideSpeed(17f);
                    Player.main.UpdateMotorMode();
                    break;
                case 7:
                    IncreaseSeaglideSpeed(23f);
                    Player.main.UpdateMotorMode();
                    break;
                case 8:
                    IncreaseSeaglideSpeed(30f);
                    Player.main.UpdateMotorMode();
                    break;
                case 9:
                    IncreaseSeaglideSpeed(38f);
                    Player.main.UpdateMotorMode();
                    break;
                case 10:
                    IncreaseSeaglideSpeed(47f);
                    Player.main.UpdateMotorMode();
                    break;
                case 11:
                    IncreaseSeaglideSpeed(57f);
                    Player.main.UpdateMotorMode();
                    break;
                case 12:
                    IncreaseSeaglideSpeed(68f);
                    Player.main.UpdateMotorMode();
                    break;
                case 13:
                    IncreaseSeaglideSpeed(80f);
                    Player.main.UpdateMotorMode();
                    break;
                case 14:
                    IncreaseSeaglideSpeed(100f);
                    Player.main.UpdateMotorMode();
                    break;
                default:
                    Logger.LogWarning("Unknown Speed Selection. Defaulting to normal");
                    ResetSeaglideSpeed();
                    Player.main.UpdateMotorMode();
                    break;
            }
        }

        public void UpdateEnergyEfficiency(int
            selection, Seaglide seaglideinstance)
        {

            switch (selection)
            {
                case 0:
                    break;
                case 1:
                    if (seaglideinstance.activeState && seaglideinstance.timeSinceUse >= 1f)
                    {
                        seaglideinstance.energyMixin.AddEnergy(SeaglideEfficiencyModuleMk1.Mk1Efficiencymultiplier);
                    }

                    break;
                case 2:
                    if (seaglideinstance.activeState && seaglideinstance.timeSinceUse >= 1f)
                    {
                        seaglideinstance.energyMixin.AddEnergy(SeaglideEfficiencyModuleMk2.Mk2Efficiencymultiplier);
                    }

                    break;
                case 3:
                    if (seaglideinstance.activeState && seaglideinstance.timeSinceUse >= 1f)
                    {
                        seaglideinstance.energyMixin.AddEnergy(SeaglideEfficiencyModuleMk3.Mk3Efficiencymultiplier);
                    }

                    break;
            }
        }

        public static void
            OnItemRemoved(
                InventoryItem item) //custom behavior for when one of the upgrades (or bleach) is removed from the storage container
        {
            if (item == null || item.item == null) //check if its somehow null
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"item or item.item is null! Further debugging required");
                }
                return;
            }

            if (item.item.GetTechType() == allowedtech[0]) //give the ability to use the air bladder back to the player
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[0]}");
                }
            }

            //reduce the capacity when the mk1 upgrade is removed, check if it has been already upgraded, let me know if it should cause issues on reload
            if (item.item.GetTechType() == allowedtech[1])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[1]}");
                }
            }

            //same as mk1, but more
            if (item.item.GetTechType() == allowedtech[2])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[2]}");
                }
            }

            //same as mk2, but even more
            if (item.item.GetTechType() == allowedtech[3])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[3]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[4])
            {
                _currentefficiency = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[4]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[5])
            {
                _currentefficiency = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[5]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[6])
            {
                _currentefficiency = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[6]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[7])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[7]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[8])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[8]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[9])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[9]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[10])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[10]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[11])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[11]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[12])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[12]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[13])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[13]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[14])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[14]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[15])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[15]}");
                }
            }

            if (item.item.GetTechType() == allowedtech[16])
            {
                currentspeed = 0;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item removed: {allowedtech[17]}");
                }
            }
        }

        public static void
            OnItemAdded(
                InventoryItem item) //custom behavior for if any of the upgrades (or bleach) has been added to the storage container
        {
            if (item == null || item.item == null) //check if its somehow null
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"item or item.item is null! Further debugging required!");
                }
                return;
            }

            if (item.item.GetTechType() == allowedtech[0]) //give the ability to use the air bladder back to the player
            {
                currentspeed = 1;
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[0]}");
                }
            }

            //reduce the capacity when the mk1 upgrade is removed, check if it has been already upgraded, let me know if it should cause issues on reload
            if (item.item.GetTechType() == allowedtech[1])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[1]}");
                }
                currentspeed = 2;
            }

            //same as mk1, but more
            if (item.item.GetTechType() == allowedtech[2])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[2]}");
                }
                currentspeed = 3;
            }

            //same as mk2, but even more
            if (item.item.GetTechType() == allowedtech[3])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[3]}");
                }
                currentspeed = 4;
            }

            if (item.item.GetTechType() == allowedtech[4])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[4]}");
                }
                _currentefficiency = 1;
            }

            if (item.item.GetTechType() == allowedtech[5])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[5]}");
                }
                _currentefficiency = 2;
            }

            if (item.item.GetTechType() == allowedtech[6])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[6]}");
                }
                _currentefficiency = 3;
            }

            if (item.item.GetTechType() == allowedtech[7])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[7]}");
                }
                currentspeed = 5;
            }

            if (item.item.GetTechType() == allowedtech[8])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[8]}");
                }
                currentspeed = 6;
            }

            if (item.item.GetTechType() == allowedtech[9])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[9]}");
                }
                currentspeed = 7;
            }

            if (item.item.GetTechType() == allowedtech[10])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[10]}");
                }
                currentspeed = 8;
            }

            if (item.item.GetTechType() == allowedtech[11])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[11]}");
                }
                currentspeed = 9;
            }

            if (item.item.GetTechType() == allowedtech[12])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[12]}");
                }
                currentspeed = 10;
            }

            if (item.item.GetTechType() == allowedtech[13])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[13]}");
                }
                currentspeed = 11;
            }

            if (item.item.GetTechType() == allowedtech[14])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[14]}");
                }
                currentspeed = 12;
            }

            if (item.item.GetTechType() == allowedtech[15])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[15]}");
                }
                currentspeed = 13;
            }

            if (item.item.GetTechType() == allowedtech[16])
            {
                if (ModOptions.debugmode)
                {
                    Logger.LogDebug($"Item added: {allowedtech[16]}");
                }
                currentspeed = 14;
            }
        }
        
        public static float[] Speedmultiplier = { 8, 12, 17, 23, 30, 38, 47, 57, 68, 80 };
        public static CustomPrefab[] Speedprefab = new CustomPrefab[10];
        public static PrefabInfo[] Prefabinfo = new PrefabInfo[10];
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
