using System.Collections.Generic;
using LawAbidingTroller.SeaglideModConcept;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;

namespace LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules
{
    public class SeaglideEfficiencyModuleMk1
    {
        public static UpgradeData Mk1EfficiencyData = new UpgradeData(0,0.025f);
        public static CustomPrefab Mk1Efficiencyprefab;
        public static PrefabInfo Info;
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            Info = PrefabInfo.WithTechType("SeaglideEfficiencyUpgradeMk1", "Seaglide Efficiency Upgrade Module Mk 1", "Mk 1 Efficiency Upgrade Module for the Seaglide. 1.25x normal efficiency. (Lasts 25% Longer)")
                .WithIcon(SpriteManager.Get(TechType.PowerUpgradeModule));
            ModOptions.upgradeValues.Add(Info.TechType, Mk1EfficiencyData);
            Mk1Efficiencyprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, TechType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Mk1Efficiencyprefab.SetGameObject(clone);
            Mk1Efficiencyprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.Battery,1),
                    new Ingredient(TechType.WiringKit,1)
                }
            })
            .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            Mk1Efficiencyprefab.SetUnlock(TechType.Seaglide);
            Mk1Efficiencyprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                Plugin.LiteralSeaglideUpgrades);
            Mk1Efficiencyprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideEfficiencyUpgradeMk1 successfully initalized!");
        }

    }
}

