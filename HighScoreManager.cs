class HighScoreManager
{
    private static string highScoreFile = "highscores.txt"; // File where high scores are stored

    // Saves the player's high score if it's among the top 5
    public static void SaveHighScore(int score)
    {
        List<(string Name, int Score)> scores = new List<(string, int)>(); // List to store high scores

        // Read existing high scores from file if available
        if (File.Exists(highScoreFile))
        {
            string[] lines = File.ReadAllLines(highScoreFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int parsedScore))
                {
                    scores.Add((parts[0], parsedScore));
                }
            }
        }

        // Check if the player's score qualifies for the leaderboard
        if (scores.Count < 5 || score > scores[scores.Count - 1].Score)
        {
            Console.Write("Congratulations! You made the high score list. Enter your name: ");
            string name = Console.ReadLine();
            try
            {
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Invalid name entered.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                name = "Unknown";
            }
            // Add new score to the list
            scores.Add((name, score));

            // Sort scores in descending order using a comparison method
            scores.Sort(CompareScores);

            // Keep only the top 5 scores
            if (scores.Count > 5)
            {
                scores.RemoveAt(5);
            }

            // Save scores back to file
            List<string> outputLines = new List<string>();
            foreach (var entry in scores)
            {
                outputLines.Add(entry.Name + "," + entry.Score);
            }
            File.WriteAllLines(highScoreFile, outputLines);
        }
        else
        {
            Console.WriteLine("Your score was not high enough for the leaderboard.");
        }
    }

    // Comparison method for sorting scores in descending order
    private static int CompareScores((string Name, int Score) a, (string Name, int Score) b)
    {
        return b.Score.CompareTo(a.Score);
    }

    // Displays the top high scores in descending order
    public static void DisplayHighScores()
    {
        if (!File.Exists(highScoreFile))
        {
            Console.WriteLine("No high scores recorded yet.");
            return;
        }

        Console.WriteLine("\n=== High Scores ===");

        string[] lines = File.ReadAllLines(highScoreFile);
        List<(string Name, int Score)> scores = new List<(string, int)>();

        // Parse high scores from file
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2 && int.TryParse(parts[1], out int parsedScore))
            {
                scores.Add((parts[0], parsedScore));
            }
        }

        // Sort scores in descending order using the same comparison method
        scores.Sort(CompareScores);

        // Print high scores
        foreach (var entry in scores)
        {
            Console.WriteLine($"{entry.Name}: {entry.Score}");
        }
    }
}
