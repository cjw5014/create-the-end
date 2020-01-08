# Cargo Offloader

## Setup
1. Create a Programmable block on your grid with a docking connector for a ship AND target cargo container
1. Create an LCD Screen, set it to Text and Images

Optional:
1. Name LCD Screen something useful (LCD Ore Offloader for example)
1. Name target container that will hold your ore
1. Name your source containers (ship cargo holds, drills and connector)

## Configuration
1. Set nameOfShipCargoContainers to the number of containers you want to source and list all their names in the array
1. Set statusDisplayName to the name of the LCD screen you setup, this helps debug too!
1. Set nameOfOreDump to the target container you want ore to go to

## Usage
1. This script does not currently run by itself since it will infrequenly run in my usecase, feel free to modify that!

## Ideas to Improve
* Ensure there is space in the target container before attempting moves
* Allow multiple target containers
* Distribution among target containers
* Specific containers for each ore type
