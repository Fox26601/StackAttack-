Stack Attack Console Game

Overview

This project is a C# console game inspired by the classic mobile game Stack Attack. The game challenges the player to avoid being crushed by falling boxes, clear full rows, and achieve the highest score possible. The player can move left (A) and right (D), jump (W), and push boxes to manage the stacking process.

Features

Player Movement: Move left/right, jump, and push boxes.

Falling Boxes: Boxes fall from the top and stack up.

Row Clearing Mechanic: Complete rows disappear, similar to Tetris.

Game Over Detection: The game ends when the player is crushed.

High Score System: Stores and displays the top scores.

Key Spam Prevention: Ensures smooth controls by limiting input frequency.

Box Collision Handling: Stops movement at a box, requiring an additional key press to push it.

Controls

A - Move Left

D - Move Right

W - Jump

Q - Restart Game

Game Components

1. Program.cs

Entry point of the game.

Initializes GameManager and starts the game loop.

2. GameManager.cs

Handles the game loop.

Spawns boxes at regular intervals.

Checks for full rows and removes them.

Detects if the player is crushed (game over).

Updates and displays the score.

3. Player.cs

Handles player movement and input.

Implements jump mechanics.

Allows pushing of boxes.

Prevents key spamming.

Stops movement at a box until a new input is received.

4. Box.cs

Represents individual falling boxes.

Stores x and y position.

Allows updating of position.

5. GridManager.cs

Manages the grid layout (20x20 field).

Renders the player and boxes.

6. HighScoreManager.cs

Saves high scores to a file (highscores.txt).

Displays the top 5 high scores.

Ensures score entries are valid and sorted.

How to Run:
Open ConsoleApp2.sln
