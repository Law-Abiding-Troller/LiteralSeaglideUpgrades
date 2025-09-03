using System.Collections.Generic;
using LawAbidingTroller.SeaglideModConcept;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
namespace LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules
{
    public class SeaglideEfficiencyModuleMk3
    {
        public static UpgradeData Mk3Efficiencydata = new UpgradeData(0,0.085f);
        public static CustomPrefab Mk3Efficiencyprefab;
        public static PrefabInfo Info;
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            Info = PrefabInfo.WithTechType("SeaglideEfficiencyUpgradeMk3", "Seaglide Efficiency Upgrade Module Mk 3", "Mk 3 Efficiency Upgrade Module for the Seaglide. 1.85x normal efficiency. (Lasts 85% Longer)")
                .WithIcon(SpriteManager.Get(TechType.PowerUpgradeModule));
            ModOptions.upgradeValues.Add(Info.TechType, Mk3Efficiencydata);
            Mk3Efficiencyprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, TechType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Mk3Efficiencyprefab.SetGameObject(clone);
            Mk3Efficiencyprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.ComputerChip,1),
                    new Ingredient(TechType.AdvancedWiringKit,1),
                    new Ingredient(SeaglideEfficiencyModuleMk2.Info.TechType,1)
                }
            })
            .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            Mk3Efficiencyprefab.SetUnlock(TechType.Seaglide);
            Mk3Efficiencyprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                Plugin.LiteralSeaglideUpgrades);
            Mk3Efficiencyprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideEfficiencyUpgradeMk3 successfully initalized!");
        }
    }
}
