using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    bool targetChanged = false;
    ICharacter oldTarget = null;
    // Golem oldFriendTarget = null;
    int oldTargetGuid = -1;

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("L1-5", new Golem.RuneFunction(L15));
        golem.setupFunctionMap.Add("L1-5", new Golem.SetupBeforeAction(L15Setup));
        golem.cleanUpFunctionMap.Add("L1-5", new Golem.CleanUpAfterAction(L15CleanUp));

        golem.runeFunctionMap.Add("L1-6", new Golem.RuneFunction(L16));
        golem.setupFunctionMap.Add("L1-6", new Golem.SetupBeforeAction(L16Setup));
        golem.cleanUpFunctionMap.Add("L1-6", new Golem.CleanUpAfterAction(L16CleanUp));

        golem.runeFunctionMap.Add("L1-7", new Golem.RuneFunction(L17));
        golem.setupFunctionMap.Add("L1-7", new Golem.SetupBeforeAction(L17Setup));
        golem.cleanUpFunctionMap.Add("L1-7", new Golem.CleanUpAfterAction(L17CleanUp));

        golem.runeFunctionMap.Add("L1-8", new Golem.RuneFunction(L18));
        golem.setupFunctionMap.Add("L1-8", new Golem.SetupBeforeAction(L18Setup));
        golem.cleanUpFunctionMap.Add("L1-8", new Golem.CleanUpAfterAction(L18CleanUp));

        golem.runeFunctionMap.Add("L1-9", new Golem.RuneFunction(L19));
        golem.setupFunctionMap.Add("L1-9", new Golem.SetupBeforeAction(L19Setup));
        golem.cleanUpFunctionMap.Add("L1-9", new Golem.CleanUpAfterAction(L19CleanUp));
    }

    private void UpdateSelecUI()
    {
        // Hide the old target UI
        if (oldTarget != null) {
            oldTarget.SelectedAndTargetUI().Hide();
        }

        // Play the new target UI
        if (golem.targetType == Golem.TargetType.Self) {
            golem.selectedAndTargetUI.PlayAutoTarget();
        } else if (golem.targetType == Golem.TargetType.Friend) {
            golem.selectedAndTargetUI.PlaySelected();
            golem.target.SelectedAndTargetUI().PlayFriendTarget();
        } else {
            golem.selectedAndTargetUI.PlaySelected();
            golem.target.SelectedAndTargetUI().PlayEnemyTarget();
        }
    }

    // L15
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
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetGolemWithHighestHealth();

        if (golem.guid == golem.target.GUID()) {
            golem.targetType = Golem.TargetType.Self;
        } else {
            golem.targetType = Golem.TargetType.Friend;
        }

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L16
    private bool L16()
    {
        // do nothing

        return true;
    }

    private void L16Setup()
    {
        golem.cooldown = 1.5f;
        golem.runeExecuted = true;
    }

    private void L16CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetGolemWithLowestHealth();

        if (golem.guid == golem.target.GUID()) {
            golem.targetType = Golem.TargetType.Self;
        } else {
            golem.targetType = Golem.TargetType.Friend;
        }

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L17
    private bool L17()
    {
        // do nothing

        return true;
    }

    private void L17Setup()
    {
        golem.cooldown = 1.5f;
        golem.runeExecuted = true;
    }

    private void L17CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetNearestGolem(golem.guid);

        if (golem.guid == golem.target.GUID()) {
            golem.targetType = Golem.TargetType.Self;
        } else {
            golem.targetType = Golem.TargetType.Friend;
        }

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L18
    private bool L18()
    {
        // do nothing

        return true;
    }

    private void L18Setup()
    {
        golem.cooldown = 1.5f;
        golem.runeExecuted = true;
    }

    private void L18CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetFartestGolem(golem.guid);

        if (golem.guid == golem.target.GUID()) {
            golem.targetType = Golem.TargetType.Self;
        } else {
            golem.targetType = Golem.TargetType.Friend;
        }

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L19
    private bool L19()
    {
        // do nothing

        return true;
    }

    private void L19Setup()
    {
        golem.cooldown = 1.5f;
        golem.runeExecuted = true;
    }

    private void L19CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem;
        golem.targetType = Golem.TargetType.Self;

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }
}
