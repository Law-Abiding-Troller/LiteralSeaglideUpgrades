using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Nautilus.Utility;
using System.Collections;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;
using JetBrains.Annotations;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;
using BepInEx.Configuration;
using Mono.Cecil.Cil;
using Nautilus.Handlers;
using rail;
using LawAbidingTroller.LiteralSeaglideUpgrades;

namespace LawAbidingTroller.SeaglideModConcept//Will credit any coders contributing to the mod
{
    [BepInPlugin("com.lawabidingtroller.literalseaglideupgrades", "Literal Seaglide Upgrades", "0.2.0")]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        //default allowed tech

        public static bool mk1SpeedUpgradeInSeaglide = false;
        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public bool hasalreadyran = false;
        public bool debugmode = false;
        //only seaglide for now. plan to add compatability to Ramune's SeaglideUpgrades mod.
        public IEnumerator SetSeaglideUpgrades(TechType techtype)
        {
            // Fetch the prefab:
            Logger.LogInfo("Fetching prefab for tech type...");
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(techtype);
            // Wait for the prefab task to complete:
            yield return task;
            Logger.LogInfo("Prefab fetch completed.");
            // Get the prefab:
            GameObject prefab = task.GetResult();
            Logger.LogInfo($"The prefab is {prefab}");
            // Use the prefab to add a storage contianer:
            PrefabUtils.AddStorageContainer(prefab, "SeaglideUpgradeStorage", "SeaglideUpgradeStorageChild", 2, 2, true);
            InitializePrefabs();
        }
        private void Awake()
        {
            // set project-scoped logger instance
            Logger = base.Logger;
            // Create the Seaglide Upgrade Storage for the Seaglides
            StartCoroutine(SetSeaglideUpgrades(TechType.Seaglide));
            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, "Literal Seaglide Upgrades");
            Logger.LogInfo($"Plugin Literal Seaglide Upgrades is loaded!");
            Nautilus.Handlers.CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "SeaglideTab", "Seaglide", SpriteManager.Get(TechType.Seaglide), "Personal", "Tools");
            Nautilus.Handlers.CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, TechType.Seaglide, "Personal", "Tools", "SeaglideTab");
            Nautilus.Handlers.CraftTreeHandler.RemoveNode(CraftTree.Type.Fabricator, "Machines", "Seaglide");
            
            if (debugmode == true)
            {
                Logger.LogInfo("Debug mode is enabled");
            }
            
        }
        TechType[] allowedtech;
        private void Start()
        {
            
        }
        
        public int timer = 0;
        bool defineallowedtech = false;
        int currentspeed;
        int currentefficiency;
        Seaglide instance;
        public void Update()
        {
            timer += 1;
            if (debugmode == true)
            {
                Logger.LogInfo(timer.ToString());
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
                        var tempstorage = heldtool.gameObject.GetComponents<StorageContainer>();
                        if (timer == 100)
                        { 
                            Logger.LogInfo($"The storage containers for {heldtool} are {tempstorage[0].storageRoot}, and {tempstorage[1].storageRoot}");
                        }
                        bool keyDown = Input.GetKeyDown(KeyCode.V);
                        if (timer <= 100
                            && !defineallowedtech
                            && tempstorage != null
                            )
                        {
                            allowedtech = new TechType[] { TechType.SeaTreaderPoop, SeaglideSpeedModulePrefab.Info.TechType, SeaglideSpeedModuleMk2.Info.TechType, SeaglideSpeedModuleMk3.Info.TechType, SeaglideEfficiencyModuleMk1.Info.TechType };
                            tempstorage[0].container.SetAllowedTechTypes(allowedtech);
                        }
                        
                        if (keyDown)
                        {
                            Logger.LogInfo($"Open Storage Container Key Pressed for {heldtool}");
                            if (tempstorage[0] != null)
                            {

                                if (tempstorage[0].open != true)
                                {
                                    tempstorage[0].container._label = "SEAGLIDE";
                                    tempstorage[0].Open();
                                }
                                
                            }
                            
                        }
                        if (tempstorage[0].container.Contains(SeaglideEfficiencyModuleMk1.Info.TechType))
                        {
                            currentefficiency = 1;
                        }
                        else if (tempstorage[0].container.Contains(SeaglideEfficiencyModuleMk2.Info.TechType))
                        {
                            currentefficiency = 2;
                        }
                        else if (tempstorage[0].container.Contains(SeaglideEfficiencyModuleMk3.Info.TechType))
                        {
                            currentefficiency = 3;
                        }
                        if (tempstorage[0].container.Contains(SeaglideSpeedModulePrefab.Info.TechType))
                        {
                        currentspeed = 2;
                        }
                        else if (tempstorage[0].container.Contains(SeaglideSpeedModuleMk2.Info.TechType))
                        {
                        currentspeed = 3;
                        }
                        else if (tempstorage[0].container.Contains(SeaglideSpeedModuleMk3.Info.TechType))
                        {
                        currentspeed = 4;
                        }
                        else if (tempstorage[0].container.Contains(TechType.SeaTreaderPoop))
                        {
                        currentspeed = 1;
                        }
                        else if (!tempstorage[0].container.Contains(SeaglideSpeedModulePrefab.Info.TechType) && !tempstorage[0].container.Contains(SeaglideSpeedModuleMk2.Info.TechType) && !tempstorage[0].container.Contains(SeaglideSpeedModuleMk3.Info.TechType) && !tempstorage[0].container.Contains(TechType.SeaTreaderPoop)) 
                        {
                        currentspeed = 0;
                        }
                        else
                        {
                            if (tempstorage[0].container.IsEmpty())
                            {
                                currentspeed = 0;
                            }
                        }
                        instance = gameObject.GetComponent<Seaglide>();
                        UpdateSeaglideSpeed(currentspeed, instance);
                        UpdateEnergyEfficiency(currentefficiency, instance);
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
        }
        float increasedspeed;
        bool enablereset;
        float default1;
        float default2;
        float default3;
        float default4;
        float default5;
        public void IncreaseSeaglideSpeed(float speed)
        {
            var playerController = Player.main.GetComponent<PlayerController>();
            
            default1 = playerController.seaglideForwardMaxSpeed;
            default2 = playerController.seaglideBackwardMaxSpeed;
            default3 = playerController.seaglideStrafeMaxSpeed;
            default4 = playerController.seaglideVerticalMaxSpeed;
            default5 = playerController.seaglideWaterAcceleration;
            playerController.seaglideForwardMaxSpeed = default1 * speed;
            playerController.seaglideBackwardMaxSpeed = default2 * speed;
            playerController.seaglideStrafeMaxSpeed = default3 * speed;
            playerController.seaglideVerticalMaxSpeed = default4 * speed;
            playerController.seaglideWaterAcceleration = default5 * speed;
            increasedspeed = speed;
            enablereset = true;
            if (timer == 10)
            {
                Logger.LogInfo($"Seaglid speed increased by a factor of {increasedspeed}");
            }
        }
        public void ResetSeaglideSpeed()
        {
            if (!enablereset)
            {
                if (timer == 100)
                {
                    Logger.LogWarning("Seaglide Speed was never increased! It likely didnt even start yet.");
                }
                return;
            }
            var playerController = Player.main.GetComponent<PlayerController>();
            playerController.seaglideForwardMaxSpeed = default1 / increasedspeed;
            playerController.seaglideBackwardMaxSpeed = default2 / increasedspeed;
            playerController.seaglideStrafeMaxSpeed = default3 / increasedspeed;
            playerController.seaglideVerticalMaxSpeed = default4 / increasedspeed;
            playerController.seaglideWaterAcceleration = default5 / increasedspeed;
            if (timer == 100)
            {
                Logger.LogInfo("Seaglide speed Reset");
            }
        }
        public void UpdateSeaglideSpeed(int currentspeed, Seaglide instance)
        {
            switch (currentspeed)
            {
                case 0:
                    ResetSeaglideSpeed();
                    Player.main.UpdateMotorMode();
                    break;
                case 1:
                    IncreaseSeaglideSpeed(1 / 2f);
                    Player.main.UpdateMotorMode();
                    if (instance.activeState && instance.timeSinceUse >= 1f)
                    {
                        instance.energyMixin.AddEnergy(0f - 0.1f);
                    }
                    break;
                case 2:
                    IncreaseSeaglideSpeed(SeaglideSpeedModulePrefab.mk1speedmultiplier);
                    Player.main.UpdateMotorMode();
                    break;
                case 3:
                    IncreaseSeaglideSpeed(SeaglideSpeedModuleMk2.mk2speedmultiplier);
                    Player.main.UpdateMotorMode();
                    break;
                case 4:
                    IncreaseSeaglideSpeed(SeaglideSpeedModuleMk3.mk3speedmultiplier);
                    Player.main.UpdateMotorMode();
                    break;
                default:
                    Logger.LogWarning("Unknown Speed Selection. Defaulting to normal");
                    ResetSeaglideSpeed();
                    Player.main.UpdateMotorMode();
                    break;
            }
        }
        public void UpdateEnergyEfficiency(int currentefficiency, Seaglide instance)
        {
            
            switch (currentefficiency)
            {
                case 0:
                    break;
                case 1:
                    if (instance.activeState && instance.timeSinceUse >= 1f)
                    {
                        instance.energyMixin.AddEnergy(SeaglideEfficiencyModuleMk1.mk1efficiencymultiplier);
                    }
                    break;
                case 2:
                    if (instance.activeState && instance.timeSinceUse >= 1f)
                    {
                        instance.energyMixin.AddEnergy(SeaglideEfficiencyModuleMk2.mk2efficiencymultiplier);
                    }
                    break;
                case 3:
                    if (instance.activeState && instance.timeSinceUse >= 1f)
                    {
                        instance.energyMixin.AddEnergy(SeaglideEfficiencyModuleMk3.mk3efficiencymultiplier);
                    }
                    break;
                default :
                    break;
            }
        }
        private void InitializePrefabs()
        {
            SeaglideSpeedModulePrefab.Register();
            SeaglideSpeedModuleMk2.Register();
            SeaglideSpeedModuleMk3.Register();
            SeaglideEfficiencyModuleMk1.Register();
            SeaglideEfficiencyModuleMk2.Register();
            SeaglideEfficiencyModuleMk3.Register();
            Logger.LogInfo("All (or most) Prefabs successfully initalized!");
        }

    }
}
