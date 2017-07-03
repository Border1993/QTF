using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text scoreText;

    private bool wasEnabled = false;

    public void Update()
    {
        if(wasEnabled == false)
        {
            Debug.Log("Logging score...");
            wasEnabled = true;
            int score = Game.instance.score;
            scoreText.text = "Score : " + score.ToString();
        }
    }

    public void RestartButton()
    {
        wasEnabled = false;
        Game.instance.NewGame();
        Game.instance.enabled = true;
        gameObject.SetActive(false);
    }

    public void LeaderboardsButton()
    {
        wasEnabled = false;
        GameObject leaderboardsScreen = transform.parent.Find("LeaderboardsScreen").gameObject;
        leaderboardsScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        wasEnabled = false;
        GameObject mainMenuScreen = transform.parent.Find("MainMenuScreen").gameObject;
        mainMenuScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
