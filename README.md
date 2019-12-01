# NoitaSeedChanger
This tool forces the game 'Noita' to use any seed you want.

Recent changes:

* Added XML reader
* Outsourced release relevant data such as version hashes and memory targets to a XML file
* Fixed some other issues

**Compatible with Build Nov 29 2019**

Supports **STEAM** releases by default. For GoG or itch.io releases you need to add the needed data to the VersionData.xml.

To add compatibility by yourself you need to add a new 'Data' tag to the VersionData.xml:

The hash for your current Noita version can be found in the installation folder: '..\Noita\ _version_hash.txt'

You can get the memory targets by searching the current seed your game is showing in the main menu with 'CheatEngine'. There should be at least two entries with green highlighted memory addresses.
