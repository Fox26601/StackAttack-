class GameManager
{
    private GridManager grid;
    private Player player;
    private List<Box> boxes;
    private int score;
    private Random rand = new Random();
    private int boxSpawnInterval; 

    // Constructor initializes the game components
    public GameManager()
    {
        grid = new GridManager(); // Create the grid
        boxes = new List<Box>(); // Initialize the box list
        player = new Player(grid, boxes, this); // Create the player
        score = 0; // Initialize score
        boxSpawnInterval = 5000; // Set initial box spawn interval
    }

    // Starts the game loop
    public void Start()
    {
        DateTime lastBoxSpawn = DateTime.Now; // Track last box spawn time

        while (true) // Infinite game loop
        {
            player.Update(); // Process player movement and actions
            UpdateBoxes(); // Update box positions
            CheckFullRows(); // Check if a row is complete
            CheckForGameOver(); // Check if the player has lost
            grid.DrawGrid(player, boxes); // Render the game state
            DisplayScore(); // Show the current score

            // Spawn a new box if enough time has passed
            if ((DateTime.Now - lastBoxSpawn).TotalMilliseconds >= boxSpawnInterval)
            {
                SpawnBox();
                lastBoxSpawn = DateTime.Now; // Reset spawn timer
            }

            Thread.Sleep(200); // Control game speed
        }
    }

    // Spawns a new falling box at a random x-position
    private void SpawnBox()
    {
        int x = rand.Next(1, GridManager.Width - 2); // Generate random x position
        boxes.Add(new Box(x, 1)); // Add new box to the list
    }

    // Updates box positions to simulate gravity
    private void UpdateBoxes()
    {
        foreach (var box in boxes)
        {
            // Move box down if there's empty space below
            if (!IsBoxAt(box.GetX(), box.GetY() + 1) && box.GetY() < GridManager.Height - 2)
            {
                box.SetY(box.GetY() + 1);
            }
        }
    }

    // Checks if there is a box at the given coordinates
    private bool IsBoxAt(int checkX, int checkY)
    {
        foreach (var box in boxes)
        {
            if (box.GetX() == checkX && box.GetY() == checkY)
            {
                return true; // Box found at location
            }
        }
        return false; // No box found
    }

    // Checks if any row is completely filled with boxes
    private void CheckFullRows()
    {
        for (int y = GridManager.Height - 2; y > 0; y--) // Start from the bottom
        {
            int count = 0; // Box counter
            for (int x = 1; x < GridManager.Width - 1; x++)
            {
                if (IsBoxAt(x, y)) count++; // Count filled spaces
            }

            // If the entire row is filled, remove the row
            if (count == GridManager.Width - 2)
            {
                List<Box> newBoxes = new List<Box>(); // Create new list for remaining boxes
                foreach (var box in boxes)
                {
                    if (box.GetY() != y) // Only keep boxes that are not in the full row
                    {
                        newBoxes.Add(box);
                    }
                }
                boxes = newBoxes; // Update the box list

                score += 100; // Increase score
                boxSpawnInterval -= boxSpawnInterval / 10; // Speed up box spawning

                // Set a minimum spawn interval to prevent excessive speed
                if (boxSpawnInterval < 500)
                {
                    boxSpawnInterval = 500;
                }
            }
        }
    }

    // Checks if the player has lost the game
    private void CheckForGameOver()
    {
        foreach (var box in boxes)
        {
            // If a box lands on the player, the game is over
            if (box.GetX() == player.GetX() && box.GetY() == player.GetY() - 1)
            {
                Console.Clear();
                Console.WriteLine("Game Over! Press Q to restart.");
                HighScoreManager.SaveHighScore(score); // Save the high score
                HighScoreManager.DisplayHighScores(); // Display leaderboard

                // Wait for the player to restart
                while (Console.ReadKey(true).Key != ConsoleKey.Q) { }

                RestartGame(); // Restart the game
            }
        }
    }

    // Restarts the game
    public void RestartGame()
    {
        Console.Clear();
        Console.WriteLine("Restarting game...");
        boxes.Clear(); // Clear all boxes
        score = 0; // Reset score
        boxSpawnInterval = 5000; // Reset spawn speed
        Start(); // Restart the game loop
    }

    // Displays the player's score on the screen
    private void DisplayScore()
    {
        Console.WriteLine("\nScore: " + score);
    }
}
