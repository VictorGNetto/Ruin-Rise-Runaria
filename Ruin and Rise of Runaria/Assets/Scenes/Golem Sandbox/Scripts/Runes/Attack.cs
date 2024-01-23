using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject explosionEffectPrefab;

    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, int> intDict = new Dictionary<string, int>();
    private Dictionary<String, bool> boolDict = new Dictionary<string, bool>();
    
    private Golem[] auxGolemArray;

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        // golem.runeFunctionMap.Add("RangedAttack", new Golem.RuneFunction(RangedAttack));
        // golem.setupFunctionMap.Add("RangedAttack", new Golem.SetupBeforeAction(RangedAttackSetup));

        golem.runeFunctionMap.Add("A2", new Golem.RuneFunction(A2));
        golem.setupFunctionMap.Add("A2", new Golem.SetupBeforeAction(A2Setup));
        golem.cleanUpFunctionMap.Add("A2", new Golem.CleanUpAfterAction(A2CleanUp));

        golem.runeFunctionMap.Add("A3", new Golem.RuneFunction(A3));
        golem.setupFunctionMap.Add("A3", new Golem.SetupBeforeAction(A3Setup));
        golem.cleanUpFunctionMap.Add("A3", new Golem.CleanUpAfterAction(A3CleanUp));

        golem.runeFunctionMap.Add("A4", new Golem.RuneFunction(A4));
        golem.setupFunctionMap.Add("A4", new Golem.SetupBeforeAction(A4Setup));
        golem.cleanUpFunctionMap.Add("A4", new Golem.CleanUpAfterAction(A4CleanUp));
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

    void DoTakeDamageDelay(float delayTime)
    {
        StartCoroutine(DelayTakeDamage(delayTime));
    }

    IEnumerator DelayTakeDamage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        golem.targetFriend.TakeDamage(floatDict["damage"]);
    }

    // A2
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
                if (!golem.targetFriend.alive) return true;
                
                golem.gameObject.GetComponent<Animator>().SetTrigger("Attack");
                DoTakeDamageDelay(0.5f);
                // golem.targetFriend.TakeDamage(floatDict["damage"]);
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

    // A3
    private bool A3()
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
                if (!golem.targetFriend.alive) return true;
                
                golem.gameObject.GetComponent<Animator>().SetTrigger("Attack");
                DoTakeDamageDelay(0.5f);
                // golem.targetFriend.TakeDamage(floatDict["damage"]);
            }
        }

        return true;
    }

    private void A3Setup()
    {
        float manaCost = 5.0f;

        floatDict.Clear();
        boolDict.Clear();

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = 1.0f;
            floatDict.Add("damage", golem.strength * 0.5f);
            floatDict.Add("attackRange", 2f);
            boolDict.Add("success", false);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = 0.5f;
        }
    }

    private void A3CleanUp()
    {
        if (boolDict["success"]) {
            golem.gameObject.GetComponent<Animator>().SetTrigger("Idle");
        }
    }

    // A4
    private bool A4()
    {
        if (!golem.runeExecuted) return true;

        float distance;
        for (int i = 0; i < golem.levelDirector.golems.Count; i++) {
            if (auxGolemArray[i] == null) continue;

            Vector3 position = auxGolemArray[i].transform.position;
            distance = (golem.transform.position - position).magnitude;

            if (distance < floatDict["attackRange"]) {
                auxGolemArray[i].TakeDamage(floatDict["damage"]);
                auxGolemArray[i] = null;
            }
        }

        return true;
    }

    private void A4Setup()
    {
        float manaCost = 15.0f;

        floatDict.Clear();
        boolDict.Clear();
        auxGolemArray = golem.levelDirector.golems.ToArray();
        auxGolemArray[golem.guid] = null;

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = 0.5f;
            floatDict.Add("damage", golem.strength * 0.25f);
            floatDict.Add("attackRange", 2f);

            golem.gameObject.GetComponent<Animator>().SetTrigger("Attack");
            GameObject go = Instantiate(explosionEffectPrefab);
            go.transform.position = golem.transform.position;
        } else {
            golem.runeExecuted = false;
            golem.cooldown = 0.5f;
        }
    }

    private void A4CleanUp()
    {
        golem.gameObject.GetComponent<Animator>().SetTrigger("Idle");
    }
}
