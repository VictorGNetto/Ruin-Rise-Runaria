using System;
using UnityEngine;

public class SelecArea : MonoBehaviour
{
    public Golem golem;

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
                golem.targetFriend.selectedAndTargetUI.PlayTarget();
            }
        } else {

        }
    }

    public void OpenRuneSelectionUI()
    {
        // golem.selectedAndTargetUI.PlayTarget();
    }
}
