using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, int> intDict = new Dictionary<string, int>();
    private Dictionary<String, bool> boolDict = new Dictionary<string, bool>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        // golem.runeFunctionMap.Add("RangedAttack", new Golem.RuneFunction(RangedAttack));
        // golem.setupFunctionMap.Add("RangedAttack", new Golem.SetupBeforeAction(RangedAttackSetup));

        golem.runeFunctionMap.Add("A2", new Golem.RuneFunction(A2));
        golem.setupFunctionMap.Add("A2", new Golem.SetupBeforeAction(A2Setup));
        golem.cleanUpFunctionMap.Add("A2", new Golem.CleanUpAfterAction(A2CleanUp));
    }

    // private bool RangedAttack()
    // {
    //     golem.cooldown = floatDict["cooldown"];

    //     if (intDict["bulletCount"] > 0) {
    //         intDict["bulletCount"] = intDict["bulletCount"] - 1;

    //         Bullet bullet = Instantiate(golem.bulletPrefab, golem.launchOffset.position, transform.rotation);
    //         bullet.target = golem.targetEnemy;
    //         bullet.damage = floatDict["damage"];
    //         bullet.speed = UnityEngine.Random.Range(10, 15);
    //     }

    //     return true;
    // }

    // private void RangedAttackSetup()
    // {
    //     float manaCost = 15.0f;

    //     floatDict.Clear();
    //     intDict.Clear();

    //     if (manaCost <= golem.mana) {
    //         golem.mana -= manaCost;
    //         floatDict.Add("damage", 15.0f);
    //         intDict.Add("bulletCount", 1);
    //         floatDict.Add("cooldown", 3.0f);
    //     } else {
    //         floatDict.Add("damage", 0.0f);
    //         intDict.Add("bulletCount", 0);
    //         floatDict.Add("cooldown", 0.5f);
    //     }
    // }

    private bool A2()
    {
        if (boolDict["success"] || !golem.runeExecuted) return true;

        float distance;
        if (golem.targetType == Golem.TargetType.Enemy) {
            Vector3 enemyPosition = golem.targetEnemy.transform.position;
            distance = (golem.transform.position - enemyPosition).magnitude;
        } else {
            Vector3 friendPosition = golem.targetFriend.transform.position;
            distance = (golem.transform.position - friendPosition).magnitude;
        }

        if (distance < floatDict["attackRange"]) {
            boolDict["success"] = true;

            if (golem.targetType == Golem.TargetType.Enemy) {
                // golem.targetEnemy.TakeDamage(floatDict["damage"]);
            } else if (golem.targetType == Golem.TargetType.Friend) {
                golem.gameObject.GetComponent<Animator>().SetTrigger("Attack");
                golem.targetFriend.TakeDamage(floatDict["damage"]);
            }
        }

        return true;
    }

    private void A2Setup()
    {
        float manaCost = 5.0f;

        floatDict.Clear();
        boolDict.Clear();

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = 1.0f;
            // golem.gameObject.GetComponent<Animator>().SetTrigger("Attack");
            floatDict.Add("damage", golem.strength * 0.75f);
            floatDict.Add("attackRange", 1f);
            boolDict.Add("success", false);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = 0.5f;
        }
    }

    private void A2CleanUp()
    {
        if (boolDict["success"]) {
            golem.gameObject.GetComponent<Animator>().SetTrigger("Idle");
        }
    }
}
