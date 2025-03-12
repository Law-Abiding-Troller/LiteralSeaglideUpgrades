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
using LawAbidingTroller.LiteralSeaglideUpgrades.Seaglide_Modules.Efficiency_Modules;

namespace LawAbidingTroller.LiteralSeaglideUpgrades
{
    public class SeaglideEfficiencyModuleMk2
    {
        public static float mk2efficiencymultiplier = 0.05f;
        public static CustomPrefab mk2efficiencyprefab;
        public static PrefabInfo Info;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            Info = PrefabInfo.WithTechType("SeaglideEfficiencyUpgradeMk2", "Seaglide Efficiency Upgrade Module Mk 2", "Mk 2 Efficiency Upgrade Module for the Seaglide. 1.5x normal efficiency. (Lasts 50% Longer)")
                .WithIcon(SpriteManager.Get(TechType.PowerUpgradeModule));
            mk2efficiencyprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, techType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            mk2efficiencyprefab.SetGameObject(clone);
            mk2efficiencyprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.ComputerChip, 1),
                    new CraftData.Ingredient(TechType.WiringKit, 1),
                    new CraftData.Ingredient(SeaglideEfficiencyModuleMk1.Info.TechType, 1)
                }
            })
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal", "Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            mk2efficiencyprefab.SetUnlock(TechType.Seaglide);
            mk2efficiencyprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideEfficiencyUpgradeMk2 successfully initalized!");
        }
    }
}
