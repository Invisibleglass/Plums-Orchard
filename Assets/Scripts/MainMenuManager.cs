using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuManager : MonoBehaviour
{
    private SoundManager soundManager;

    [Header("Sounds")]
    public AudioClip buttonClickSound;

    [Header("Screens")]
    public GameObject mainMenuScreen;
    public GameObject controlsScreen;
    public GameObject creditsScreen;

    [Header("Buttons")]
    public Button startButton;
    public Button controlsButton;
    public Button creditsButton;
    public List<Button> backButtons;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        if (startButton)
            startButton.onClick.AddListener(ToGameScene);
        if (controlsButton)
            controlsButton.onClick.AddListener(OpenControlsScreen);
        if (creditsButton)
            creditsButton.onClick.AddListener(OpenCreditsScreen);
        for (int i = 0; i < backButtons.Count; i++)
        {
            if (backButtons[i])
                backButtons[i].onClick.AddListener(OpenMainScreen);
        }
    }

    private void ToGameScene()
    {
        soundManager.PlayOneShot(buttonClickSound);
        SceneManager.LoadScene("GameScene");
    }

    private void OpenControlsScreen()
    {
        soundManager.PlayOneShot(buttonClickSound);
        mainMenuScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    private void OpenCreditsScreen()
    {
        soundManager.PlayOneShot(buttonClickSound);
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    private void OpenMainScreen()
    {
        soundManager.PlayOneShot(buttonClickSound);
        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
