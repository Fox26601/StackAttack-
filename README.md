Stack Attack Console Game

This project is a C# console game inspired by the classic mobile game Stack Attack. The goal is to avoid getting crushed by falling boxes, clear full rows, and achieve the highest possible score. The player can move left (A), move right (D), jump (W), and push boxes to manage the stacking process.

Features

Player Movement: Move left/right, jump, and push boxes.

Falling Boxes: Boxes fall from the top and stack up over time.

Row Clearing Mechanic: Complete rows disappear, similar to Tetris.

Game Over Detection: The game ends when the player is crushed by a falling box.

High Score System: Stores and displays the top scores.

Input Handling Improvements: Prevents key spamming.

Box Collision Handling: Stops movement at a box until a new input is received.

Controls

A - Move Left

D - Move Right

W - Jump

Q - Restart Game

Project Structure

1. Program.cs

The entry point of the game.

Initializes GameManager and starts the game loop.

2. GameManager.cs

Controls the game loop.

Spawns boxes at regular intervals.

Detects when the player loses the game.

Manages the score system.

3. Player.cs

Manages player movement and actions.

Implements jumping, gravity, and collision detection.

Allows pushing of boxes and prevents movement spam.

4. Box.cs

Manages box properties and movement.

Handles row-clearing mechanics.

Checks for collisions and box stacking.

5. GridManager.cs

Manages the grid layout (20x20 field).

Renders the player and boxes on the screen.

6. HighScoreManager.cs

Saves high scores to a file (highscores.txt).

Loads and displays the top 5 high scores.

Ensures score entries are valid and sorted properly.

How to Run:
Open StartGame.sln
