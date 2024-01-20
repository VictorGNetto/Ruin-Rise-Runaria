using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRuneUI : MonoBehaviour
{
    public ProgramUI programUI;
    public GameObject details;
    public String runeName;
    public LevelDirector levelDirector;

    public void Select()
    {
        if (levelDirector.levelStartedRunning) return;

        GameObject duplicate = Instantiate(this.gameObject);
        programUI.InsertRune(duplicate, runeName);
    }

    public void ShowRuneDetails()
    {
        details.SetActive(true);
    }

    public void HideRuneDetails()
    {
        details.SetActive(false);
    }
}
