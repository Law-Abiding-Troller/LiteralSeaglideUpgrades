# LiteralSeaglideUpgrades
My Literal Seaglide Upgrades Subnautica mod. Requires Nautilus.

## Dependencies
 - Subnautica
 - BepInEx
 - Nautilus
 - UpgradesLIB

## How to install:

1. Ensure you have BepInEx and Nautilus installed, if you do not, the mod will not work.
2. Find the right of this repository. There should be sections labeled `About`, `Releases`, `Packages`, and `Languages`. Find `Releases`, just under the about section
3. Click on the latest release.
4. Download the ZIP file (should currently be labeled LiteralSeaglideUpgradesv0.2.5.zip)
5. If you have Vortex, follow the Vortex Installation just after number 7.
6. If you have Subnautica Mod Manager, skip the Vortex Installation and go to Subnautica Mod Manager Installation (Not QModManager)
7. If you have neither, skip both the Vortex Installation and the Subnautica Mod Manager Installation and go straight to Manual Installation.
### VORTEX INSTALLATION
#### WARNING: UNTESTED. MAY BREAK
1. Open Vortex
2. Find the file you just downloaded
3. Click the `Mods` tab
4. Drag the folder you just found into the `Drop Files` area next to `Find More` near the bottom of Vortex
5. Installed
### SUBNAUTICA MOD MANAGER INSTALLATION (NOT QMODMANAGER)
1. Run Subnautica
2. Find the file you just downloaded
3. Click the `Mod Manager` button in the main menu
4. Click the `Install` tab near the top of the new menu
5. Click the `Open mods download foler (Put downloaded zip files here)` button.
6. A new window should open. Drag the file you just found into that new window.
7. Click `Install all mods`.
8. If it asks you to restart, do restart.
9. Done
### MANUAL INSTALLATION
1. Find the file you just downloaded
2. Extract(Unzip) the folder to anywhere
3. Find your Subnautica folder, then double click on it to open it.
4. Find the `BepInEx` folder at the top of your Subnautica folder
5. Find the `plugins` folder at the bottom of the list of files in the BepInEx folder. Also above `LogOutput.log`. Open it
6. Drag the Extracted(Unziped) folder that you just Extracted(Unziped) into the folder you just opened\
7. Done. Can Launch Subnautica now.

## Notes:
### READ BEFORE PLAYING
 - There are now mod options since v0.2.5
 - How to find these:
 - 1. Run Subnautica
   2. Find and Click `Options`
   3. Find and Click `Mods`
   4. Find `Literal Seaglide Upgrades` drop down
   5. Read over the settings, and make sure you want them the way they are
### What Each Setting is:
 - Open Upgrades Container Key: It sets a keybind which you want to open the upgrades container while holding the seaglide!
 - Extra Speed Upgrades? (None-Mk13): Allows you to choose which extra speed upgrade you want to have as your maximum. You can have the base Mk 3, all the way to Mk 13. Lowering the setting (say for example from mk 13 to mk 4) after you already have crafted the upgrades (say, mk 13) will not remove the upgrades. Just the ability to craft those upgrades. The upgrades you have will still work in the Seaglide.
 - Enable Debug Mode? (Requires Restart): Tick to have a check mark which will enable debug mode. It will require a restart to fully enable debug mode. When sending me issues and bugs of this mod, enable debug mode before restarting the game, then recreate it. Its un-checked version will keep your log file uncluttered for other modders to read.  
## Known Bugs: 

None! Please inform me of any you find!

## Update News:
Fixing many bugs and rewriting the full mod. It should be ready by August. However, that depends on how much I can get done, it may release sooner.

Discord User: Law Abiding Coder. I am in the Subnautica Moding discord server. DM or Ping me there.
# Changelog
## v0.1.0 (basically what this mod added to the game)

- Added Logging for debugging
- Added Literal Seaglide Upgrades
- Added storage for the Seaglide
- Added the ablility to press V when holding a Seaglide to access the storages
- Added Seaglide Speed Upgrade Mk 1
- Added Seaglide Speed Upgrade Mk 2
- Added Seaglide Speed Upgrade Mk 3
## v0.1.1
- Changed a variable to remove a counter from the log
## v0.1.2
- Removed a few lines of code to allow anything inside the container
- Added a few lines of code to allow anything inside the container
## v0.1.3
- Actually fixed the bug with adding upgrades to the container
- Atempted to make quickly switching between Seaglides less clunky
## v0.1.4
- Actually made quickly switching between Seaglides less clunky
- Changed how it checks for upgrades
- Rearanged some code
- Added some code
## v0.2.0
- Added Seaglide Effiecency Upgrade Mk 1
- Added Seaglide Effiecency Upgrade Mk 2
- Added Seaglide Effiecency Upgrade Mk 3
- Changed the tool tip for All speed upgrades to actually say their upgrade. (from "Mk 5" to "Mk 1", "Mk 2", and "Mk 3)
- Fixed the Upgrade Modules not unlocking in survival. Now unlock with `Seaglide`
- Removed the Seaglide from the `Deployables` tab in the `Fabricator`
- Removed All speed upgrades from the `Deployables` tab in the `Fabricator`
- Added a new tab: `Seaglide` to the `Fabricator` at: `Personal`, `Tools`
- Added the ability to craft the Seaglide in the tab: `Seaglide` in the `Fabricator`. Steps to get to tab, `Seaglide`: `Personal`, `Tools`
- Added the ability to craft All of the Speed Upgrades in the tab: `Seaglide` in the `Fabricator`. Steps to get to tab, `Seaglide`: `Personal`, `Tools`
- Added the ability to craft All of the Efficiency Upgrades in the tab: `Seaglide` in the `Fabricator`. Steps to get to tab, `Seaglide`: `Personal`, `Tools`
- Updated Source Code
## v0.2.5
 - Added the ability to change the upgrades container key
 - Added mod options
 - Added the ability to enable Debug Mode without recompiling the mod
 - Added a speed addon to the base mod
   - The addon adds 10 additional speed upgrades. Going up to the max speed multiplier of 100x
   - To unlock these upgrades, it is in the mod options menu mentioned in the Notes of this page
   - Does require the Seaglide to unlock
 - Updated source code
 - Squashed many bugs
 - Actually released on Nexus
### More to come!
