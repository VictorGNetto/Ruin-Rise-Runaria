using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelectionUI : MonoBehaviour
{
    public GameObject scrollContent;
    public Golem golem;
    public GolemProgram golemProgram;
    private float lastTimeScale;

    public void OpenRuneSelectionUI()
    {
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void CloseRuneSelectionUI()
    {
        Time.timeScale = lastTimeScale;
        gameObject.SetActive(false);
    }

    private List<String> GetProgram()
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

    private Dictionary<String, Sprite> GetRuneSprites()
    {
        Dictionary<String, Sprite> runeSprites = new Dictionary<string, Sprite>();

        int length = scrollContent.transform.childCount / 2;

        for (int i = 0; i < length; i++)
        {
            Transform transform = scrollContent.transform.GetChild(2*i + 1);
            if (transform.GetComponent<RuneUI>() != null) {
                Sprite sprite = transform.GetChild(0).GetComponent<Image>().sprite;
                runeSprites[transform.GetComponent<RuneUI>().runeName] = sprite;
            } else if (transform.GetComponent<ConditionalRuneUI>() != null) {
                Sprite sprite = transform.GetChild(0).GetComponent<Image>().sprite;
                runeSprites[transform.GetComponent<ConditionalRuneUI>().runeName] = sprite;

                sprite = transform.GetChild(1).GetComponent<Image>().sprite;
                runeSprites[transform.GetComponent<ConditionalRuneUI>().conditionalRuneName] = sprite;
            }
        }

        return runeSprites;
    }

    public void LoadProgram()
    {
        golemProgram.program = GetProgram();
        golemProgram.runeSprites = GetRuneSprites();
        golemProgram.Reset();

        golem.Setup();
    }
}
