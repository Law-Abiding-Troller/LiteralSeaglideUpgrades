using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Nautilus.Utility;
using System.Collections;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;
using JetBrains.Annotations;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;
using BepInEx.Configuration;
using Mono.Cecil.Cil;

namespace LawAbidingTroller.SeaglideModConcept//Will credit any coders contributing to the mod
{
    [BepInPlugin("com.lawabidingtroller.literalseaglideupgrades", "Literal Seaglide Upgrades", "0.1.0")]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        //default allowed tech
        public TechType[] allowedtech = { TechType.SeaTreaderPoop, SeaglideSpeedModulePrefab.Info.TechType, SeaglideSpeedModuleMk2.Info.TechType, SeaglideSpeedModuleMk3.Info.TechType };
        public static bool mk1SpeedUpgradeInSeaglide = false;
        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public bool hasalreadyran = false;
        public bool allowedtechalreadyset = false;
        public bool debugmode = true;
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

            // Create the Seaglide Upgrade Storage
            StartCoroutine(SetSeaglideUpgrades(TechType.Seaglide));
            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, "Literal Seaglide Upgrades");
            Logger.LogInfo($"Plugin Literal Seaglide Upgrades is loaded!");
            
            if (debugmode == true)
            {
                Logger.LogInfo("Debug mode is enabled");
            }
            
        }
        public int timer = 0;
        bool easteregg;
        public void Update()
        {
            timer += 1;
            if (debugmode == true)
            {
                Logger.LogInfo(timer.ToString());
            }
            if (timer == 300 && hasalreadyran == false)
            {
                timer = 0;
                Logger.LogInfo("Intitial Update ran! Timer Reset.");
                hasalreadyran = true;
            }
            else if (timer == 100)
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
                        var tempstorage = heldtool.gameObject.GetComponent<StorageContainer>();
                        bool keyDown = Input.GetKeyDown(KeyCode.V);
                        if (keyDown)
                        {
                            Logger.LogInfo("Open Storage Container Key Pressed!");
                            Logger.LogInfo($"The held tool is, in fact, {heldtool}");
                            
                            if (tempstorage != null)
                            {

                                if (tempstorage.open != true)
                                {
                                    tempstorage.Open();
                                }
                                
                            }
                            
                        }
                        if (!allowedtechalreadyset && tempstorage.container != null)
                        {
                            tempstorage.container.SetAllowedTechTypes(allowedtech);
                            allowedtechalreadyset = true;
                        }
                        if (tempstorage != null && tempstorage.open)
                        {
                            //ManageSpeed(tempstorage.container, mk1SpeedUpgradeInSeaglide, SeaglideSpeedModulePrefab.mk1speedmultiplier, SeaglideSpeedModulePrefab.Info.TechType);
                            if (tempstorage.container.Contains(SeaglideSpeedModulePrefab.Info.TechType))
                            {
                                IncreaseSeaglideSpeed(SeaglideSpeedModulePrefab.mk1speedmultiplier);
                            }
                            else if (tempstorage.container.Contains(SeaglideSpeedModuleMk2.Info.TechType))
                            {
                                IncreaseSeaglideSpeed(SeaglideSpeedModuleMk2.mk2speedmultiplier);
                            }
                            else if (tempstorage.container.Contains(SeaglideSpeedModuleMk3.Info.TechType))
                            {
                                IncreaseSeaglideSpeed(SeaglideSpeedModuleMk3.mk3speedmultiplier);
                            }
                            else if (tempstorage.container.Contains(TechType.SeaTreaderPoop))
                            {
                                IncreaseSeaglideSpeed(1 / 2f);
                            }
                            else if (!tempstorage.container.Contains(SeaglideSpeedModulePrefab.Info.TechType) && !tempstorage.container.Contains(SeaglideSpeedModuleMk2.Info.TechType) && !tempstorage.container.Contains(SeaglideSpeedModuleMk3.Info.TechType) && !tempstorage.container.Contains(TechType.SeaTreaderPoop)) 
                            {
                                ResetSeaglideSpeed();
                            }
                            else
                            {
                                if (tempstorage.container.IsEmpty())
                                {
                                    ResetSeaglideSpeed();
                                }
                            }
                        }
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

        public void IncreaseSeaglideSpeed(float speed)
        {
            var playerController = Player.main.GetComponent<PlayerController>();
            playerController.seaglideForwardMaxSpeed = 25f * speed;
            playerController.seaglideBackwardMaxSpeed = 6.35f * speed;
            playerController.seaglideStrafeMaxSpeed = 6.35f * speed;
            playerController.seaglideVerticalMaxSpeed = 6.34f * speed;
            playerController.seaglideWaterAcceleration = 36.56f * speed;
            Logger.LogInfo($"Seaglider speed increased by a factor of {speed}");
        }
        public void ResetSeaglideSpeed()
        {
            var playerController = Player.main.GetComponent<PlayerController>();
            playerController.seaglideForwardMaxSpeed = 25f;
            playerController.seaglideBackwardMaxSpeed =6.35f;
            playerController.seaglideStrafeMaxSpeed = 6.35f;
            playerController.seaglideVerticalMaxSpeed = 6.34f;
            playerController.seaglideWaterAcceleration = 36.56f;
            playerController.seaglideSwimDrag = 2.5f;
            Logger.LogInfo("Seaglide speed Reset");
        }
        //public void ManageSpeed(ItemsContainer container, bool module, float speed, TechType techtypw)
        //{
            //var speedornospeed = container.Contains(techtypw);            
            //module = speedornospeed;
            //if (module && !multiplied)
            //{
                //if (Player.main.motorMode == Player.MotorMode.Seaglide && !multiplied)
                //{
                    //IncreaseSeaglideSpeed(speed);
                    //multiplied = true;
                //}
            //}
            //else
            //{
                //if (multiplied)
                //{
                    //ResetSeaglideSpeed();
                    //multiplied = false;
                //}
            //}
        //}
        private void InitializePrefabs()
        {
            SeaglideSpeedModulePrefab.Register();
            SeaglideSpeedModuleMk2.Register();
            SeaglideSpeedModuleMk3.Register();
            Logger.LogInfo("All (or most) Prefabs successfully initalized!");
        }

    }
}
