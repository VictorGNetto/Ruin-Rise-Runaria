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

        golem.runeFunctionMap.Add("L1-1", new Golem.RuneFunction(L11));
        golem.setupFunctionMap.Add("L1-1", new Golem.SetupBeforeAction(L11Setup));
        golem.cleanUpFunctionMap.Add("L1-1", new Golem.CleanUpAfterAction(L11CleanUp));

        golem.runeFunctionMap.Add("L1-2", new Golem.RuneFunction(L12));
        golem.setupFunctionMap.Add("L1-2", new Golem.SetupBeforeAction(L12Setup));
        golem.cleanUpFunctionMap.Add("L1-2", new Golem.CleanUpAfterAction(L12CleanUp));

        golem.runeFunctionMap.Add("L1-3", new Golem.RuneFunction(L13));
        golem.setupFunctionMap.Add("L1-3", new Golem.SetupBeforeAction(L13Setup));
        golem.cleanUpFunctionMap.Add("L1-3", new Golem.CleanUpAfterAction(L13CleanUp));

        golem.runeFunctionMap.Add("L1-4", new Golem.RuneFunction(L14));
        golem.setupFunctionMap.Add("L1-4", new Golem.SetupBeforeAction(L14Setup));
        golem.cleanUpFunctionMap.Add("L1-4", new Golem.CleanUpAfterAction(L14CleanUp));

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

    private void GoToSafeTarget()
    {
        if (golem.target == null) {
            golem.target = golem;
            golem.targetType = Golem.TargetType.Self;
            golem.targetBias = Golem.TargetType.Friend;
        }
    }

    // L11
    private bool L11()
    {
        // do nothing

        return true;
    }

    const float cooldown_value = 0.5f;

    private void L11Setup()
    {
        golem.targetBias = Golem.TargetType.Enemy;
        golem.cooldown = cooldown_value;
        golem.runeExecuted = true;
    }

    private void L11CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetEnemyWithHighestHealth();
        golem.targetType = Golem.TargetType.Enemy;

        GoToSafeTarget();

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L12
    private bool L12()
    {
        // do nothing

        return true;
    }

    private void L12Setup()
    {
        golem.targetBias = Golem.TargetType.Enemy;
        golem.cooldown = cooldown_value;
        golem.runeExecuted = true;
    }

    private void L12CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetEnemyWithLowestHealth();
        golem.targetType = Golem.TargetType.Enemy;

        GoToSafeTarget();

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L13
    private bool L13()
    {
        // do nothing

        return true;
    }

    private void L13Setup()
    {
        golem.targetBias = Golem.TargetType.Enemy;
        golem.cooldown = cooldown_value;
        golem.runeExecuted = true;
    }

    private void L13CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetNearestEnemy(golem.guid);
        golem.targetType = Golem.TargetType.Enemy;

        GoToSafeTarget();

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L14
    private bool L14()
    {
        // do nothing

        return true;
    }

    private void L14Setup()
    {
        golem.targetBias = Golem.TargetType.Enemy;
        golem.cooldown = cooldown_value;
        golem.runeExecuted = true;
    }

    private void L14CleanUp()
    {
        targetChanged = false;
        oldTarget = golem.target;
        oldTargetGuid = oldTarget.GUID();

        golem.target = golem.levelDirector.GetFartestEnemy(golem.guid);
        golem.targetType = Golem.TargetType.Enemy;

        GoToSafeTarget();

        if (oldTargetGuid != golem.target.GUID()) targetChanged = true;

        if (targetChanged && golem.selected) UpdateSelecUI();
    }

    // L15
    private bool L15()
    {
        // do nothing

        return true;
    }

    private void L15Setup()
    {
        golem.targetBias = Golem.TargetType.Friend;
        golem.cooldown = cooldown_value;
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
        golem.targetBias = Golem.TargetType.Friend;
        golem.cooldown = cooldown_value;
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
        golem.targetBias = Golem.TargetType.Friend;
        golem.cooldown = cooldown_value;
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
        golem.targetBias = Golem.TargetType.Friend;
        golem.cooldown = cooldown_value;
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
        golem.targetBias = Golem.TargetType.Self;
        golem.cooldown = cooldown_value;
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
