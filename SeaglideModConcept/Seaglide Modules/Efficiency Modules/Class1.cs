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
    public class SeaglideEfficiencyModuleMk1
    {
        public static float mk1efficiencymultiplier = 0.025f;
        public static CustomPrefab mk1efficiencyprefab;
        public static PrefabInfo Info;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            Info = PrefabInfo.WithTechType("SeaglideEfficiencyUpgradeMk1", "Seaglide Efficiency Upgrade Module Mk 1", "Mk 1 Efficiency Upgrade Module for the Seaglide. 1.25x normal efficiency. (Lasts 25% Longer)")
                .WithIcon(SpriteManager.Get(TechType.PowerUpgradeModule));
            mk1efficiencyprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, techType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            mk1efficiencyprefab.SetGameObject(clone);
            mk1efficiencyprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.Battery, 1),
                    new CraftData.Ingredient(TechType.WiringKit, 1)
                }
            })
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal", "Tools", "SeaglideTab")
            .WithCraftingTime(5f);
            mk1efficiencyprefab.SetUnlock(TechType.Seaglide);
            mk1efficiencyprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideEfficiencyUpgradeMk1 successfully initalized!");
        }

    }
}

