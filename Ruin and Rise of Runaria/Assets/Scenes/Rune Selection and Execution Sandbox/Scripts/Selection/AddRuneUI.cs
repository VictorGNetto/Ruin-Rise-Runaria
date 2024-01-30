using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddRuneUI : MonoBehaviour
{
    public GameObject programUI;
    
    private GameObject add;
    private GameObject plusButton;

    private void Awake()
    {
        add = this.gameObject;
        plusButton = transform.GetChild(1).transform.GetChild(8).gameObject;
    }

    public void SetAsInsertPosition()
    {
        add.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);

        add.transform.Find("New Rune").gameObject.SetActive(true);
        add.transform.Find("Insert Position").gameObject.SetActive(false);
    }

    public void UnsetAsInsertPosition()
    {
        add.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);
    
        add.transform.Find("New Rune").gameObject.SetActive(false);
        add.transform.Find("Insert Position").gameObject.SetActive(true);
    }

    public void ChangeInsertRunePosition()
    {
        int newInsertRunePosition = this.transform.GetSiblingIndex();
        programUI.GetComponent<ProgramUI>().ChangeInsertRunePosition(newInsertRunePosition);
    }

    public void HidePlusButton()
    {
        if (plusButton == null) return;

        plusButton.SetActive(false);
    }
}
