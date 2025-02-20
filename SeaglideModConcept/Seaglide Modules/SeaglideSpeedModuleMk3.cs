using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModuleMk3
    {
        public static float mk3speedmultiplier = 8.0f;
        public static CustomPrefab mk3speedprefab;
        public static PrefabInfo Info;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static bool mk3SpeedModuleInSeaglide;
        public static void Register()
        {

            Info = PrefabInfo.WithTechType("SeaglideSpeedUpgradeMk3", "Seaglide Speed Upgrade Module Mk 3", "Mk 5 Speed Upgrade Module for the Seaglide. 8x normal speed. Only so fast you can make a Seaglide")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            mk3speedprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, techType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            mk3speedprefab.SetGameObject(clone);
            mk3speedprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.Lubricant, 1),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit, 1),
                    new CraftData.Ingredient(TechType.Battery, 1),
                    new CraftData.Ingredient(SeaglideSpeedModuleMk2.Info.TechType, 1)
                }
            })
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Machines")
            .WithCraftingTime(5f);
            mk3speedprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideSpeedUpgradeMk2 successfully initalized!");
        }

    }
}