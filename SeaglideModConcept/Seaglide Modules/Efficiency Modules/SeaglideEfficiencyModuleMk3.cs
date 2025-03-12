using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawAbidingTroller.SeaglideModConcept;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
namespace LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules
{
    public class SeaglideEfficiencyModuleMk3
    {
        public static float mk3efficiencymultiplier = 0.085f;
        public static CustomPrefab mk3efficiencyprefab;
        public static PrefabInfo Info;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            Info = PrefabInfo.WithTechType("SeaglideEfficiencyUpgradeMk3", "Seaglide Efficiency Upgrade Module Mk 3", "Mk 3 Efficiency Upgrade Module for the Seaglide. 1.85x normal efficiency. (Lasts 85% Longer)")
                .WithIcon(SpriteManager.Get(TechType.PowerUpgradeModule));
            mk3efficiencyprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, techType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            mk3efficiencyprefab.SetGameObject(clone);
            mk3efficiencyprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.ComputerChip, 1),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit, 1),
                    new CraftData.Ingredient(SeaglideEfficiencyModuleMk2.Info.TechType, 1)
                }
            })
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal", "Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            mk3efficiencyprefab.SetUnlock(TechType.Seaglide);
            mk3efficiencyprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideEfficiencyUpgradeMk3 successfully initalized!");
        }
    }
}
