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
        List<String> program = new List<string>();

        int length = scrollContent.transform.childCount / 2;

        for (int i = 0; i < length; i++)
        {
            Transform transform = scrollContent.transform.GetChild(2*i + 1);
            if (transform.GetComponent<RuneUI>() != null) {
                program.Add(transform.GetComponent<RuneUI>().runeName);
            } else if (transform.GetComponent<ConditionalRuneUI>() != null) {
                program.Add(transform.GetComponent<ConditionalRuneUI>().conditionalMode);
                program.Add(transform.GetComponent<ConditionalRuneUI>().conditionalRuneName);
                program.Add(transform.GetComponent<ConditionalRuneUI>().runeName);
            }
        }

        return program;
    }
}
