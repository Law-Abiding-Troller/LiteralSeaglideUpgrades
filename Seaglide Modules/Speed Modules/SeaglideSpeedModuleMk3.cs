﻿using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModuleMk3
    {
        public static UpgradeData Mk3Data = new UpgradeData(8.0f);
        public static CustomPrefab Mk3Speedprefab;
        public static PrefabInfo Info;
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static bool Mk3SpeedModuleInSeaglide;
        public static void Register()
        {

            Info = PrefabInfo.WithTechType("SeaglideSpeedUpgradeMk3", "Seaglide Speed Upgrade Module Mk 3", "Mk 3 Speed Upgrade Module for the Seaglide. 8x normal speed. Only so fast you can make a Seaglide")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            ModOptions.upgradeValues.Add(Info.TechType, Mk3Data);
            Mk3Speedprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, TechType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Mk3Speedprefab.SetGameObject(clone);
            Mk3Speedprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.Lubricant),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit),
                    new CraftData.Ingredient(TechType.Battery),
                    new CraftData.Ingredient(SeaglideSpeedModuleMk2.Info.TechType)
                }
            })
            .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            Mk3Speedprefab.SetUnlock(TechType.Seaglide);
            Mk3Speedprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                Plugin.LiteralSeaglideUpgrades);
            Mk3Speedprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideSpeedUpgradeMk2 successfully initalized!");
        }

    }
}