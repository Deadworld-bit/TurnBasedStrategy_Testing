[![Create a Turn Based Strategy Game with Unity | © 2023 by Deadworld ]](https://github.com/Deadworld-bit/TurnBasedStrategy_Testing.git)

Welcome to the **TurnBasedStrategy_Testing** repository! This Unity project showcases a turn-based strategy game demo built with a custom grid system, action point mechanics, and intelligent enemy AI. Explore tactical gameplay with strategic movement, dynamic combat, and an evolving user interface.

# Project Overview

This repository contains a Unity-based turn-based strategy game with the following key features:

- **Turn-Based Action Point System**: Units operate using Action Points (AP) to perform actions like movement, attacks, or special abilities.
- **Custom Grid System**: A flexible grid enables strategic movement and positioning, enhancing tactical depth.
- **Pathfinding Algorithm**: A robust system accounts for obstacles, walls, player units, and enemies to calculate optimal movement paths.
- **Enemy AI**: Smart AI evaluates and prioritizes actions based on suitability and available AP during enemy turns.
- **Basic UI**: A functional user interface, with planned updates to improve usability and aesthetics.
- **Visual Assets**: Models and assets from the Unity Asset Store enhance the game's visual experience.

---

# Key Features

## Turn-Based Action Point System
- **Action Points (AP)**: Each unit has a set amount of AP per turn to spend on actions.
- **Action Costs**:
  - **Movement**: AP cost varies based on distance traveled on the grid.
  - **Attacks**: Different weapons or skills have unique AP costs.
  - **Abilities**: Powerful abilities may require more AP or multiple turns to execute.

## Pathfinding Algorithm
- The game features a custom pathfinding system that ensures efficient navigation across the grid.
- Accounts for obstacles, walls, player units, and enemies to calculate viable paths.
- Used by both player units and enemies for coordinated and strategic movement.

![Pathfinding Diagram](https://github.com/Deadworld-bit/TurnBasedStrategy_Testing/blob/main/Pictures/Pathfinding.drawio.png)

## Enemy AI
- Activates during the enemy’s turn, responding dynamically to the game state.
- Evaluates available actions based on AP and calculates a "suitability score" for each.
- Prioritizes and executes the action with the highest suitability, ensuring challenging and intelligent opponents.

## Planned Updates
- **Multi-Floor Support**: Introduce multi-level environments for expanded tactical gameplay.
- **UI Enhancements**: Improve the user interface for better functionality and a polished experience.
- **Additional Features**: Stay tuned for more updates to enrich gameplay and visuals.

---

# Screenshots

Explore the demo’s visuals through these screenshots:

![Screenshot 1](https://github.com/Deadworld-bit/TurnBasedStrategy_Testing/blob/main/Pictures/Pic_01.png)  
![Screenshot 2](https://github.com/Deadworld-bit/TurnBasedStrategy_Testing/blob/main/Pictures/Pic_02.png)  
![Screenshot 3](https://github.com/Deadworld-bit/TurnBasedStrategy_Testing/blob/main/Pictures/Pic_03.png)  
![Screenshot 4](https://github.com/Deadworld-bit/TurnBasedStrategy_Testing/blob/main/Pictures/Pic_04.png)

# Getting Started
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Deadworld-bit/TurnBasedStrategy_Testing.git
2. **Launch Unity Hub**
3. **Click Add → select the cloned project folder**
4. **Open the project and press Play**

# Contributing
Contributions are welcome! Feel free to submit issues, suggest features, or create pull requests to improve the project.

##### © Deadworld 2023
