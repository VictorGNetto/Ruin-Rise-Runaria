using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalRuneUI : RuneUI
{
    private GameObject runeIcon;
    private GameObject conditionalIcon;

    private void Awake()
    {
        runeIcon = transform.GetChild(0).gameObject;
        conditionalIcon = transform.GetChild(1).gameObject;
    }

    // public void SetRuneSprite(Sprite sprite)
    // {
    //     runeIcon.GetComponent<Image>().sprite = sprite;
    // }

    // public void RemoveRune()
    // {
    //     int removeRunePostion = transform.GetSiblingIndex();
    //     programUI.GetComponent<ProgramUI>().RemoveRune(removeRunePostion);
    // }
}
