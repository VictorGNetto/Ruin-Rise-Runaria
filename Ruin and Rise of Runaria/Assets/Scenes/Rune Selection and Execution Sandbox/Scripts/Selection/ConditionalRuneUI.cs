using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalRuneUI : MonoBehaviour
{
    public GameObject programUI;
    public String conditionalMode;
    public String runeName;
    public String conditionalRuneName;

    private GameObject runeIcon;
    private GameObject conditionalIcon;

    private void Awake()
    {
        runeIcon = transform.GetChild(0).gameObject;
        conditionalIcon = transform.GetChild(1).gameObject;
    }

    public void SetRuneSprite(Sprite sprite)
    {
        runeIcon.GetComponent<Image>().sprite = sprite;
    }

    public void SetConditionalRuneSprite(Sprite sprite)
    {
        conditionalIcon.GetComponent<Image>().sprite = sprite;
    }

    public void RemoveRune()
    {
        int removeRunePostion = transform.GetSiblingIndex();
        programUI.GetComponent<ProgramUI>().RemoveRune(removeRunePostion);
    }
}
