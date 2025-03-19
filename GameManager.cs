
// Manages the overall game flow and logic
class GameManager
{
    private GridManager grid;
    private Player player;
    private int score;
    private int boxSpawnInterval;

    // Constructor initializes the game components
    public GameManager()
    {
        grid = new GridManager(); // Create the grid
        player = new Player(); // Create the player
        score = 0;
        boxSpawnInterval = 5000;
    }

    // Starts the game loop
    public void Start()
    {
        DateTime lastBoxSpawn = DateTime.Now;

        while (true)
        {
            player.Update();
            Box.UpdateBoxes();
            CheckFullRows();
            CheckForGameOver();
            grid.DrawGrid(player, Box.GetAllBoxes()); // Use static method instead of direct access
            DisplayScore();

            if ((DateTime.Now - lastBoxSpawn).TotalMilliseconds >= boxSpawnInterval)
            {
                Box.SpawnBox();
                lastBoxSpawn = DateTime.Now;
            }

            Thread.Sleep(200);
        }
    }
    // Checks and clears filled rows, updates score and difficulty
    private void CheckFullRows()
    {
        for (int y = GridManager.Height - 2; y > 0; y--)
        {
            int count = 0;
            for (int x = 1; x < GridManager.Width - 1; x++)
            {
                if (Box.IsBoxAt(x, y))
                {
                    count++;
                }
            }

            if (count == GridManager.Width - 2)
            {
                Box.RemoveRow(y);
                score += 100;
                boxSpawnInterval = Math.Max(500, boxSpawnInterval - boxSpawnInterval / 10);
            }
        }
    }

    private void CheckForGameOver()
    {
        if (Box.IsBoxAt(player.GetX(), player.GetY() - 1))
        {
            Console.Clear();
            Console.WriteLine("Game Over! Press Q to restart.");
            HighScoreManager.SaveHighScore(score);
            HighScoreManager.DisplayHighScores();

            while (Console.ReadKey(true).Key != ConsoleKey.Q) { }
            RestartGame();
        }
    }

    public void RestartGame()
    {
        Console.Clear();
        Console.WriteLine("Restarting game...");
        Box.ClearAllBoxes();
        score = 0;
        boxSpawnInterval = 5000;
        Start();
    }

    private void DisplayScore()
    {
        Console.WriteLine("\nScore: " + score);
    }
}
