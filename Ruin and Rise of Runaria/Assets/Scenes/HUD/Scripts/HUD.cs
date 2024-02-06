using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;
    public List<RuneSelectionUI> runeSelectionUIs;
    public LevelDirector levelDirector;

    private float runningTime = 0.0f;
    private GameObject timer;
    private GameObject reload;
    private GameObject pause;
    private GameObject play2;
    private GameObject play3;
    private int buttonState = 1;  // 1 - Paused; 2 - playing x1; 3 - playing x1.5; 4 - playing x2
    private bool playWasPressed = false;
    private float ticTac = 0;

    private void Awake()
    {
        ticTac = 0;
        Time.timeScale = ticTac;

        reload = transform.GetChild(1).transform.GetChild(0).gameObject;
        pause = transform.GetChild(1).transform.GetChild(1).gameObject;
        play2 = transform.GetChild(1).transform.GetChild(3).gameObject;
        play3 = transform.GetChild(1).transform.GetChild(4).gameObject;
        timer = transform.GetChild(2).transform.GetChild(0).gameObject;

        InitializeGolemPrograms();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelDirector.levelDone) return;
        
        runningTime += Time.deltaTime;

        timer.GetComponent<Text>().text = GetFormatedRunningTime();
    }

    private void InitializeGolemPrograms()
    {
        for (int i = 0; i < runeSelectionUIs.Count; i++)
        {
            runeSelectionUIs[i].SaveProgram();
        }
    }

    private String GetFormatedRunningTime()
    {
        float minutes = Mathf.Floor(runningTime / 60.0f);
        float seconds = Mathf.Floor(runningTime - 60.0f * minutes);

        return String.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        // Change buttons appearance
        int lastButtonState = buttonState;
        buttonState = 1;
        GameObject lastActiveButton = transform.GetChild(1).transform.GetChild(lastButtonState).gameObject;
        GameObject currentActiveButton = transform.GetChild(1).transform.GetChild(buttonState).gameObject;

        lastActiveButton.GetComponent<Button>().image.color = inactiveColor;
        currentActiveButton.GetComponent<Button>().image.color = activeColor;

        // Change the time scale
        ticTac = 0.0f;
        Time.timeScale = ticTac;
    }

    public void Play()
    {
        // Active other buttons first time play's pressed
        if (!playWasPressed) {
            playWasPressed = true;
            runningTime = 0.0f;
            levelDirector.levelStartedRunning = true;
            DisableRuneSelectionInteractability();
            reload.GetComponent<Button>().interactable = true;
            pause.GetComponent<Button>().interactable = true;
            play2.GetComponent<Button>().interactable = true;
            play3.GetComponent<Button>().interactable = true;
        }

        // Change buttons appearance
        int lastButtonState = buttonState;
        buttonState = 2;
        GameObject lastActiveButton = transform.GetChild(1).transform.GetChild(lastButtonState).gameObject;
        GameObject currentActiveButton = transform.GetChild(1).transform.GetChild(buttonState).gameObject;

        lastActiveButton.GetComponent<Button>().image.color = inactiveColor;
        currentActiveButton.GetComponent<Button>().image.color = activeColor;

        // Change the time scale
        ticTac = 1.0f;
        Time.timeScale = ticTac;
    }

    public void PlayFaster()
    {
        // Change buttons appearance
        int lastButtonState = buttonState;
        buttonState = 3;
        GameObject lastActiveButton = transform.GetChild(1).transform.GetChild(lastButtonState).gameObject;
        GameObject currentActiveButton = transform.GetChild(1).transform.GetChild(buttonState).gameObject;

        lastActiveButton.GetComponent<Button>().image.color = inactiveColor;
        currentActiveButton.GetComponent<Button>().image.color = activeColor;

        // Change the time scale
        ticTac = 1.5f;
        Time.timeScale = ticTac;
    }

    public void PlayEvenFaster()
    {
        // Change buttons appearance
        int lastButtonState = buttonState;
        buttonState = 4;
        GameObject lastActiveButton = transform.GetChild(1).transform.GetChild(lastButtonState).gameObject;
        GameObject currentActiveButton = transform.GetChild(1).transform.GetChild(buttonState).gameObject;

        lastActiveButton.GetComponent<Button>().image.color = inactiveColor;
        currentActiveButton.GetComponent<Button>().image.color = activeColor;

        // Change the time scale
        ticTac = 2.0f;
        Time.timeScale = ticTac;
    }

    private void DisableRuneSelectionInteractability()
    {
        for (int i = 0; i < runeSelectionUIs.Count; i++)
        {
            runeSelectionUIs[i].DisableInteractability();
        }
    }

    public void StopTicTac()
    {
        Time.timeScale = 0;
    }

    public void ResumeTicTac()
    {
        Time.timeScale = ticTac;
    }
}
