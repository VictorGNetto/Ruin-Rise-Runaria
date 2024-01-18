using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgramUI : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject runePrefab;
    public GameObject conditionalRunePrefab;
    public GameObject addRunePrefab;

    private GameObject empty;
    public int insertRunePosition = 0;

    private String conditionalMode;
    private Sprite conditionalRuneSprite;

    private bool insertRuneAsConditional = false;
    private String conditionalRuneName;

    private void Awake()
    {
        empty = new GameObject();
    }
    
    public void InsertRune(GameObject rune, String runeName)
    {
        scrollContent.transform.GetChild(insertRunePosition).GetComponent<AddRuneUI>().UnsetAsInsertPosition();

        if (insertRuneAsConditional) {
            GameObject conditionalRuneGO = Instantiate(conditionalRunePrefab, scrollContent.transform);
            conditionalRuneGO.transform.SetSiblingIndex(insertRunePosition + 1);
            conditionalRuneGO.GetComponent<ConditionalRuneUI>().SetRuneSprite(rune.GetComponent<Image>().sprite);
            conditionalRuneGO.GetComponent<ConditionalRuneUI>().SetConditionalRuneSprite(conditionalRuneSprite);
            conditionalRuneGO.GetComponent<ConditionalRuneUI>().conditionalMode = conditionalMode;
            conditionalRuneGO.GetComponent<ConditionalRuneUI>().runeName = runeName;
            conditionalRuneGO.GetComponent<ConditionalRuneUI>().conditionalRuneName = conditionalRuneName;
            conditionalRuneGO.GetComponent<ConditionalRuneUI>().programUI = this.gameObject;
        } else {
            GameObject runeGO = Instantiate(runePrefab, scrollContent.transform);
            runeGO.transform.SetSiblingIndex(insertRunePosition + 1);
            runeGO.GetComponent<RuneUI>().SetRuneSprite(rune.GetComponent<Image>().sprite);
            runeGO.GetComponent<RuneUI>().programUI = this.gameObject;
            runeGO.GetComponent<RuneUI>().runeName = runeName;
        }


        GameObject addRuneGO = Instantiate(addRunePrefab, scrollContent.transform);
        addRuneGO.transform.SetSiblingIndex(insertRunePosition + 2);
        addRuneGO.GetComponent<AddRuneUI>().SetAsInsertPosition();
        addRuneGO.GetComponent<AddRuneUI>().programUI = this.gameObject;

        insertRunePosition += 2;
        Destroy(rune);
    }

    public void SetInsertRuneAsConditional(Sprite sprite, String conditionalMode, String conditionalRuneName)
    {
        insertRuneAsConditional = true;
        conditionalRuneSprite = sprite;
        this.conditionalMode = conditionalMode;
        this.conditionalRuneName = conditionalRuneName;
    }

    public void UnsetInsertRuneAsConditional()
    {
        insertRuneAsConditional = false;
        this.conditionalMode = "";
        this.conditionalRuneName = "";
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
