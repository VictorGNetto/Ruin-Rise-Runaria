using System;
using UnityEngine;

public class SelecGolem : MonoBehaviour
{
    public Golem golem;
    public RuneSelectionUI runeSelectionUI;

    public void UnselectGolem()
    {
        golem.selectedAndTargetUI.Hide();

        if (golem.targetType == Golem.TargetType.Friend) {
            if (golem.targetFriend != null) {
                golem.targetFriend.selectedAndTargetUI.Hide();
            }
        } else {

        }
    }

    public void SelectGolem()
    {
        golem.selectedAndTargetUI.PlaySelected();

        if (golem.targetType == Golem.TargetType.Friend) {
            if (golem.targetFriend != null) {
                golem.targetFriend.selectedAndTargetUI.PlayFriendTarget();
            }
        } else {

        }
    }

    public void OpenRuneSelectionUI()
    {
        runeSelectionUI.OpenRuneSelectionUI();
    }
}
