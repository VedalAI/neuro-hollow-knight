# <img src="Assets/icon.png" width="29" style="vertical-align:middle;"> Hollow Knight Neuro Integration

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - If on Windows, download and install the .NET SDK x64 for Windows.
- [Hollow Knight](https://store.steampowered.com/app/367520/Hollow_Knight/)

## Installation

1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx) (more comfortable using it than the HK-specific mod loader).
2. Copy `Assembly-CSharp.dll`, `PlayMaker.dll`, `UnityEngine.UI.dll` from your Hollow Knight installation to `./lib`.
3. Run `dotnet build` from the root of this repository.
4. Copy `./bin/Debug/netstandard2.1/FSharp.Core.dll` to `Hollow Knight/BepInEx/core`.
5. Copy `./bin/Debug/netstandard2.1/HollowNeuro.dll` to `Hollow Knight/BepInEx/plugins`.

Cheat menu: hold home+end for 5 seconds, then toggle with home.

Debug tools: press F10 to save player data to hero.json or F5 to load it from hero.json (can be used for creating arbitrary save files or comparing data after certain milestones or actions) - note that it wont change the active scene or any game objects and also not sure if it will work on windows due to game folder permissions.

Profiler (not included in this repo): F1 to reset/F2 to save trace.
