# Tower_defense_game_PPI
Skeleton apocalypse
Skeleton apocalypse is a dynamic 3D Tower Defense strategy game built in Unity using C#. Players must strategically place defensive structures, manage their economy, and optimize real-time upgrades to defend their core base against scaling waves of enemies.

Features
Dynamic Scaling & Upgrade System: Structural stats (HP, Damage, and Fire Rate) scale exponentially by a multiplier instead of flat addition. Upgrading a structure dynamically updates its 3D mesh colors/materials to reflect its current power level.

Distinct Structural Classes:

Tower: An offensive powerhouse that automatically tracks the nearest targets and fires projectiles with scaling damage.

Wall: A heavy-duty defensive barrier with a massive health pool, designed to block enemy paths and buy precious time.

Robust Game State Management: A fully realized persistent game loop featuring a Main Menu, an interactive settings panel, an in-game pause system that freezes time (via the P key), and clear Victory/Game Over states.

Responsive UI Layout: Built using modern Unity UI (UGUI) anchor presets and a Canvas Scaler configured to scale with screen size. This ensures the layout, menus, and text elements remain perfectly centered and scaled on any monitor resolution, from small windows up to 4K displays.

Persistent Audio Architecture: Features a seamless, uninterrupted loop for the background music (background.mp3) that persists across scene transitions using a MusicManager (implemented via the Singleton pattern with DontDestroyOnLoad). A centralized SoundManager handles global SFX triggers for building, upgrading, selling, and base damage events.

Data Persistence: Utilizes Unity's PlayerPrefs system to seamlessly transfer user configuration data (master volume level and maximum wave count sliders) from the main menu settings straight into the active gameplay scene.

AI Usage Disclosure
The C# codebase architecture, mathematical scaling optimization, global audio persistence managers, and technical documentation structure were developed, refined, and refactored with the assistance of generative AI tools.

Technologies and Tools
Game Engine: Unity 2022.3+ LTS

Language: C#

User Interface: Unity UI (UGUI), TextMeshPro

3D Assets: Unity Asset Store

How to Run the Game
There are two ways to experience the project:

Option 1: Standalone Executable (No Unity Required)
If you just want to play the compiled game directly:

Go to the Releases page of this repository.

Download the XYZ.zip file.

Extract the archive to a folder on your computer.

Important: Ensure that the _Data folder remains in the exact same directory as the .exe file.

Double-click the game's .exe file to launch!

Option 2: Run from Source Code
If you want to view the scripts or modify the project within the editor:

Clone this repository to your local machine.

Open Unity Hub, click Add, and select the cloned project folder.

Launch the project using the correct Unity Editor version.

Inside the Assets/Scenes folder, open the MainMenu scene and press Play.

Controls
LPM (Left Mouse Button): Place structures, select built structures, and navigate UI menus.

PPM (Right Mouse Button): Cancel current build mode or deselect a structure (closes the upgrade panel).

P Key: Toggle the in-game pause overlay and freeze/unfreeze time.
