using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;
using LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab;

namespace LawAbidingTroller.SeaglideModConcept.SeaglideModules.SpeedPrefab
{
    public class SeaglideSpeedModuleMk4
    {
        
        public static TechType TechType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            
            Plugin.Prefabinfo[Plugin.Index] = PrefabInfo.WithTechType($"SeaglideSpeedUpgradeMk{GetSpeed()}", $"Seaglide Speed Upgrade Module Mk {GetSpeed()}", $"Mk {GetSpeed()} Speed Upgrade Module for the Seaglide. {Plugin.Speedmultiplier[Plugin.Index]}x normal speed.")
                .WithIcon(SpriteManager.Get(TechType.Seaglide));
            Plugin.Speedprefab[Plugin.Index] = new CustomPrefab(Plugin.Prefabinfo[Plugin.Index]);
            var clone = new CloneTemplate(Plugin.Prefabinfo[Plugin.Index], TechType);
            clone.ModifyPrefab += obj =>
            {
                var model = obj.gameObject;
                model.transform.localScale = Vector3.one / 0002;
            };
            Plugin.Speedprefab[Plugin.Index].SetGameObject(clone);
            
                Plugin.Speedprefab[Plugin.Index].SetRecipe(new Nautilus.Crafting.RecipeData()
                    {
                        craftAmount = 1,
                        Ingredients = new List<CraftData.Ingredient>()
                        {
                            new CraftData.Ingredient(TechType.Lubricant, 3),
                            new CraftData.Ingredient(TechType.AdvancedWiringKit, 2),
                            new CraftData.Ingredient(TechType.Battery),
                            new CraftData.Ingredient(GetCurrentModule())
                        }
                    })
                    .WithFabricatorType(UpgradesLIB.Items.Equipment.Handheldprefab.HandheldfabTreeType)
                    .WithStepsToFabricatorTab("Tools", "SeaglideTab", "ExtraUpgrades")
                    .WithCraftingTime(5f);
                Plugin.Speedprefab[Plugin.Index].SetUnlock(TechType.Seaglide);
                Plugin.Speedprefab[Plugin.Index].SetPdaGroupCategory(UpgradesLIB.Plugin.equipmentupgrademodules,
                    Plugin.LiteralSeaglideUpgrades);
                Plugin.Speedprefab[Plugin.Index].Register();
            

            Plugin.Logger.LogInfo($"Prefab SeaglideSpeedUpgradeMk{GetSpeed()} successfully initalized!");
        }
        private static TechType GetCurrentModule()
        {
            if (Plugin.Index == 0)
            {
                return TechType.WiringKit;
            }
            return Plugin.Prefabinfo[Plugin.Index - 1].TechType;
        }
        private static float GetSpeed()
        {
            if (Plugin.Changespeedmultiplier == 20.0f)
            {
                return 13;
            }

            if (Plugin.Changespeedmultiplier == 4f)
            {
                return 4;
            }
            return Plugin.Changespeedmultiplier;
        }
    }
}