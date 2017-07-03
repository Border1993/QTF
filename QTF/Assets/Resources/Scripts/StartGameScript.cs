using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;

        Screen.orientation = ScreenOrientation.Portrait;
        Screen.SetResolution(480, 800, false);

        SetupSettings();
	}

    private void SetupSettings()
    {
        int qualityLevel = PlayerPrefs.GetInt("qualityLevel");
        float volume = PlayerPrefs.GetFloat("volume");

        AudioListener.volume = volume;
        QualitySettings.SetQualityLevel(qualityLevel);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
