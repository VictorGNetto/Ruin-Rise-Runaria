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
    public ProgramUI programUI;
    public GameObject saveProgramGO;
    public GameObject interactablityMessageGO ;

    private float lastTimeScale;
    private List<GameObject> savedProgram = new List<GameObject>();
    private int savedinsertRunePosition = 0;

    public void OpenRuneSelectionUI()
    {
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        
        SaveProgramState();
        programUI.insertRunePosition = savedinsertRunePosition;

        gameObject.SetActive(true);
    }

    private void SaveProgramState()
    {
        for (int i = 0; i < savedProgram.Count; i++) {
            Destroy(savedProgram[i]);
        }
        savedProgram.Clear();

        for (int i = 0; i < scrollContent.transform.childCount; i++) {
            GameObject go = Instantiate(scrollContent.transform.GetChild(i).gameObject);
            go.SetActive(false);
            savedProgram.Add(go);
        }
    }

    public void CloseRuneSelectionUI()
    {
        LoadSavedProgramState();
        Time.timeScale = lastTimeScale;
        gameObject.SetActive(false);
    }

    public void LoadSavedProgramState()
    {
        List<GameObject> toBeDestroyed = new List<GameObject>();
        for (int i = 0; i < scrollContent.transform.childCount; i++) {
            GameObject go = scrollContent.transform.GetChild(i).gameObject;
            go.SetActive(false);
            toBeDestroyed.Add(go);
        }

        for (int i = 0; i < savedProgram.Count; i++) {
            savedProgram[i].SetActive(true);
            savedProgram[i].transform.SetParent(scrollContent.transform);
            savedProgram[i].transform.SetSiblingIndex(i);
        }

        savedProgram.Clear();

        for (int i = 0; i < toBeDestroyed.Count; i++) {
            Destroy(toBeDestroyed[i]);
        }
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

    public void SaveProgram()
    {
        savedinsertRunePosition = programUI.insertRunePosition;
        SaveProgramState();

        golemProgram.program = GetProgram();
        golemProgram.runeSprites = GetRuneSprites();
        golemProgram.Reset();

        golem.Setup();
    }

    public void DisableInteractability()
    {
        // Hide remove buttons from the RuneUI and ConditionalRuneUI
        int length = scrollContent.transform.childCount / 2;

        programUI.ChangeInsertRunePosition(length*2);

        for (int i = 0; i < length; i++)
        {
            Transform transform = scrollContent.transform.GetChild(2*i + 1);
            if (transform.GetComponent<RuneUI>() != null) {
                transform.GetComponent<RuneUI>().HideRemoveButton();
            } else if (transform.GetComponent<ConditionalRuneUI>() != null) {
                transform.GetComponent<ConditionalRuneUI>().HideRemoveButton();
            }
        }

        for (int i = 0; i <= length; i++)
        {
            Transform transform = scrollContent.transform.GetChild(2*i);
            transform.GetComponent<AddRuneUI>().HidePlusButton();
        }

        saveProgramGO.SetActive(false);
        interactablityMessageGO.SetActive(true);
    }
}
