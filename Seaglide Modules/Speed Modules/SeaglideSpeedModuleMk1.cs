using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModulePrefab
    {
        public static UpgradeData Mk1Data = new UpgradeData(2.0f);
        public static CustomPrefab Mk1Speedprefab;
        public static PrefabInfo Info;
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        { 
            Info = PrefabInfo.WithTechType("SeaglideSpeedUpgradeMk1", "Seaglide Speed Upgrade Module Mk 1", "Mk 1 Speed Upgrade Module for the Seaglide. 2x normal speed.")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            ModOptions.upgradeValues.Add(Info.TechType, Mk1Data);
            Mk1Speedprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, TechType);
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Mk1Speedprefab.SetGameObject(clone);
            Mk1Speedprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.Lubricant,1),
                    new Ingredient(TechType.WiringKit,1)
                }
            })
            .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools","SeaglideTab")
            .WithCraftingTime(5f);
            Mk1Speedprefab.SetUnlock(TechType.Seaglide);
            Mk1Speedprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                Plugin.LiteralSeaglideUpgrades);
            Mk1Speedprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideCustomSpeedUpgrade successfully initalized!");
        }

    }   
}
