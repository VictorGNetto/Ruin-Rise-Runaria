using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("RangedAttack", new Golem.RuneFunction(RangedAttack));
        golem.setupFunctionMap.Add("RangedAttack", new Golem.SetupBeforeAction(RangedAttackSetup));
    }

    private bool RangedAttack()
    {
        golem.cooldown = floatDict["cooldown"];

        if (intDict["bulletCount"] > 0) {
            intDict["bulletCount"] = intDict["bulletCount"] - 1;

            Bullet bullet = Instantiate(golem.bulletPrefab, golem.launchOffset.position, transform.rotation);
            bullet.target = golem.target;
            bullet.damage = floatDict["damage"];
        }

        return true;
    }

    private void RangedAttackSetup()
    {
        float manaCost = 15.0f;

        floatDict.Clear();
        intDict.Clear();

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("damage", 15.0f);
            intDict.Add("bulletCount", 1);
            floatDict.Add("cooldown", 3.0f);
        } else {
            floatDict.Add("damage", 0.0f);
            intDict.Add("bulletCount", 0);
            floatDict.Add("cooldown", 0.5f);
        }
    }
}
