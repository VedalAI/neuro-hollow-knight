# <img src="Assets/icon.png" width="29" style="vertical-align:middle;"> Hollow Knight Neuro Integration

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - If on Windows, download and install the .NET SDK x64 for Windows
- [Hollow Knight](https://store.steampowered.com/app/367520/Hollow_Knight/)
- Neuro Agent running locally (e.g. [neuro-api-tony](https://github.com/Pasu4/neuro-api-tony))
- Environment variable `NEURO_SDK_WS_URL` pointing to the Neuro Agent (example value `ws://localhost:8000`)

## Installation

1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx) (more comfortable using it than the HK-specific mod loader).
2. Copy `Assembly-CSharp.dll`, `PlayMaker.dll`, `UnityEngine.UI.dll` from your Hollow Knight installation to `./lib`.
3. Run `dotnet build` from the root of this repository.
4. Copy `./bin/Debug/netstandard2.1/FSharp.Core.dll` to `Hollow Knight/BepInEx/core`.
5. Copy `./bin/Debug/netstandard2.1/HollowNeuro.dll` to `Hollow Knight/BepInEx/plugins`.

Profiler (not included in this repo): F1 to reset/F2 to save trace.

## Disclaimer

Hollow Knight is owned by Team Cherry. This project is unaffiliated with Team Cherry and does not distribute or include any Team Cherry assets.
