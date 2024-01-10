using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramRuneUI : MonoBehaviour
{
    public GameObject programUI;

    private GameObject rune;
    private GameObject remove;
    private GameObject add;

    private void Awake()
    {
        rune = this.transform.GetChild(0).gameObject;
        remove = this.transform.GetChild(1).gameObject;
        add = this.transform.GetChild(2).gameObject;
    }

    public void SetRuneSprite(Sprite sprite)
    {
        rune.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    public void ShowRune()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100);

        rune.SetActive(true);
        remove.SetActive(true);
    }

    public void HideInsertPositionUI()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100);

        add.SetActive(false);
    }

    public void ShowInsertPositionUI()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 150);

        add.SetActive(true);
    }

    public void SetAsInsertPosition()
    {
        add.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 200);

        add.transform.Find("New Rune").gameObject.SetActive(true);
        add.transform.Find("Insert Position").gameObject.SetActive(false);
    }

    public void UnsetAsInsertPosition()
    {
        add.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);
        
        add.transform.Find("New Rune").gameObject.SetActive(false);
        add.transform.Find("Insert Position").gameObject.SetActive(true);
    }

    // public void ChangeInsertRunePosition()
    // {
    //     int newInsertRunePosition = this.transform.GetSiblingIndex();
    //     programUI.GetComponent<ProgramUI>().ChangeInsertRunePosition(newInsertRunePosition);
    // }

    // public void RemoveRune()
    // {
    //     int removeRunePostion = this.transform.GetSiblingIndex();
    //     programUI.GetComponent<ProgramUI>().RemoveRune(removeRunePostion);
    // }
}
