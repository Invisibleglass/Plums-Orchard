using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    private bool mutedBool = false;

    public Slider masterVolumeSlider;
    public Button muteButton;
    public Sprite muted;
    public Sprite unmuted;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        if(muteButton)
        {
            muteButton.onClick.AddListener(Mute);
        }
        // Load saved volume settings or initialize default values
        LoadSettings();
    }

    public void UpdateVolume()
    {
        // Implement logic to set Music volume in your game audio manager or directly
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        SaveSettings();
        LoadSettings();
        if (masterVolumeSlider.value > -80f)
        {
            mutedBool = false;
            muteButton.GetComponent<Image>().sprite = unmuted;
        }
    }

    private void LoadSettings()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        if (masterVolumeSlider.value == -80f)
        {
            mutedBool = true;
            muteButton.GetComponent<Image>().sprite = muted;
        }
    }

    public void SaveSettings()
    {
        // Save current slider values to PlayerPrefs or a similar storage method
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.Save();
    }

    private void Mute()
    {
        if (!mutedBool)
        {
            mutedBool = true;
            muteButton.GetComponent<Image>().sprite = muted;
            audioMixer.SetFloat("MasterVolume", -80f);
            SaveSettings();
        }
        else
        {
            mutedBool = false;
            muteButton.GetComponent<Image>().sprite = unmuted;
            audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
            SaveSettings();
        }
    }
}
