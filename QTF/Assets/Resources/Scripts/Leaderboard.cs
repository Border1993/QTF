using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    private List<int> scores;

    public static Leaderboard instance
    {
        get;
        private set;
    }

    private void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    private void Awake()
    {
        scores = new List<int>();
        LoadScores();
        SaveScores();
    }

    private void OnDestroy()
    {
        SaveScores();
    }

    public void LoadScores()
    {
        for (int i = 0; i < 10; i++)
        {
            int value = PlayerPrefs.GetInt("leaderBoardPlace" + i.ToString() + "Points");
            scores.Add(value);
            PlayerPrefs.SetInt("leaderBoardPlace" + i.ToString() + "Points", scores[i]);
        }
        PlayerPrefs.Save();
    }

    public void SaveScores()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("leaderBoardPlace" + i.ToString() + "Points", scores[i]);
        }

        PlayerPrefs.Save();
    }

    public void AddScore(int score)
    {
        scores.Add(score);
        scores.Sort((a, b) => -1 * a.CompareTo(b));

        while (scores.Count > 10) scores.RemoveAt(10);
        SaveScores();
    }

    public List<int> GetScores()
    {
        return new List<int>(scores);
    }

}