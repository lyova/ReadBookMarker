# Read Book Marker

A client-side modlet for **7 Days to Die V 3.2.0** that puts a green check mark on books you have
already read and on skill magazines whose skill is already maxed out.

No more picking up a stack of magazines to find out none of them do anything.

## What it does

A small check mark appears in the top right corner of the item slot when the item is spent for
you. It shows everywhere items are drawn: inventory, containers, the loot window, the crafting
screen, the trader's stock list - so you can see a magazine is useless before paying for it - and
the reward picker when a quest is turned in. The item info panel shows it on the large preview
too.

Three cases:

- **Books** - the one-shot ones that unlock a perk. Marked once read.
- **Skill magazines** - marked when the crafting skill they feed has reached its cap.
- **Schematics** - marked once the recipe they teach is known.

## How it decides

All three carry an `Unlocks` property, but it points at two different systems.

Books and magazines name a progression entry, and every entry has its own maximum: 1 for a book,
50, 75 or 100 depending on the crafting skill. The mod marks the item when that progression sits
at its cap, so magazines stop being marked exactly when they stop being useful.

Schematics name a recipe instead, so the mod asks the game whether that recipe is already known.

Nothing is hardcoded - books, magazines and schematics added by other mods work too, as long as
they follow the vanilla pattern.

## Requirements

- 7 Days to Die **V 3.2.0**
- Launch **without EasyAntiCheat** - the mod ships a DLL, and EAC blocks those.
- Client side only: no server install, other players do not need it.

## Installation

Extract into your `Mods` folder so you end up with `Mods\ReadBookMarker\ModInfo.xml`, then start
the game without EAC.

## Compatibility

Adds one sprite to the item slot template through XPath and answers a custom binding with a
Harmony postfix, so no vanilla file is overwritten. UI overhauls that replace the item slot
template outright would drop the marker - everything else is fine.

## Building

Needs the .NET SDK 8.0 or newer. Adjust `GameDir` in `Directory.Build.props` if your game lives
elsewhere.

```powershell
.\build.ps1
```

## License

MIT - see [LICENSE](LICENSE).
