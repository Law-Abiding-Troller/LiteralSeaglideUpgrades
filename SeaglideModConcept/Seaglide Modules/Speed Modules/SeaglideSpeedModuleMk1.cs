using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModulePrefab
    {
        public static float mk1speedmultiplier = 2.0f;
        public static CustomPrefab mk1speedprefab;
        public static PrefabInfo Info;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        { 
            Info = PrefabInfo.WithTechType("SeaglideSpeedUpgradeMk1", "Seaglide Speed Upgrade Module Mk 1", "Mk 1 Speed Upgrade Module for the Seaglide. 2x normal speed.")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            mk1speedprefab = new CustomPrefab(Info);
            var clone = new CloneTemplate(Info, techType);
            //so wat ur saying is: modify clone
            clone.ModifyPrefab += obj =>
            {
                GameObject model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            mk1speedprefab.SetGameObject(clone);
            mk1speedprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.Lubricant, 1),
                    new CraftData.Ingredient(TechType.WiringKit, 1)
                }
            })
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal","Tools","SeaglideTab")
            .WithCraftingTime(5f);
            mk1speedprefab.SetUnlock(TechType.Seaglide);
            mk1speedprefab.Register();

            Plugin.Logger.LogInfo("Prefab SeaglideCustomSpeedUpgrade successfully initalized!");
        }

    }   
}
