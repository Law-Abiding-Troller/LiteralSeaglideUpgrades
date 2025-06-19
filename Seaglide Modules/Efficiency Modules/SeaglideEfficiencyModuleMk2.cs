using System.Collections.Generic;
using LawAbidingTroller.SeaglideModConcept;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;

namespace LawAbidingTroller.LiteralSeaglideUpgrades
{
    public class SeaglideEfficiencyModuleMk2
    {
        public static UpgradeData Mk2Efficiencydata = new UpgradeData(0,0.05f);
        public static CustomPrefab Mk2Efficiencyprefab;
        public static PrefabInfo Info;
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            Info = PrefabInfo.WithTechType("SeaglideEfficiencyUpgradeMk2", "Seaglide Efficiency Upgrade Module Mk 2", "Mk 2 Efficiency Upgrade Module for the Seaglide. 1.5x normal efficiency. (Lasts 50% Longer)")
                .WithIcon(SpriteManager.Get(TechType.PowerUpgradeModule));
            ModOptions.upgradeValues.Add(Info.TechType, Mk2Efficiencydata);
            Mk2Efficiencyprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, TechType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Mk2Efficiencyprefab.SetGameObject(clone);
            Mk2Efficiencyprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.ComputerChip),
                    new CraftData.Ingredient(TechType.WiringKit),
                    new CraftData.Ingredient(SeaglideEfficiencyModuleMk1.Info.TechType)
                }
            })
            .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            Mk2Efficiencyprefab.SetUnlock(TechType.Seaglide);
            Mk2Efficiencyprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                Plugin.LiteralSeaglideUpgrades);
            Mk2Efficiencyprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideEfficiencyUpgradeMk2 successfully initalized!");
        }
    }
}
