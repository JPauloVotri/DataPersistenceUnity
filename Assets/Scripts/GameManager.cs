using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PlayerName { get; set; }
    public HighScore BestScore { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    [System.Serializable]
    public class HighScore
    {
        public string Name;
        public int Score;

        public HighScore(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public void SaveScoreIfIsHigher(int score)
    {
        if (BestScore != null && score <= BestScore.Score)
            return;

        BestScore = new(PlayerName, score);
        SaveScore();
    }

    private void SaveScore()
    {
        string json = JsonUtility.ToJson(BestScore);
        File.WriteAllText($"{Application.persistentDataPath}/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = $"{Application.persistentDataPath}/savefile.json";
        Debug.Log(path);

        if (!File.Exists(path))
            return;

        string json = File.ReadAllText(path);
        BestScore = JsonUtility.FromJson<HighScore>(json);
    }
}
