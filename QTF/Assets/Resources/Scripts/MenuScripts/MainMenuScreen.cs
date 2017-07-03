using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public void QuitButton()
    {
        Debug.Log("Application is quitting...");
        Application.Quit();
    }

    public void NewGameButton()
    {
        GameObject ingameCategory = GameObject.Find("Ingame");
        GameObject gameObject = ingameCategory.transform.Find("GameScript").gameObject;
        gameObject.SetActive(true);

        Game gameScript = gameObject.GetComponent<Game>();
        gameScript.NewGame();

        this.gameObject.SetActive(false);
    }

    public void LeaderboardButton()
    {
        GameObject leaderboardsMenu = transform.parent.Find("LeaderboardsScreen").gameObject;
        leaderboardsMenu.SetActive(true);
        gameObject.SetActive(false);
        leaderboardsMenu.GetComponent<LeaderboardsScreen>().Toggle(true);
    }

    public void HowToPlayButton()
    {
        GameObject howToPlayMenu = transform.parent.Find("HowToPlayScreen").gameObject;
        howToPlayMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SettingsButton()
    {
        GameObject settingsMenu = transform.parent.Find("SettingsScreen").gameObject;
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
