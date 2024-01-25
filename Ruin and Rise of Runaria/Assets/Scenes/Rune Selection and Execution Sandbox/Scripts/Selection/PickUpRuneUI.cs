using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpRuneUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ProgramUI programUI;
    public GameObject details;
    public String runeName;
    public LevelDirector levelDirector;

    public void Select()
    {
        if (levelDirector.levelStartedRunning) return;

        GameObject duplicate = Instantiate(this.gameObject);
        programUI.InsertRune(duplicate, runeName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        details.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        details.SetActive(false);
    }
}
