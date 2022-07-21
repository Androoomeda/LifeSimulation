using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject GamePanel;

    [SerializeField] private Toggle dontShowToggle;
    [SerializeField] private GameObject soundsOffButton, soundsOnButton;
    [SerializeField] private GameObject musicOffButton, musicOnButton;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] private InputController inputController;

    private bool isDontShowTutorial = false;
    private bool isMusicOn = true, isSoundsOn = true;

    private void Awake()
    {
        LoadPlayerSettings();
    }

    private void LoadPlayerSettings()
    {
        if (PlayerPrefs.GetString("DontShowTutorial") == "True")
        {
            isDontShowTutorial = true;
            dontShowToggle.isOn = true;
        }

        if (PlayerPrefs.GetString("Music") == "False")
        {
            isMusicOn = false;
            audioMixer.SetFloat("MusicVolume", -80f);
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
        }

        if (PlayerPrefs.GetString("Sounds") == "False")
        {
            isSoundsOn = false;
            audioMixer.SetFloat("SoundsVolume", -80f);
            soundsOffButton.SetActive(false);
            soundsOnButton.SetActive(true);
        }
    }

    private void Update()
    {
        if (TutorialPanel.activeSelf || MainMenuPanel.activeSelf)
            inputController.isBlocked = true;
        else
            inputController.isBlocked = false;
    }

    public void PlayGame()
    {
        if (!isDontShowTutorial)
            TutorialPanel.SetActive(true);
        else
            GamePanel.SetActive(true);
    }

    public void SaveToggleValue(bool isOn)
    {
        isDontShowTutorial = isOn;
        PlayerPrefs.SetString("DontShowTutorial", isDontShowTutorial.ToString());
    }

    public void SetSoundsSettings(bool isOn)
    {
        isSoundsOn = isOn;
        if (!isOn)
            audioMixer.SetFloat("SoundsVolume", -80f);
        else
            audioMixer.ClearFloat("SoundsVolume");
        PlayerPrefs.SetString("Sounds", isSoundsOn.ToString());
    }

    public void SetMusicSettings(bool isOn)
    {
        isMusicOn = isOn;
        if (!isOn)
            audioMixer.SetFloat("MusicVolume", -80f);
        else
            audioMixer.ClearFloat("MusicVolume");
        PlayerPrefs.SetString("Music", isMusicOn.ToString());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
