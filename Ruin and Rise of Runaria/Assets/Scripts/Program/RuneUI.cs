using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneUI : MonoBehaviour
{
    public GameObject programUI;

    private GameObject runeIcon;

    private void Awake()
    {
        runeIcon = this.transform.GetChild(0).gameObject;
    }

    public void SetRuneSprite(Sprite sprite)
    {
        Debug.Log(runeIcon);
        runeIcon.GetComponent<Image>().sprite = sprite;
    }

    public void RemoveRune()
    {
        int removeRunePostion = this.transform.GetSiblingIndex();
        programUI.GetComponent<ProgramUI>().RemoveRune(removeRunePostion);
    }
}
