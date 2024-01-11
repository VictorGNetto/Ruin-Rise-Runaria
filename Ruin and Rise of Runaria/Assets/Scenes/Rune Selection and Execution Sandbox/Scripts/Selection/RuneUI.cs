using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneUI : MonoBehaviour
{
    public GameObject programUI;
    public String runeName;

    private GameObject runeIcon;

    private void Awake()
    {
        runeIcon = this.transform.GetChild(0).gameObject;
    }

    public void SetRuneSprite(Sprite sprite)
    {
        runeIcon.GetComponent<Image>().sprite = sprite;
    }

    public void RemoveRune()
    {
        int removeRunePostion = this.transform.GetSiblingIndex();
        programUI.GetComponent<ProgramUI>().RemoveRune(removeRunePostion);
    }
}
