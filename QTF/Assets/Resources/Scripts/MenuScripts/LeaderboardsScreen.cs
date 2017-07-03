using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsScreen : MonoBehaviour
{
    public GameObject scorePanel;
    public GameObject scorePrefab;
    


	// Use this for initialization
	void Start ()
    {
        enabled = false;
	}

    public void Toggle(bool state)
    {
        enabled = state;
    }

    public void Refresh()
    {
        DeleteChildren();
        var scoreList = Leaderboard.instance.GetScores();

        int index = 1;
        foreach (var score in scoreList)
        {
            GameObject scoreObject = Instantiate(scorePrefab);
            scoreObject.GetComponent<Text>().text = index.ToString() + " : " + score.ToString();
            scoreObject.transform.SetParent(scorePanel.transform);

            index++;
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void MainMenuButton()
    {
        this.enabled = false;
        GameObject mainMenu = transform.parent.Find("MainMenuScreen").gameObject;
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void DeleteChildren()
    {
        for(int i = 0; i < scorePanel.transform.childCount; i++)
        {
            Destroy(scorePanel.transform.GetChild(i).gameObject);
        }
    }
}
