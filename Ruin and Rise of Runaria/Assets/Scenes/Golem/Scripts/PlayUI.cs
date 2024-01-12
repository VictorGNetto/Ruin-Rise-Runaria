using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayUI : MonoBehaviour
{
    public List<GolemProgram> golemPrograms;
    public List<Golem> golems;

    public void TogglePlay()
    {
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
        } else {
            Time.timeScale = 0;
        }
    }

    public void LoadPrograms()
    {
        for (int i = 0; i < golemPrograms.Count; i++)
        {
            golemPrograms[i].LoadProgram();
        }

        for (int i = 0; i < golems.Count; i++) {
            golems[i].Setup();
        }
    }
}
