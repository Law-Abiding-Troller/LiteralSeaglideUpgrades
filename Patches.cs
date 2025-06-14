using System.Linq;
using UnityEngine;
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;
using HarmonyLib;
using LawAbidingTroller.LiteralSeaglideUpgrades;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;
using RootMotion.FinalIK;

namespace LawAbidingTroller.SeaglideModConcept;

[HarmonyPatch(typeof(Seaglide))]
public class SeaglidePatches
{
    public static float Efficiency;
    [HarmonyPatch(nameof(Seaglide.Update))]
    [HarmonyPostfix]
    public static void Update_Postfix(Seaglide __instance)
    {
        if (__instance == null) return;
        var tempstorage = __instance.GetComponents<StorageContainer>();
        if( tempstorage==null )return;
        if (Input.GetKeyDown(ModOptions.OpenUpgradesContainerkey))
        {
            if (tempstorage[0].open) {ErrorMessage.AddMessage("Close 'SEAGLIDE' to open it!"); return;}
            tempstorage[0].Open();
        }
    }

    [HarmonyPatch(nameof(Seaglide.UpdateEnergy))]
    [HarmonyPostfix]
    public static void UpdateEnergy_Postfix(Seaglide __instance)
    {
        if (__instance == null) return;//Since I'm not willing to make a transpiler to change one number,
        if (__instance.activeState)// copy-paste SN code and use it to add the efficiency percentage.
        {
            __instance.timeSinceUse += Time.deltaTime;
            if (__instance.timeSinceUse >= 1f)
            {
                __instance.energyMixin.AddEnergy(Efficiency);
                __instance.timeSinceUse -= 1f;
            }
        }
        if (__instance.powerGlideActive)
        {
            float num = (Efficiency*10) * Time.deltaTime;//Multiply by 10 to match percentage.
            if (__instance.energyMixin.charge >= num)
            {
                __instance.energyMixin.ConsumeEnergy(num);
            }
        }
        
    }
}

[HarmonyPatch(typeof(PlayerController))]
public class PlayerControllerPatches
{
    public static float ToChangeSpeedMultiplier;
    public static bool DoChangeSpeed;
    [HarmonyPatch(nameof(PlayerController.Start))]
    [HarmonyPostfix]
    public static void Start_Postfix(PlayerController __instance)
    {
        if (__instance == null) return;
        
    }
    [HarmonyPatch(nameof(PlayerController.SetMotorMode))]
    [HarmonyPrefix]
    public static void SetMotorMode_Prefix(PlayerController __instance, Player.MotorMode newMotorMode)
    {
        if (newMotorMode != Player.MotorMode.Seaglide) return;
        if (ToChangeSpeedMultiplier <= 0) return;
        if (!DoChangeSpeed) return;
        __instance.seaglideForwardMaxSpeed *= ToChangeSpeedMultiplier;
        __instance.seaglideBackwardMaxSpeed *= ToChangeSpeedMultiplier;
        __instance.seaglideStrafeMaxSpeed *= ToChangeSpeedMultiplier;
        __instance.seaglideVerticalMaxSpeed *= ToChangeSpeedMultiplier;
        __instance.seaglideWaterAcceleration *= ToChangeSpeedMultiplier/8f;
        __instance.seaglideSwimDrag /= ToChangeSpeedMultiplier/8f;
    }
}

[HarmonyPatch(typeof(PlayerTool))]
public class PlayerToolPatches
{
    [HarmonyPatch(nameof(PlayerTool.Awake))]
    [HarmonyPrefix]
    public static void Awake_Prefix(PlayerTool __instance)
    {
        if (__instance == null) return;
        if (__instance is not Seaglide) return;
        var tempstorage = __instance.GetComponents<StorageContainer>();
        if (tempstorage == null) return;
        tempstorage[0].container.onAddItem += OnItem.Added;
        tempstorage[0].container.onRemoveItem += OnItem.Removed;
        tempstorage[0].container._label = "SEAGLIDE";
        if (Plugin.CollectedDefaultValues) return;
        var allowedtech = new TechType[17]
        {
            TechType.SeaTreaderPoop, SeaglideSpeedModulePrefab.Info.TechType,
            SeaglideSpeedModuleMk2.Info.TechType, SeaglideSpeedModuleMk3.Info.TechType,
            SeaglideEfficiencyModuleMk1.Info.TechType, SeaglideEfficiencyModuleMk2.Info.TechType,
            SeaglideEfficiencyModuleMk3.Info.TechType, Plugin.Prefabinfo[0].TechType, 
            Plugin.Prefabinfo[1].TechType, Plugin.Prefabinfo[2].TechType, Plugin.Prefabinfo[3].TechType, 
            Plugin.Prefabinfo[4].TechType, Plugin.Prefabinfo[5].TechType, Plugin.Prefabinfo[6].TechType, 
            Plugin.Prefabinfo[7].TechType, Plugin.Prefabinfo[8].TechType, Plugin.Prefabinfo[9].TechType
        };
        tempstorage[0].container.SetAllowedTechTypes(allowedtech);
    }
}
public class OnItem
     {
         public static void Added(InventoryItem item)
         {
             if (ModOptions.upgradeValues.TryGetValue(item.item.GetTechType(), out UpgradeData upgradeData))
             {
                 if (upgradeData.efficiencymultiplier != 0)
                 {
                     SeaglidePatches.Efficiency = upgradeData.efficiencymultiplier;
                 }
                 else
                 {
                     PlayerControllerPatches.ToChangeSpeedMultiplier = upgradeData.speedmultiplier;
                 }
             }
         }
 
         public static void Removed(InventoryItem item)
         {
             
         }
     }
 
     public class UpgradeData
     {
         public float speedmultiplier = 0f;
         public float efficiencymultiplier = 0f;

         public UpgradeData(float speed = 0, float efficiency = 0)
         {
             speedmultiplier = speed;
             efficiencymultiplier = efficiency;
         }
     }