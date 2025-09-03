using System;
using System.Collections.Generic;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept;

[Menu("Literal Seaglide Upgrades")]
public class ModOptions : ConfigFile
{
    public static Dictionary<TechType, UpgradeData> upgradeValues = new Dictionary<TechType, UpgradeData>();
    
    
    //[Keybind("Open Upgrades Container Key"), OnChange(nameof(KeyBindChangeEvent))]
    public KeyCode OpenUpgradesContainerKey = KeyCode.V;
    public static KeyCode OpenUpgradesContainerkey = KeyCode.V;

    void KeyBindChangeEvent(KeybindChangedEventArgs args)
    {
        OpenUpgradesContainerkey = args.Value;
    }
    
    [Choice("Extra Speed Upgrades? (None-Mk13)", "None", "Mk 4", "Mk 5", "Mk 6", "Mk 7", "Mk 8", "Mk 9", "Mk 10", "Mk 11", "Mk 12", "Mk 13"),OnChange(nameof(ChoiceChangeEvent))]
    public int ExtraSpeedUpgrades;

    void ChoiceChangeEvent(ChoiceChangedEventArgs<int> args)
    {
        switch (args.Value)
        {
            case 0:
                KnownTech.Remove(Plugin.Prefabinfo[0].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[1].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[2].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[3].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 1:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[1].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[2].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[3].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 2:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[2].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[3].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 3:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[3].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 4:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 5:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Add(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 6:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Add(Plugin.Prefabinfo[4].TechType);
                KnownTech.Add(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 7:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Add(Plugin.Prefabinfo[4].TechType);
                KnownTech.Add(Plugin.Prefabinfo[5].TechType);
                KnownTech.Add(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 8:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Add(Plugin.Prefabinfo[4].TechType);
                KnownTech.Add(Plugin.Prefabinfo[5].TechType);
                KnownTech.Add(Plugin.Prefabinfo[6].TechType);
                KnownTech.Add(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 9:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Add(Plugin.Prefabinfo[4].TechType);
                KnownTech.Add(Plugin.Prefabinfo[5].TechType);
                KnownTech.Add(Plugin.Prefabinfo[6].TechType);
                KnownTech.Add(Plugin.Prefabinfo[7].TechType);
                KnownTech.Add(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
            case 10:
                KnownTech.Add(Plugin.Prefabinfo[0].TechType);
                KnownTech.Add(Plugin.Prefabinfo[1].TechType);
                KnownTech.Add(Plugin.Prefabinfo[2].TechType);
                KnownTech.Add(Plugin.Prefabinfo[3].TechType);
                KnownTech.Add(Plugin.Prefabinfo[4].TechType);
                KnownTech.Add(Plugin.Prefabinfo[5].TechType);
                KnownTech.Add(Plugin.Prefabinfo[6].TechType);
                KnownTech.Add(Plugin.Prefabinfo[7].TechType);
                KnownTech.Add(Plugin.Prefabinfo[8].TechType);
                KnownTech.Add(Plugin.Prefabinfo[9].TechType);
                break;
            default:
                KnownTech.Remove(Plugin.Prefabinfo[0].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[1].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[2].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[3].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[4].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[5].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[6].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[7].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[8].TechType);
                KnownTech.Remove(Plugin.Prefabinfo[9].TechType);
                break;
        }
    }
}