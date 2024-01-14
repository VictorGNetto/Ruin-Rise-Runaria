using System;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("Heal", new Golem.RuneFunction(Heal));
        golem.setupFunctionMap.Add("Heal", new Golem.SetupBeforeAction(HealSetup));
        golem.cleanUpFunctionMap.Add("Heal", new Golem.CleanUpAfterAction(HealCleanUp));
    }

    private bool Heal()
    {
        golem.cooldown = floatDict["cooldown"];

        float totalHeal = floatDict["totalHeal"];
        float amount = totalHeal * Time.deltaTime / golem.cooldown;
        golem.health = Math.Min(golem.health + amount, golem.maxHealth);

        return true;
    }

    private void HealSetup()
    {
        float manaCost = 20.0f;

        floatDict.Clear();

        floatDict.Add("health", golem.health);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", 25.0f);
            floatDict.Add("cooldown", 1.5f);
        } else {
            floatDict.Add("totalHeal", 0.0f);
            floatDict.Add("cooldown", 0.5f);
        }
    }

    private void HealCleanUp()
    {
        golem.health = floatDict["health"] + floatDict["totalHeal"];
    }
}
