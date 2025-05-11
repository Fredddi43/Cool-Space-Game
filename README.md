# Cool Space Game Prototype

A minimal 2D arcade-style shooting game built in Unity as a technical exercise. The player pilots a spaceship, destroys incoming enemies, collects power-ups, and aims for the highest score before collision.

---

## Table of Contents

- [Cool Space Game Prototype](#cool-space-game-prototype)
  - [Table of Contents](#table-of-contents)
  - [Project Overview](#project-overview)
  - [Gameplay Mechanics](#gameplay-mechanics)
  - [Technical Implementation](#technical-implementation)
  - [Art \& Audio](#art--audio)
  - [Controls](#controls)
  - [Installation \& Running](#installation--running)
  - [What I Learned](#what-i-learned)
  - [Future Improvements](#future-improvements)
  - [Disclaimer](#disclaimer)
  - [Asset Sources](#asset-sources)

---

## Project Overview

This project was built to specification as a skill check for a hiring process and remains in prototyping stage. The goal was to demonstrate core game-loop mechanics, entity management, simple particle effects, audio integration, and a rudimentary upgrade system.

---

## Gameplay Mechanics

- **Player Ship**  
  - Circular ship moves freely within screen bounds via WASD or arrow keys.  
- **Enemies** (spawn at random Y on right edge)  
  1. **Red Square**: constant speed left.  
  2. **Yellow Square**: double speed left.  
  3. **Asteroid**: splits into two smaller asteroids (±45°) when shot; small fragments are destroyed by one more hit.  
- **Weapons** (press SPACE to fire)  
  - Single Shot: one projectile straight right.  
  - Split Shot: two projectiles at ±30°.  
  - Triple Shot: single + split combined.  
- **Scoring & Drops**  
  - +100 points per enemy destroyed.  
  - 20% chance per kill to drop one of three items (uniform chance):  
    - **Points**: +50 score instantly.  
    - **Weapon Upgrade**: advances weapon level (max 3).  
    - **Shield Upgrade**: 5 s invincibility.  
- **Game Over**  
  - Occurs on collision with an enemy when not invincible.  
  - Final score overlay appears; press SPACE to restart.  
- **UI**  
  - Score displayed at top-right.

---

## Technical Implementation

- **Engine & Language**  
  - Unity 6, C#  
- **Core Systems**  
  - Fixed-timestep game loop with separate Update and Render phases.  
  - Input handling via Unity’s Input System mapping keyboard to movement and firing.  
  - Entity management using object pooling for enemies, projectiles, and drops.  
  - Collision detection using CircleCollider2D and BoxCollider2D.  
  - State machine for weapon levels and shield timer.  
  - Randomized spawn intervals and drop probability via `Random.Range`.

---

## Art & Audio

- Visual effects: simple sprite animations, explosion particles, camera shake on hit.  
- Sound effects: laser fire, explosions, item pickup, using free assets.  
- All visual and audio assets are either created by me or sourced under free licenses.

---

## Controls

| Action         | Key(s)         |
| -------------- | -------------- |
| Move Up        | W or ↑ Arrow   |
| Move Down      | S or ↓ Arrow   |
| Move Left      | A or ← Arrow   |
| Move Right     | D or → Arrow   |
| Fire / Restart | SPACE          |

---

## Installation & Running

1. **Clone**  
   ```bash
   git clone https://github.com/Fredddi43/Cool-Space-Game.git
   cd cool-space-game
   ```
2. **Open in Unity**  
   - Launch Unity Hub, add the cloned folder as a project, and open with Unity 6.  
3. **Play**  
   - In the Unity Editor, click Play.  
   - To build a standalone executable, use **File → Build Settings** and select target platform.

---

## What I Learned

- Implemented a clean **game loop** with decoupled update/render phases in Unity.  
- Managed **dynamic entities** and object pooling for performance.  
- Created an **upgradeable weapon** system and time-limited shield.  
- Balanced **spawn rates** and drop probabilities through playtesting.  
- Integrated **audio** and basic **particle systems** for feedback.  
- Structured project folders and scripts for a rapid prototype.

---

## Future Improvements

- Dynamic difficulty scaling (increasing spawn rate over time).  
- Persistent high-score leaderboard using PlayerPrefs or server backend.  
- Additional enemy behaviors (e.g. homing missiles, zig-zag movement).  
- Polished UI with menus and pause functionality.  
- Mobile/touch input support and on-screen controls.

---

## Disclaimer

All rights reserved by author. No permission is granted to copy, modify, distribute, or otherwise use this repository or its contents beyond personal review and learning. This code is provided “as-is” for portfolio demonstration only.

---

## Asset Sources

- **Sounds** by Zapsplat:  
  - Science Fiction Spacecraft Fighter Zoom by Ultra Fast 1  
    https://www.zapsplat.com/music/science-fiction-spacecraft-fighter-zoom-by-ultra-fast-1/  
  - Additional sounds from Zapsplat free library.  
- **Background** by Enji:  
  https://enjl.itch.io/background-starry-space  
- **Background art** generated with ChatGPT (DALL·E).  
- **Sprites** from CraftPix free collections:  
  - Pixel-Art Enemy Spaceship 2D Sprites  
    https://craftpix.net/freebies/free-pixel-art-enemy-spaceship-2d-sprites/  
  - Free Spaceship Pixel Art Sprite Sheets  
    https://craftpix.net/freebies/free-spaceship-pixel-art-sprite-sheets/  
  - Free Space Shooter Game Objects  
    https://craftpix.net/freebies/free-space-shooter-game-objects/  
```