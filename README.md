## Installation

1. install [BepInEx 5](https://github.com/BepInEx/BepInEx) (more comfortable using it than the HK-specific mod loader)
2. copy `Assembly-CSharp.dll`, `PlayMaker.dll`, `UnityEngine.UI.dll` to `lib`
3. `dotnet build`
4. copy `bin/Debug/netstandard2.1/FSharp.Core.dll` to `BepInEx/core`
5. copy `bin/Debug/netstandard2.1/HollowNeuro.dll` to `BepInEx/plugins`

cheat menu: hold home+end for 5 seconds, then toggle with home

debug tools: press f10 to save player data to hero.json or f5 to load it from hero.json (can be used for creating arbitrary save files or comparing data after certain milestones or actions)
(note that it wont change the active scene or any game objects)
(also not sure if it will work on windows due to game folder permissions)

profiler (not included in this repo): f1 to reset/f2 to save trace
