using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HowToPlayScreen : MonoBehaviour
{

    public List<GameObject> pages;
    public Button previousPageButton;
    public Button nextPageButton;

    private int pageID;

    void Start ()
    {
        pageID = 0;
        pages[0].SetActive(true);
        previousPageButton.interactable = false;
	}

    public void PreviousPageButton()
    {
        nextPageButton.interactable = true;

        if (pageID > 0)
        {
            pages[pageID].SetActive(false);
            pageID--;
            pages[pageID].SetActive(true);

            if(pageID == 0)
            {
                previousPageButton.interactable = false;
            }
            else previousPageButton.interactable = true;
        }
        else return;

    }

    public void NextPageButton()
    {
        previousPageButton.interactable = true;

        if (pageID < pages.Count - 1)
        {
            pages[pageID].SetActive(false);
            pageID++;
            pages[pageID].SetActive(true);

            if (pageID == pages.Count - 1)
            {
                nextPageButton.interactable = false;
            }
            else nextPageButton.interactable = true;
        }
        else return;
    }

    public void MainMenuButton()
    {
        GameObject mainMenuScreen = transform.parent.Find("MainMenuScreen").gameObject;
        mainMenuScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
