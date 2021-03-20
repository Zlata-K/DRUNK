using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreManager : MonoBehaviour
{
    [Serializable]
    public class ScoreData
    {
        public string name;
        public int score;
        public DateTime date;
    }

    public const int maxHighscores = 10; // Is 10 highscores enough ?

    private static string scoreFilename = Path.Combine(Application.persistentDataPath, "scores.txt");
    private static FileStream scoreFile = new FileStream(scoreFilename, FileMode.OpenOrCreate);
    private static List<ScoreData> scores = ReadScores();

    private static void SaveScores()
    {
        IFormatter formatter = new BinaryFormatter();

        scoreFile.SetLength(0); // Truncate the file
        formatter.Serialize(scoreFile, scores);
        scoreFile.Flush();
    }

    private static List<ScoreData> ReadScores()
    {
        IFormatter formatter = new BinaryFormatter();

        try {
            return (List<ScoreData>)formatter.Deserialize(scoreFile);
        } catch { // If file doesn't exist
            return new List<ScoreData>();
        }
    }

    // Return true if the given score will enter the highscore list
    public static bool IsHighScore(int newscore)
    {
        if (scores.Count < maxHighscores)
            return true;
        if (scores[0].score > newscore)
            return false;
        return true;
    }

    // Add the new highscore to list and sort it
    public static void AddScore(string name, int score)
    {
        if (!IsHighScore(score))
            return;
        scores.Add(new ScoreData { date = System.DateTime.Now, name = name, score = score });
        scores.Sort((ScoreData a, ScoreData b) => { return a.score - b.score; });
        if (scores.Count > maxHighscores)
            scores.RemoveAt(0);
        SaveScores();
    }

    // Clear the highscores
    public static void Clear()
    {
        scores.Clear();
        SaveScores();
    }

    // Get the current highscore list (read only)
    public static IList<ScoreData> GetScores()
    {
        return scores.AsReadOnly();
    }
}
