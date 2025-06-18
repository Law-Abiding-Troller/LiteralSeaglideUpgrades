using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;
using HarmonyLib;
using LawAbidingTroller.LiteralSeaglideUpgrades;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;

namespace LawAbidingTroller.SeaglideModConcept;

[HarmonyPatch(typeof(Seaglide))]
public class SeaglidePatches
{
    public float HeldEfficiency;
    [HarmonyPatch(nameof(Seaglide.Update))]
    [HarmonyPostfix]
    public static void Update_Postfix(Seaglide __instance)
    {
        if (__instance == null) return;
        var tempstorage = __instance.GetComponents<StorageContainer>();
        if( tempstorage == null) return;
        if (Input.GetKeyDown(ModOptions.OpenUpgradesContainerkey))
        {
            if (tempstorage[0].open) {ErrorMessage.AddMessage("Close 'SEAGLIDE' to open it!"); return;}
            tempstorage[0].Open();
        }
    }
}

[HarmonyPatch(typeof(Seaglide))]
[HarmonyPatch(nameof(Seaglide.UpdateEnergy))]
public static class Seaglide_UpdateEnergy_Patch
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codeMatcher = new CodeMatcher(instructions)
            .MatchForward(false, new CodeMatch(OpCodes.Ldc_R4, 0.1))
            .SetAndAdvance(OpCodes.Ldarg_0, null)
            .Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UpgradeData), "GetEfficiency")));
        return codeMatcher.InstructionEnumeration();
    }
}

[HarmonyPatch(typeof(PlayerController))]
public class PlayerControllerPatches
{
    
    [HarmonyPatch(nameof(PlayerController.SetMotorMode))]
    [HarmonyPrefix]
    public static void SetMotorMode_Prefix(PlayerController __instance, 
        Player.MotorMode newMotorMode, out SpeedData __state)
    {
        __state = new SpeedData(__instance.seaglideForwardMaxSpeed, 
            __instance.seaglideBackwardMaxSpeed, 
            __instance.seaglideStrafeMaxSpeed, 
            __instance.seaglideVerticalMaxSpeed,
            __instance.seaglideWaterAcceleration, 
            __instance.seaglideSwimDrag);
        var held = Inventory.main.GetHeldTool();
        if (held == null || held is not Seaglide) return;
        var speed = UpgradeData.CalculateSpeed(held as Seaglide);
        __instance.seaglideForwardMaxSpeed *= speed;
        __instance.seaglideBackwardMaxSpeed *= speed;
        __instance.seaglideStrafeMaxSpeed *= speed;
        __instance.seaglideVerticalMaxSpeed *= speed;
        __instance.seaglideWaterAcceleration *= speed/16f;
        __instance.seaglideSwimDrag /= speed/16f;
    }
    
    [HarmonyPatch(nameof(PlayerController.SetMotorMode))]
    [HarmonyPostfix]
    public static void SetMotorMode_Postfix(PlayerController __instance, 
        Player.MotorMode newMotorMode, SpeedData __state)
    {
        __instance.seaglideForwardMaxSpeed = __state.DefaultseaglideForwardMaxSpeed;
        __instance.seaglideBackwardMaxSpeed = __state.DefaultseaglideBackwardMaxSpeed;
        __instance.seaglideStrafeMaxSpeed = __state.DefaultseaglideStrafeMaxSpeed;
        __instance.seaglideVerticalMaxSpeed = __state.DefaultseaglideVerticalMaxSpeed;
        __instance.seaglideWaterAcceleration = __state.DefaultseaglideWaterAcceleration;
        __instance.seaglideSwimDrag = __state.DefaultseaglideSwimDrag;
    }
}

[HarmonyPatch(typeof(PlayerTool))]
public class PlayerToolPatches
{
    [HarmonyPatch(nameof(PlayerTool.Awake))]
    [HarmonyPrefix]
    public static void Awake_Postfix(PlayerTool __instance)
    {
        if (__instance == null) return;
        if (__instance is not Seaglide) return;
        var tempstorage = __instance.GetComponents<StorageContainer>();
        if (tempstorage == null) return;
        tempstorage[0].container._label = "SEAGLIDE";
        var allowedtech = new[]
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
        UpgradeData eastereggvalue = new(0.5f,-0.1f, __instance as Seaglide);
        ModOptions.upgradeValues.Add(TechType.SeaTreaderPoop, eastereggvalue);
    }
}
 
     public class UpgradeData
     {
         public float speedmultiplier = 0f;
         public float efficiencymultiplier = 0f;
         public Seaglide SeaglideInstance;

         public UpgradeData(float speed = 0, float efficiency = 0, Seaglide identifier = null)
         {
             speedmultiplier = speed;
             efficiencymultiplier = efficiency;
             SeaglideInstance = identifier;
         }
         public static float GetEfficiency(Seaglide instance)
         {
             var tempstorage = instance.GetComponent<StorageContainer>();
             if (tempstorage == null)
             {
                 Plugin.Logger.LogError("No storage container found on Seaglide. WTF Happened.");
                 return 0;
             }
             UpgradeData upgradeData;
             float highestEfficiency = 0;
             foreach (var item in tempstorage.container.GetItemTypes())
             {
                 if (!ModOptions.upgradeValues.TryGetValue(item, out upgradeData))
                 {
                     Plugin.Logger.LogError($"Cannot get TechType ({item}) from upgrade values dictionary.");
                     continue;
                 }
                 highestEfficiency = Mathf.Max(highestEfficiency, upgradeData.efficiencymultiplier);
             }
             return 0.1f - highestEfficiency;
         }

         public static float CalculateSpeed(Seaglide instance)
         {
             var tempstorage = instance.GetComponent<StorageContainer>();
             if (tempstorage == null)
             {
                 Plugin.Logger.LogError("No storage container found on Seaglide. WTF Happened.");
                 return 0;
             }
             UpgradeData upgradeData;
             float highestSpeed = 0;
             foreach (var item in tempstorage.container.GetItemTypes())
             {
                 if (!ModOptions.upgradeValues.TryGetValue(item, out upgradeData))
                 {
                     Plugin.Logger.LogError($"Cannot get TechType ({item}) from upgrade values.");
                     continue;
                 }
                 highestSpeed = Mathf.Max(highestSpeed, upgradeData.speedmultiplier);
             }
             return highestSpeed;
         }
     }

     public class SpeedData
     {
         public float DefaultseaglideForwardMaxSpeed;
         public float DefaultseaglideBackwardMaxSpeed;
         public float DefaultseaglideStrafeMaxSpeed;
         public float DefaultseaglideVerticalMaxSpeed;
         public float DefaultseaglideWaterAcceleration;
         public float DefaultseaglideSwimDrag;

         public SpeedData(float defaultForwardMaxSpeed,
             float defaultBackwardMaxSpeed,  
             float defaultStrafeMaxSpeed, 
             float defaultVerticalMaxSpeed,
             float defaultWaterAcceleration, 
             float defaultSwimDrag)
         {
             DefaultseaglideForwardMaxSpeed = defaultForwardMaxSpeed;
             DefaultseaglideBackwardMaxSpeed = defaultBackwardMaxSpeed;
             DefaultseaglideStrafeMaxSpeed = defaultStrafeMaxSpeed;
             DefaultseaglideVerticalMaxSpeed = defaultVerticalMaxSpeed;
             DefaultseaglideWaterAcceleration = defaultWaterAcceleration;
             DefaultseaglideSwimDrag = defaultSwimDrag;
         }
     }