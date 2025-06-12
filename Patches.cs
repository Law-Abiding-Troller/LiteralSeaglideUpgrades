using System.Linq;
using UnityEngine;
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;
using HarmonyLib;
using LawAbidingTroller.LiteralSeaglideUpgrades;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;

namespace LawAbidingTroller.SeaglideModConcept;

[HarmonyPatch(typeof(Seaglide))]
public class SeaglidePatches
{
    [HarmonyPatch(nameof(Seaglide.Update))]
    [HarmonyPostfix]
    public static void Postfix(Seaglide __instance)
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
}

[HarmonyPatch(typeof(PlayerController))]
public class PlayerControllerPatches
{
    public static float ToChangeSpeedMultiplier;
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
        tempstorage[0].container.onAddItem += Plugin.OnItemAdded;
        tempstorage[0].container.onRemoveItem += Plugin.OnItemRemoved;
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

    public class OnItem
    {
        public static void Added(InventoryItem item)
        {
            
        }

        public static void Removed(InventoryItem item)
        {
            
        }
    }
}