using System;
using UnityEngine;

public class SelecGolem : MonoBehaviour
{
    public Golem golem;
    public RuneSelectionUI runeSelectionUI;

    public void UnselectGolem()
    {
        golem.Unselect();

    }

    public void SelectGolem()
    {
        golem.Select();
    }

    public void OpenRuneSelectionUI()
    {
        golem.OpenRuneSelectionUI();
    }
}
