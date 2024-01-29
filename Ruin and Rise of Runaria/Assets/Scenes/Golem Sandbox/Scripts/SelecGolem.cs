using System;
using UnityEngine;

public class SelecGolem : MonoBehaviour
{
    public Golem golem;

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();
    }

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
