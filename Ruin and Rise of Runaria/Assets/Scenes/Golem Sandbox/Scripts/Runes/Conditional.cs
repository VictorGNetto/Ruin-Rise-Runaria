using System;
using System.Collections.Generic;
using UnityEngine;

public class Conditional : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("C1", new Golem.RuneFunction(HealthAbove75Percent));
        golem.setupFunctionMap.Add("C1", new Golem.SetupBeforeAction(HealthAbove75PercentSetup));

        golem.runeFunctionMap.Add("C2", new Golem.RuneFunction(ConditionalDebug));
        golem.runeFunctionMap.Add("C3", new Golem.RuneFunction(ConditionalDebug));
    }

    private bool ConditionalDebug()
    {
        bool b = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        Debug.Log(b);
        golem.cooldown = 0;

        return b;
    }

    private bool HealthAbove75Percent()
    {
        return golem.health / golem.maxHealth > 0.75f;
    }

    private void HealthAbove75PercentSetup()
    {
        // float manaCost = 5.0f;
        golem.cooldown = 1.0f;

        // if (manaCost <= golem.mana) {
        // } else {
        // }
    }
}