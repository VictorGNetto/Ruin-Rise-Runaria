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
        runeGO.GetComponent<RuneUI>().runeName = runeName;

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
    }

    private void FixProgramUI()
    {
        empty.transform.SetParent(scrollContent.transform);
        empty.transform.SetParent(scrollContent.transform.parent.transform);
    }
}
