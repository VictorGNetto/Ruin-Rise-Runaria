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
}
