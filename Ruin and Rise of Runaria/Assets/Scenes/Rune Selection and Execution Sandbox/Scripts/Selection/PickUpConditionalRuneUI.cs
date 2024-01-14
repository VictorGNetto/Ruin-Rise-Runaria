using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpConditionalRuneUI : MonoBehaviour
{
    public ProgramUI programUI;
    public GameObject details;
    public String conditionalRuneName;

    public Sprite unselected;
    public Sprite selectedAsIf;
    public Sprite selectedAsElse;

    public List<PickUpConditionalRuneUI> othersPickUpConditionalRune;

    private int internalState = 0;  // 0 - unselected; 1 - selected as IF; 2 - selected as ELSE

    public void Select()
    {
        internalState = (internalState + 1) % 3;

        if (internalState == 0) {
            gameObject.GetComponent<Image>().sprite = unselected;
            programUI.UnsetInsertRuneAsConditional();
            SetOthersPickUpConditionalRuneInteractivity(true);
        } else if (internalState == 1) {
            gameObject.GetComponent<Image>().sprite = selectedAsIf;
            programUI.SetInsertRuneAsConditional(selectedAsIf, "IF", conditionalRuneName);
            SetOthersPickUpConditionalRuneInteractivity(false);
        } else {
            gameObject.GetComponent<Image>().sprite = selectedAsElse;
            programUI.SetInsertRuneAsConditional(selectedAsElse, "ELSE", conditionalRuneName);
        }
    }

    private void SetOthersPickUpConditionalRuneInteractivity(bool interactivity)
    {
        if (interactivity) {
            for (int i = 0; i < othersPickUpConditionalRune.Count; i++) {
                othersPickUpConditionalRune[i].SetInteractable();
            }
        } else {
            for (int i = 0; i < othersPickUpConditionalRune.Count; i++) {
                othersPickUpConditionalRune[i].UnsetInteractable();
            }
        }
    }

    public void SetInteractable()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void UnsetInteractable()
    {
        gameObject.GetComponent<Button>().interactable = false;
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
