using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModuleMk2
    {
        public static UpgradeData Mk2Data = new UpgradeData(5.0f);
        public static CustomPrefab Mk2Speedprefab;
        public static PrefabInfo Info;
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static bool Mk2SpeedModuleInSeaglide;
        public static void Register()
        {

            Info = PrefabInfo.WithTechType("SeaglideSpeedUpgradeMk2", "Seaglide Speed Upgrade Module Mk 2", "Mk 2 Speed Upgrade Module for the Seaglide. 5x normal speed.")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            ModOptions.upgradeValues.Add(Info.TechType, Mk2Data);
            Mk2Speedprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, TechType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Mk2Speedprefab.SetGameObject(clone);
            Mk2Speedprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.Lubricant, 2),
                    new Ingredient(TechType.AdvancedWiringKit,1),
                    new Ingredient(SeaglideSpeedModulePrefab.Info.TechType,1)
                }
            })
            .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools","SeaglideTab")
            .WithCraftingTime(5f);
            Mk2Speedprefab.SetUnlock(TechType.Seaglide);
            Mk2Speedprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                Plugin.LiteralSeaglideUpgrades);
            Mk2Speedprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideSpeedUpgradeMk2 successfully initalized!");
        }

    }
}
