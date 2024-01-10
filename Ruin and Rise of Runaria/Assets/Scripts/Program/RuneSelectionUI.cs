using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSelectionUI : MonoBehaviour
{
    public ProgramUI programUI;
    public String runeName;

    public void Select()
    {
        GameObject duplicate = Instantiate(this.gameObject);
        programUI.InsertRune(duplicate, runeName);
    }
}
