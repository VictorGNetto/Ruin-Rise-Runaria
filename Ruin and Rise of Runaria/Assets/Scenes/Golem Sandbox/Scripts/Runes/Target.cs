using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("L1-5", new Golem.RuneFunction(L15));
        golem.setupFunctionMap.Add("L1-5", new Golem.SetupBeforeAction(L15Setup));
        golem.cleanUpFunctionMap.Add("L1-5", new Golem.CleanUpAfterAction(L15CleanUp));
    }

    private bool L15()
    {
        // do nothing

        return true;
    }

    private void L15Setup()
    {
        golem.cooldown = 1.5f;
        golem.runeExecuted = true;
    }

    private void L15CleanUp()
    {
        bool targetChanged = false;
        Golem oldFriendTarget = golem.targetFriend;
        int oldTargetGuid = oldFriendTarget.guid;

        golem.targetFriend = golem.levelDirector.GetGolemWithHighestHealth();
        if (golem.guid == golem.targetFriend.guid) {
            golem.targetType = Golem.TargetType.Self;
        } else {
            golem.targetType = Golem.TargetType.Friend;
        }

        if (oldTargetGuid != golem.targetFriend.guid) targetChanged = true;

        if (!targetChanged || !golem.selected) return;

        // Update Select UI
        // Hide the old target UI
        if (oldFriendTarget != null) {
            oldFriendTarget.selectedAndTargetUI.Hide();
        }

        // Play the new target UI
        if (golem.targetType == Golem.TargetType.Self) {
            golem.targetFriend.selectedAndTargetUI.PlayAutoTarget();
        } else if (golem.targetType == Golem.TargetType.Friend) {
            golem.selectedAndTargetUI.PlaySelected();
            golem.targetFriend.selectedAndTargetUI.PlayFriendTarget();
        } else {
            golem.selectedAndTargetUI.PlaySelected();
            // golem.targetEnemy.selectedAndTargetUI.PlayTarget();
        }
    }
}
