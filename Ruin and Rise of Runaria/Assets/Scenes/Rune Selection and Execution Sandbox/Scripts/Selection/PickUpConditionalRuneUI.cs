using System;
using UnityEngine;
using UnityEngine.UI;

public class PickUpConditionalRuneUI : MonoBehaviour
{
    public ProgramUI programUI;
    public GameObject details;
    public String runeName;

    public Sprite unselected;
    public Sprite selectedAsIf;
    public Sprite selectedAsElse;

    private int internalState = 0;  // 0 - unselected; 1 - selected as IF; 2 - selected as ELSE

    public void Select()
    {
        internalState = (internalState + 1) % 3;

        if (internalState == 0) {
            gameObject.GetComponent<Image>().sprite = unselected;
        } else if (internalState == 1) {
            gameObject.GetComponent<Image>().sprite = selectedAsIf;
        } else {
            gameObject.GetComponent<Image>().sprite = selectedAsElse;
        }

        // runeIcon.GetComponent<Image>().sprite = sprite;
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
