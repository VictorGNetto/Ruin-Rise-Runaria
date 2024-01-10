using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramUI : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject RunePrefab;
    public GameObject AddRunePrefab;

    private GameObject empty;
    private int insertRunePosition = 0;

    private void Awake()
    {
        empty = new GameObject();
    }
    
    public void InsertRune(GameObject rune, String runeName)
    {
        scrollContent.transform.GetChild(insertRunePosition).GetComponent<AddRuneUI>().UnsetAsInsertPosition();

        GameObject runeGO = Instantiate(RunePrefab, scrollContent.transform);
        runeGO.transform.SetSiblingIndex(insertRunePosition + 1);
        runeGO.GetComponent<RuneUI>().SetRuneSprite(rune.GetComponent<Image>().sprite);
        runeGO.GetComponent<RuneUI>().programUI = this.gameObject;

        GameObject addRuneGO = Instantiate(AddRunePrefab, scrollContent.transform);
        addRuneGO.transform.SetSiblingIndex(insertRunePosition + 2);
        addRuneGO.GetComponent<AddRuneUI>().SetAsInsertPosition();
        addRuneGO.GetComponent<AddRuneUI>().programUI = this.gameObject;

        insertRunePosition += 2;
        Destroy(rune);
    }

    public void ChangeInsertRunePosition(int index)
    {
        scrollContent.transform.GetChild(insertRunePosition).GetComponent<AddRuneUI>().UnsetAsInsertPosition();
        scrollContent.transform.GetChild(index).GetComponent<AddRuneUI>().SetAsInsertPosition();
        insertRunePosition = index;

        FixProgramUI();
    }

    public void RemoveRune(int index)
    {
        GameObject go1 = scrollContent.transform.GetChild(index).gameObject;
        GameObject go2 = scrollContent.transform.GetChild(index + 1).gameObject;

        Destroy(go1);
        Destroy(go2);

        if (insertRunePosition == index + 1)
        {
            insertRunePosition = index - 1;
            scrollContent.transform.GetChild(insertRunePosition).GetComponent<AddRuneUI>().SetAsInsertPosition();
        } else if (insertRunePosition > index) {
            insertRunePosition -= 2;
        }

        // GameObject go = scrollContent.transform.GetChild(index).gameObject;
        // go.transform.SetParent(scrollContent.transform.parent.transform);
        // Destroy(go);
        // if (insertRunePosition > index) {
        //     insertRunePosition -= 1;
        // }

        // UpdateInsertPositionUI();
    }

    private void FixProgramUI()
    {
        empty.transform.SetParent(scrollContent.transform);
        empty.transform.SetParent(scrollContent.transform.parent.transform);
    }

    // public void InsertRune(GameObject rune, String runeName)
    // {
    //     GameObject go = Instantiate(programRunePrefab, scrollContent.transform);
    //     go.transform.SetSiblingIndex(insertRunePosition);
    //     go.GetComponent<ProgramRuneUI>().SetRuneSprite(rune.GetComponent<Image>().sprite);
    //     go.GetComponent<ProgramRuneUI>().ShowRune();
    //     go.GetComponent<ProgramRuneUI>().programUI = this.gameObject;

    //     insertRunePosition += 1;
    //     UpdateInsertPositionUI();
    //     Destroy(rune);
    // }

    // private void UpdateInsertPositionUI()
    // {
    //     for (int i = 0; i < scrollContent.transform.childCount; i++) {
    //         GameObject go = scrollContent.transform.GetChild(i).gameObject;
            
    //         if (i != insertRunePosition - 1 && i != insertRunePosition) {
    //             go.GetComponent<ProgramRuneUI>().ShowInsertPositionUI();
    //             go.GetComponent<ProgramRuneUI>().UnsetAsInsertPosition();
    //         } else if (i != insertRunePosition) {
    //             go.GetComponent<ProgramRuneUI>().HideInsertPositionUI();
    //         }
    //     }
    // }

    // public void ChangeInsertRunePosition(int index)
    // {
    //     if (index == scrollContent.transform.childCount - 1) {
    //         scrollContent.transform.GetChild(insertRunePosition).transform.SetSiblingIndex(index);
    //         insertRunePosition = index;
    //     } else {
    //         scrollContent.transform.GetChild(insertRunePosition).transform.SetSiblingIndex(index + 1);
    //         insertRunePosition = index + 1;
    //     }

    //     UpdateInsertPositionUI();
    // }

    // public void RemoveRune(int index)
    // {
    //     GameObject go = scrollContent.transform.GetChild(index).gameObject;
    //     go.transform.SetParent(scrollContent.transform.parent.transform);
    //     Destroy(go);
    //     if (insertRunePosition > index) {
    //         insertRunePosition -= 1;
    //     }

    //     UpdateInsertPositionUI();
    // }
}
