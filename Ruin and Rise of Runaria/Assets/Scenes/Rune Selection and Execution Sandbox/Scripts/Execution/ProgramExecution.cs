using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramExecution : MonoBehaviour
{
    public GameObject runeSelection;

    public void ShowRuneSelectionUI()
    {
        runeSelection.SetActive(true);
    }
}
