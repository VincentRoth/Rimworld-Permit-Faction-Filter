# Permit Faction Filter

## What it does

In the Royalty **permits tab** of a pawn, the "switch faction" button opens a menu listing every visible faction. With many modded factions this gets long and mostly useless.

This mod keeps only the factions that **offer permits** (at least one `RoyalTitlePermitDef` whose `faction` is that faction's def). The criterion is the same one vanilla uses to decide whether to show the switch button at all.

No Defs are changed and saves are untouched. Remove the mod at any time.

## Requirements

- **Royalty** DLC
- **Harmony** (`brrainz.harmony`). Load this mod after both.

## How it works

A Harmony transpiler on `RimWorld.PermitsCardUtility.DrawRecordsCard` replaces the call to `FactionManager.AllFactionsVisibleInViewOrder` with a filtered version (`Source/PermitFactionFilter.cs`). If that call is not found (game update), the patch does nothing and logs a warning.

## Building from source

- **1.6**: `Source/PermitFactionFilter.csproj` (`net472`) → `1.6/Assemblies/`
- **1.7**: `Source/1.7/PermitFactionFilter.csproj` (`net10.0`, references `Assembly-CSharp`, `UnityStubs` and Harmony 1.7 at the game root) → `1.7/Assemblies/`. The source file is shared between both projects, each must be built against the matching installed RimWorld.

1. Open it in Visual Studio or Rider (or `dotnet build -c Release`).
2. Edit `<RimWorldPath>` and `<RimWorldModPath>` at the top of the `.csproj` if your paths differ.
3. Build: the `.dll` goes straight to `1.6/Assemblies/`.

## Deploying

`Deploy-Mod.ps1` mirrors the mod (without sources, pdb, git files) to `<RimWorld>\Mods\PermitFactionFilter`. Use `-WhatIf` for a dry run.
