# <img src="Assets/icon.png" width="29" style="vertical-align:middle;"> Hollow Knight Neuro Integration

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - If on Windows, download and install the .NET SDK x64 for Windows
- [Hollow Knight](https://store.steampowered.com/app/367520/Hollow_Knight/)
- Neuro Agent running locally (e.g. [neuro-api-tony](https://github.com/Pasu4/neuro-api-tony))
- Environment variable `NEURO_SDK_WS_URL` pointing to the Neuro Agent (example value `ws://localhost:8000`)

## Installation

1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx) (more comfortable using it than the HK-specific mod loader).
2. Copy `dll` files from your Hollow Knight installation to `./lib`.
3. Run `dotnet build` from the root of this repository.
4. Copy `./bin/Debug/netstandard2.1/FSharp.Core.dll` to `Hollow Knight/BepInEx/core`.
5. Copy `./bin/Debug/netstandard2.1/HollowNeuro.dll` to `Hollow Knight/BepInEx/plugins`.

Extra tools that were used in the development are included [in a separate repo](https://github.com/chayleaf/neuro-hollow-night-integration-tools),
however they are unlikely to be usable as-is and should be primarily used for reference.

For pathfinding, data from [Hollow Knight Randomizer](https://github.com/homothetyhk/RandomizerMod/) has been integrated -
it allowed (mostly) accurately determining room transition reachability.

## Disclaimer

Hollow Knight is owned by Team Cherry. This project is unaffiliated with Team Cherry and does not distribute or include any Team Cherry assets.
