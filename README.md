# NoitaSeedChanger
This tool forces the game 'Noita' to use any seed you want.



**Since the latest Noita update, changing seeds with NSC only work if you restart Noita completely. The code have to be rewritten to work properly with the latest versions. Since there are much simpler alternatives, which work without updating an external tool, NSC can be considered as abandoned. Here is an Alternative to change Seeds for future use: https://modworkshop.net/mod/25898**



**DOWNLOAD:** [https://github.com/RNG42/NoitaSeedChanger/releases/latest](https://github.com/RNG42/NoitaSeedChanger/releases/latest)

Compatible with the latest Build ( May 20 2020) - STEAM

Beta versions are no longer supported, due to the frequency of updates. You are still able to add the needed data yourself if you want. See the 'Compatibility' section below.

**Recent changes:**

* Added XML reader
* Outsourced release relevant data such as version hashes and memory targets to a XML file
* removed outdated seeds (more will be added soon)
* Fixed some other issues

**Installation:**

Unzip the files to a separate folder and start NoitaSeedChanger.exe. The files can be saved anywhere you like and do not need to be copied into Noita's installation folder.

**Usage:**

Type a number from the square brackets to select a seed or enter a new one (1 to 4294967295), let the tool run in the background and start a completely new game. The seed will be changed automatically after the game restarts. If you want to set a different seed, you need to restart NoitaSeedChanger. You can do this easily by pressing CTRL+C while the window is focused.

You can add a seed + corresponding description to the SeedList.txt on a new line by following the format:
SEED:DESCRIPTION - where the seed is seperated from the description by a colon (important!).

**Compatibility:**

Note that only STEAM releases are supported by default. For GoG or itch.io releases you can add compatibility by adding a new Data tag to the ReleaseData.xml including the version hash and memory addresses.

The hash for your current Noita version can be found in the installation folder: '\..\Noita\ _version_hash.txt'

The memory addresses by searching the current seed your game is showing in the main menu with 'CheatEngine' from cheatengine.org

There should be at least two entries with green highlighted memory addresses.
