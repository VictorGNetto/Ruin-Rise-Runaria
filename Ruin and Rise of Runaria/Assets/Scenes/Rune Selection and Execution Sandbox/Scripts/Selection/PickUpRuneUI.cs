using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpRuneUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ProgramUI programUI;
    public GameObject details;
    public PopUp popUp;
    public String runeName;
    public int weight;
    public LevelDirector levelDirector;

    public void Select()
    {
        if (levelDirector.levelStartedRunning) {
            Vector3 mouse = Input.mousePosition;
            popUp.SetText("Reinicie o Level\nPara Modificar as Ações");
            popUp.Open(mouse.x, mouse.y - 100, 400, 100, 3);

            return;
        }

        if (!programUI.CanInsert(weight)) {
            Vector3 mouse = Input.mousePosition;
            popUp.SetText("Sem capacidade para\ninserir esta runa");
            popUp.Open(mouse.x, mouse.y - 100, 400, 100, 3);

            return;
        }

        GameObject duplicate = Instantiate(this.gameObject);
        programUI.InsertRune(duplicate, runeName, weight);
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
