# Ionfall

[![Godot Engine](https://img.shields.io/badge/Godot-4.4%2B-478CBF?style=for-the-badge&logo=godot-engine&logoColor=white)](https://godotengine.org/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Space](https://img.shields.io/badge/Genre-Space-blue?style=for-the-badge)](#)

**Ionfall** is a 2D space-themed shooter/platformer built with Godot 4 and C#.
The game mixes tight platforming with ranged combat, epic music, and dark-space level themes.

Status: WIP

---

## Features (Planned / Implemented)
- 2D platforming movement with responsive jump/run
- Mouse-aimed, ammo-based gunplay with adjustable fire rate
- Aim-linked player animations (cursor-driven facing)
- Interface-driven architecture (e.g., `IControllable`, `ISpawnable`, `IHittable`)
- Handcrafted level design
- Clean C# project structure for fast iteration

---

## Project Structure
    Ionfall/
      Assets/         Sprites, audio, fonts (not provided in repo)
      Scenes/         Godot scenes (levels, entities, UI)
      Scripts/        Game logic (C#)
        Entities/     Player, enemies, projectiles
        Objects/      Platforms, pickups, hazards
        UI/           HUD, menus
        Interfaces/   IControllable, ISpawnable, IHittable, etc.
      README.md

---

## Controls (Default)
- Move: A / D
- Jump: Space
- Crouch: C
- Aim: Mouse
- Fire: Left Click

---

## Development
- Engine: Godot 4.4+
- Language: C#
- Scaffold: new-godot-project

### Requirements
- Godot 4.4 or later (Mono build)
- .NET SDK 8.0+ (for C# builds)

### Running
    $ git clone https://github.com/BenEklundCS/Ionfall.git
    $ cd Ionfall
Open the project in Godot 4.4+ (Mono) and run the main scene.
