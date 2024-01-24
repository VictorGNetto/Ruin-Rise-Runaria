using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Public Attacks Parameters
    public float A1_Dano = 1.0f;
    public float A1_Alcance = 1.0f;
    public float A1_Execucao = 0.5f;
    public float A1_Recuperacao = 0.5f;
    public float A1_Mana = 10.0f;

    public float A2_Dano = 0.75f;
    public float A2_Alcance = 1.5f;
    public float A2_Execucao = 0.5f;
    public float A2_Recuperacao = 0.5f;
    public float A2_Mana = 15.0f;

    public float A3_Dano = 1.25f;
    public float A3_Alcance = 1.0f;
    public float A3_Execucao = 1.0f;
    public float A3_Recuperacao = 0.5f;
    public float A3_Mana = 20.0f;

    public float A4_Dano = 1.25f;
    public float A4_Alcance = 1.5f;
    public float A4_Execucao = 1.5f;
    public float A4_Recuperacao = 0.5f;
    public float A4_Mana = 40.0f;

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

        golem.runeFunctionMap.Add("A1", new Golem.RuneFunction(A1));
        golem.setupFunctionMap.Add("A1", new Golem.SetupBeforeAction(A1Setup));
        golem.cleanUpFunctionMap.Add("A1", new Golem.CleanUpAfterAction(A1CleanUp));

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

    // A1
    private bool A1()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

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
        }

        return true;
    }

    private void A1Setup()
    {
        float manaCost = A1_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A1_Execucao + A1_Recuperacao;
            floatDict.Add("damage", golem.strength * A1_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.meleeRange * A1_Alcance);

            golem.speed = golem.baseSpeed * 0.25f;
            float attackDelay = 1.0f;
            golem.animator.speed = attackDelay / A1_Execucao;

            golem.attacking = true;
            DoA1Damage(A1_Execucao);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A1_Recuperacao;
        }
    }

    private void A1CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    void DoA1Damage(float delayTime)
    {
        StartCoroutine(A1Damage(delayTime));
    }

    IEnumerator A1Damage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        golem.speed = golem.baseSpeed * 0.75f;
        golem.animator.speed = 1;
        golem.attacking = false;

        if (boolDict["success"]) {
            golem.targetFriend.TakeDamage(floatDict["damage"]);
        }
    }

    // A2
    private bool A2()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

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
        }

        return true;
    }

    private void A2Setup()
    {
        float manaCost = A2_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A2_Execucao + A2_Recuperacao;
            floatDict.Add("damage", golem.strength * A2_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.meleeRange * A2_Alcance);

            golem.speed = golem.baseSpeed * 0.25f;
            float attackDelay = 1.0f;
            golem.animator.speed = attackDelay / A2_Execucao;

            golem.attacking = true;
            DoA2Damage(A2_Execucao);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A2_Recuperacao;
        }
    }

    private void A2CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    void DoA2Damage(float delayTime)
    {
        StartCoroutine(A2Damage(delayTime));
    }

    IEnumerator A2Damage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        golem.speed = golem.baseSpeed * 0.75f;
        golem.animator.speed = 1;
        golem.attacking = false;

        if (boolDict["success"]) {
            golem.targetFriend.TakeDamage(floatDict["damage"]);
        }
    }

    // A3
    private bool A3()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

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
        }

        return true;
    }

    private void A3Setup()
    {
        float manaCost = A3_Mana;

        floatDict.Clear();
        boolDict.Clear();
        intDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A3_Execucao + A3_Recuperacao;
            floatDict.Add("damage", golem.strength * A3_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.meleeRange * A3_Alcance);
            floatDict.Add("playsCount", 2);
            intDict.Add("hits", 2);


            golem.speed = golem.baseSpeed * 0.25f;
            float attackDelay = 1.0f;
            golem.animator.speed = floatDict["playsCount"] * attackDelay / A3_Execucao;

            golem.attacking = true;
            DoA3Damage(A3_Execucao / floatDict["playsCount"]);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A3_Recuperacao;
        }
    }

    private void A3CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    void DoA3Damage(float delayTime)
    {
        StartCoroutine(A3Damage(delayTime));
    }

    IEnumerator A3Damage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        intDict["hits"] -= 1;

        if (boolDict["success"]) {
            golem.targetFriend.TakeDamage(floatDict["damage"]);
            boolDict["success"] = false;
        }

        if (intDict["hits"] == 0) {
            //Do the action after the delay time has finished.
            golem.speed = golem.baseSpeed * 0.75f;
            golem.animator.speed = 1;
            golem.attacking = false;
        } else {
            DoA3Damage(A3_Execucao / floatDict["playsCount"]);
        }
    }

    // A4
    private bool A4()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

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
        }

        return true;
    }

    private void A4Setup()
    {
        float manaCost = A4_Mana;

        floatDict.Clear();
        boolDict.Clear();
        intDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A4_Execucao + A4_Recuperacao;
            floatDict.Add("damage", golem.strength * A4_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.meleeRange * A4_Alcance);
            floatDict.Add("playsCount", 3);
            intDict.Add("hits", 3);


            golem.speed = golem.baseSpeed * 0.25f;
            float attackDelay = 1.0f;
            golem.animator.speed = floatDict["playsCount"] * attackDelay / A4_Execucao;

            golem.attacking = true;
            DoA4Damage(A4_Execucao / floatDict["playsCount"]);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A4_Recuperacao;
        }
    }

    private void A4CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    void DoA4Damage(float delayTime)
    {
        StartCoroutine(A4Damage(delayTime));
    }

    IEnumerator A4Damage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime + 0.1f);

        intDict["hits"] -= 1;

        if (boolDict["success"]) {
            golem.targetFriend.TakeDamage(floatDict["damage"]);
            boolDict["success"] = false;
        }

        if (intDict["hits"] == 0) {
            //Do the action after the delay time has finished.
            golem.speed = golem.baseSpeed * 0.75f;
            golem.animator.speed = 1;
            golem.attacking = false;
        } else {
            DoA4Damage(A3_Execucao / floatDict["playsCount"]);
        }
    }

    // // A4
    // private bool A4()
    // {
    //     if (!golem.runeExecuted) return true;

    //     float distance;
    //     for (int i = 0; i < golem.levelDirector.golems.Count; i++) {
    //         if (auxGolemArray[i] == null) continue;

    //         Vector3 position = auxGolemArray[i].transform.position;
    //         distance = (golem.transform.position - position).magnitude;

    //         if (distance < floatDict["attackRange"]) {
    //             auxGolemArray[i].TakeDamage(floatDict["damage"]);
    //             auxGolemArray[i] = null;
    //         }
    //     }

    //     return true;
    // }

    // private void A4Setup()
    // {
    //     float manaCost = 15.0f;

    //     floatDict.Clear();
    //     boolDict.Clear();
    //     auxGolemArray = golem.levelDirector.golems.ToArray();
    //     auxGolemArray[golem.guid] = null;

    //     if (manaCost <= golem.mana) {
    //         golem.mana -= manaCost;
    //         golem.runeExecuted = true;
    //         golem.cooldown = 0.5f;
    //         floatDict.Add("damage", golem.strength * 0.25f);
    //         floatDict.Add("attackRange", 2f);

    //         golem.gameObject.GetComponent<Animator>().SetTrigger("Attack");
    //         GameObject go = Instantiate(explosionEffectPrefab);
    //         go.transform.position = golem.transform.position;
    //     } else {
    //         golem.runeExecuted = false;
    //         golem.cooldown = 0.5f;
    //     }
    // }

    // private void A4CleanUp()
    // {
    //     golem.gameObject.GetComponent<Animator>().SetTrigger("Idle");
    // }
}
