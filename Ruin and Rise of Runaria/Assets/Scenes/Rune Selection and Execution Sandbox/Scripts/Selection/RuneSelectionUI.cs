using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneSelectionUI : MonoBehaviour
{
    public GameObject scrollContent;

    public void CloseRuneSelectionUI()
    {
        this.gameObject.SetActive(false);
    }

    public List<String> GetProgram()
    {
        int length = scrollContent.transform.childCount / 2;
        string[] program = new string[length];

        for (int i = 0; i < length; i++)
        {
            program[i] = scrollContent.transform.GetChild(2*i + 1).GetComponent<RuneUI>().runeName;
        }

        return program.OfType<String>().ToList();
    }
}
