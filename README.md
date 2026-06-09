Doomfall
A first-person shooter built in Unity, inspired by classic arena shooters. Fight through waves of enemies, unlock weapons, and take down a boss.

Gameplay

First-person movement and shooting
3 unlockable weapons — each found as pickups in the level
Melee attack for close-range combat
Enemy AI that idles until alerted, then chases and attacks
Arena spawner that continuously spawns enemies up to a cap
Boss fight with health bar UI
Health and armor system — armor absorbs damage first
Death screen on player death


Controls
ActionKey / InputMoveW A S DLookMouseJumpSpaceShootLeft Mouse ButtonMeleeVSwitch Weapon1 2 3

Weapons
WeaponDamageFire RateAmmoPistol252/s30Machine Pistol1210/s100Shotgun15 × 6 pellets1/s50
Weapons are locked by default and must be picked up in the level. Shooting alerts nearby enemies within a configurable radius.

Enemy System

Enemies are idle until the player shoots nearby or gets spotted
Once aggro, they chase and attack using Unity NavMesh
Arena spawner spawns enemies at random positions away from the player
Boss has 300 HP, melee attack, and a dedicated health bar UI


Project Structure
Assets/
├── Scripts/
│   ├── Player/         # Movement, shooting, health, inventory
│   ├── Enemy/          # AI, awareness, spawner, manager
│   ├── Boss/           # Boss logic and UI
│   ├── Item/           # Weapon and item pickups
│   ├── Door/           # Door logic
│   └── UI/             # Canvas/HUD manager
├── Scenes/
│   └── Doomfall.unity
└── Starfield Skybox/   # Skybox assets

Built With

Unity (URP)
Unity Input System
Unity NavMesh
TextMesh Pro


Setup

Clone the repo
Open in Unity 2022.3 or later
Open Assets/Scenes/Doomfall.unity
Press Play
