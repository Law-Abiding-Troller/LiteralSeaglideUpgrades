using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace LawAbidingTroller.SeaglideModConcept;

[Menu("Literal Seaglide Upgrades")]
public class ModOptions : ConfigFile
{
    [Keybind("Open Upgrades Container Key"), OnChange(nameof(KeyBindChangeEvent))]
    public KeyCode OpenUpgradesContainerKey = KeyCode.V;

    public void KeyBindChangeEvent(KeybindChangedEventArgs newbind)
    {
        OpenUpgradesContainerKey = newbind.Value;
    }
    [Toggle("Enable Extra Upgrades? (Requires Restart)"), OnChange(nameof(ToggleChangeEventUpgrades))]
    public bool ExtraUpgrades;
    
    public void ToggleChangeEventUpgrades(bool changed)
    {
        ExtraUpgrades = changed;
    }
    [Toggle("Enable Debug Mode? (Requires Restart)"), OnChange(nameof(ToggleChangeEventDebugMode))]
    public bool debugmode;

    public void ToggleChangeEventDebugMode(bool changed)
    {
        debugmode = changed;
    }
}