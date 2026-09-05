# Read Book Marker

**[Download on Nexus Mods](https://www.nexusmods.com/7daystodie/mods/12279)**

A client-side modlet for **7 Days to Die V 3.2.0** that puts a green check mark on reading material
you no longer need: books you have read, magazines whose skill is capped, schematics you already
know.

No more hauling a stack of magazines back to base to find out none of them do anything.

## What it does

A check mark appears in the corner of the item's icon whenever that item is spent for you. Three
cases, all handled by the same mark:

- **Books** - the one-shot ones that unlock a perk. Marked once read.
- **Skill magazines** - marked when the crafting skill they feed has reached its cap.
- **Schematics** - marked once the recipe they teach is known.

## Where the mark shows

Every place the game draws an item, which took four separate hooks because each list is its own
controller:

| where | what you see |
|-------|--------------|
| Inventory, backpack, containers, loot windows | mark in the top right of the slot |
| Crafting screen and its ingredient slots | same slot mark |
| Trader's stock list | mark on the row icon, so you can tell before paying |
| Quest reward picker | mark on the reward icon, before you choose |
| Item info panel | larger mark on the big preview |

## How it decides

All three kinds carry an `Unlocks` property, but it points at two different systems.

Books and magazines name a progression entry, and every entry has its own maximum: 1 for a book,
50, 75 or 100 depending on the crafting skill. The mod marks the item when that progression sits
at its cap, so a magazine stops being marked exactly when it stops being useful - no hardcoded
"level 100" that would be wrong for half the skills.

Schematics name a recipe instead, so the mod asks the game whether that recipe is already known.
That covers recipes unlocked by perks too, not just by reading the schematic.

Nothing is hardcoded: books, magazines and schematics added by other mods work as well, as long as
they follow the vanilla pattern.

## Requirements

- 7 Days to Die **V 3.2.0**
- Launch **without EasyAntiCheat** - the mod ships a DLL, and EAC blocks those. Start the game
  from `7DaysToDie.exe`, or pick the non-EAC option in the Steam launcher.
- No other mods needed.

## Installation

1. Extract the archive into `<game folder>\Mods\` so you end up with
   `<game folder>\Mods\ReadBookMarker\ModInfo.xml`.
2. Start the game without EAC.

Vortex and the Mod Launcher handle the archive as a normal modlet. To uninstall, delete the
folder - the mod writes nothing to your save.

## Multiplayer

Client side only: the server does not need it and neither do the other players. Everything it
reads - your progression, your known recipes - is already on your client.

Only tested in single player so far. It should behave the same as a client on a dedicated server;
feedback welcome.

## Compatibility

Adds sprites to item templates through XPath and answers a custom binding with Harmony postfixes,
so no vanilla file is overwritten. UI overhauls that only restyle the templates are fine; one that
replaces an item template outright would drop the mark in that particular window while the rest
keep working.

## Building

Needs the .NET SDK 8.0 or newer. Adjust `GameDir` in `Directory.Build.props` if your game lives
elsewhere.

```powershell
.\build.ps1
```

The build deploys the mod into the game's `Mods` folder and packs an archive into `dist\`.

## License

MIT - see [LICENSE](LICENSE).
