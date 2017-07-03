using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    public Slider slider;
    public Text qualityLevelText;

    private List<string> qualityLevelStrings;
    private int qualityLevel;

    private void Start()
    {
        slider.value = AudioListener.volume;
        qualityLevelStrings = new List<string>();
        qualityLevelStrings.Add("Fastest");
        qualityLevelStrings.Add("Fast");
        qualityLevelStrings.Add("Simple");
        qualityLevelStrings.Add("Good");
        qualityLevelStrings.Add("Beautiful");
        qualityLevelStrings.Add("Fantastic");
        
        qualityLevel = QualitySettings.GetQualityLevel();
        qualityLevelText.text = qualityLevelStrings[qualityLevel];
    }

    public void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat("volume", slider.value);
        AudioListener.volume = slider.value;
    }

    public void OnQualityLevelIncrease()
    {
        qualityLevel++;
        if (qualityLevel >= qualityLevelStrings.Count) qualityLevel = 0;
        qualityLevelText.text = qualityLevelStrings[qualityLevel];
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("qualityLevel", qualityLevel);
    }

    public void OnQualityLevelDecrease()
    {
        qualityLevel--;
        if (qualityLevel < 0) qualityLevel = qualityLevelStrings.Count - 1;
        qualityLevelText.text = qualityLevelStrings[qualityLevel];
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("qualityLevel", qualityLevel);
    }

    public void BackButton()
    {
        this.gameObject.SetActive(false);
        GameObject mainMenu = transform.parent.Find("MainMenuScreen").gameObject;
        mainMenu.SetActive(true);
        PlayerPrefs.Save();
    }

}
