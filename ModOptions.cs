using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept;

[Menu("Literal Seaglide Upgrades")]
public class ModOptions : ConfigFile
{
    [Keybind("Open Upgrades Container Key")]
    public KeyCode OpenUpgradesContainerKey = KeyCode.V;
    
    
    [Choice("Extra Speed Upgrades? (None-Mk13)", "None", "Mk 4", "Mk 5", "Mk 6", "Mk 7", "Mk 8", "Mk 9", "Mk 10", "Mk 11", "Mk 12", "Mk 13")]
    public int ExtraSpeedUpgrades;
    
    [Toggle("Enable Debug Mode? (Requires Restart)")]
    public bool debugmode;

    
}