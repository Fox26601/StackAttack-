using System;
using System.Collections.Generic;
using System.IO;

// Manages high scores
class HighScoreManager
{
    private static string filePath = "highscores.txt";
    private static int maxHighScores = 5;

    // Checks if the player's score qualifies for the leaderboard
    public static bool IsHighScore(int score)
    {
        List<(string Name, int Score)> scores = LoadHighScores();
        
        // If the leaderboard has fewer than max scores or the score is higher than the lowest high score
        if (scores.Count < maxHighScores)
        {
            return true;
        }

        for (int i = 0; i < scores.Count; i++)
        {
            if (score > scores[i].Score)
            {
                return true;
            }
        }

        return false;
    }

    // Saves the high score if it qualifies
    public static void SaveHighScore(int score)
    {
        if (!IsHighScore(score))
        {
            Console.WriteLine("Your score was not high enough for the leaderboard.");
            return;
        }

        List<(string Name, int Score)> scores = LoadHighScores();
        Console.Write("Congratulations! You made the high score list. Enter your name: ");
        string name = Console.ReadLine();
        
        try
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Invalid name entered.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            name = "Unknown";
        }
        
        scores.Add((name, score));
        SortScores(scores);

        // Keep only the top scores
        while (scores.Count > maxHighScores)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        // Save to file
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < scores.Count; i++)
                {
                    writer.WriteLine(scores[i].Name + "," + scores[i].Score);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving high score: " + ex.Message);
        }
    }

    // Loads high scores from file
    public static List<(string Name, int Score)> LoadHighScores()
    {
        List<(string Name, int Score)> scores = new List<(string, int)>();

        if (File.Exists(filePath))
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int parsedScore))
                        {
                            scores.Add((parts[0], parsedScore));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading high scores: " + ex.Message);
            }
        }

        SortScores(scores);
        return scores;
    }

    // Displays the high scores
    public static void DisplayHighScores()
    {
        List<(string Name, int Score)> scores = LoadHighScores();
        
        if (scores.Count == 0)
        {
            Console.WriteLine("No high scores recorded yet.");
            return;
        }

        Console.WriteLine("\n=== High Scores ===");

        for (int i = 0; i < scores.Count; i++)
        {
            Console.WriteLine(scores[i].Name + ": " + scores[i].Score);
        }
    }

    // Sorts scores in descending order
    private static void SortScores(List<(string Name, int Score)> scores)
    {
        for (int i = 0; i < scores.Count - 1; i++)
        {
            for (int j = i + 1; j < scores.Count; j++)
            {
                if (scores[j].Score > scores[i].Score)
                {
                    var temp = scores[i];
                    scores[i] = scores[j];
                    scores[j] = temp;
                }
            }
        }
    }
}
