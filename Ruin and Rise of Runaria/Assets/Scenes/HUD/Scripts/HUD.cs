using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;
    public bool levelStartedRunning = false;
    public List<RuneSelectionUI> runeSelectionUIs;

    private float runningTime = 0.0f;
    private GameObject timer;
    private GameObject reload;
    private GameObject pause;
    private GameObject play2;
    private GameObject play3;
    private int buttonState = 1;  // 1 - Paused; 2 - playing x1; 3 - playing x1.5; 4 - playing x2
    private bool playWasPressed = false;

    private void Awake()
    {
        Time.timeScale = 0;

        reload = transform.GetChild(1).transform.GetChild(0).gameObject;
        pause = transform.GetChild(1).transform.GetChild(1).gameObject;
        play2 = transform.GetChild(1).transform.GetChild(3).gameObject;
        play3 = transform.GetChild(1).transform.GetChild(4).gameObject;
        timer = transform.GetChild(2).transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        runningTime += Time.deltaTime;

        timer.GetComponent<Text>().text = GetFormatedRunningTime();
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
        Time.timeScale = 0;
    }

    public void Play()
    {
        // Active other buttons first time play's pressed
        if (!playWasPressed) {
            playWasPressed = true;
            runningTime = 0.0f;
            levelStartedRunning = true;
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
        Time.timeScale = 1;
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
        Time.timeScale = 1.5f;
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
        Time.timeScale = 2f;
    }

    private void DisableRuneSelectionInteractability()
    {
        for (int i = 0; i < runeSelectionUIs.Count; i++)
        {
            runeSelectionUIs[i].DisableInteractability();
        }
    }
}