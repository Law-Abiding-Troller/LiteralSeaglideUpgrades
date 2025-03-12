using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModuleMk2
    {
        public static float mk2speedmultiplier = 5.0f;
        public static CustomPrefab mk2speedprefab;
        public static PrefabInfo Info;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static bool mk2SpeedModuleInSeaglide;
        public static void Register()
        {

            Info = PrefabInfo.WithTechType("SeaglideSpeedUpgradeMk2", "Seaglide Speed Upgrade Module Mk 2", "Mk 2 Speed Upgrade Module for the Seaglide. 5x normal speed.")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            mk2speedprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, techType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            mk2speedprefab.SetGameObject(clone);
            mk2speedprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.Lubricant, 2),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit, 1),
                    new CraftData.Ingredient(SeaglideSpeedModulePrefab.Info.TechType, 1)
                }
            })
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal","Tools","SeaglideTab")
            .WithCraftingTime(5f);
            mk2speedprefab.SetUnlock(TechType.Seaglide);
            mk2speedprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideSpeedUpgradeMk2 successfully initalized!");
        }

    }
}
