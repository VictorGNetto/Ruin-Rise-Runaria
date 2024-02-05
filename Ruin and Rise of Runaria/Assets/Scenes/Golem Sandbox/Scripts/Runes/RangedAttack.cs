using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    // Public Attacks Parameters
    public float A8_Dano = 1.0f;
    public float A8_Alcance = 1.0f;
    public float A8_Execucao = 1.0f;
    public float A8_Recuperacao = 0.5f;
    public float A8_Mana = 15.0f;

    public float A9_Dano = 2.0f;
    public float A9_Alcance = 1.5f;
    public float A9_Execucao = 1.5f;
    public float A9_Recuperacao = 0.5f;
    public float A9_Mana = 25.0f;
    public Color A9_Ray_Color;

    public float A10_Dano = 0.75f;
    public float A10_Alcance = 1.5f;
    public float A10_Execucao = 1.5f;
    public float A10_Recuperacao = 0.5f;
    public float A10_Mana = 30.0f;

    public float A11_Dano = 4.0f;
    public float A11_Alcance = 1.5f;
    public float A11_Execucao = 1.5f;
    public float A11_Recuperacao = 0.5f;
    public float A11_Mana = 50.0f;
    public Color A11_Ray_Color;

    public Transform launchOffset;
    public GameObject runicSpellPrefab;
    public GameObject rayPrefab;

    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, int> intDict = new Dictionary<string, int>();
    private Dictionary<String, bool> boolDict = new Dictionary<string, bool>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("A8", new Golem.RuneFunction(A8));
        golem.setupFunctionMap.Add("A8", new Golem.SetupBeforeAction(A8Setup));
        golem.cleanUpFunctionMap.Add("A8", new Golem.CleanUpAfterAction(A8CleanUp));

        golem.runeFunctionMap.Add("A9", new Golem.RuneFunction(A9));
        golem.setupFunctionMap.Add("A9", new Golem.SetupBeforeAction(A9Setup));
        golem.cleanUpFunctionMap.Add("A9", new Golem.CleanUpAfterAction(A9CleanUp));

        golem.runeFunctionMap.Add("A10", new Golem.RuneFunction(A10));
        golem.setupFunctionMap.Add("A10", new Golem.SetupBeforeAction(A10Setup));
        golem.cleanUpFunctionMap.Add("A10", new Golem.CleanUpAfterAction(A10CleanUp));

        golem.runeFunctionMap.Add("A11", new Golem.RuneFunction(A11));
        golem.setupFunctionMap.Add("A11", new Golem.SetupBeforeAction(A11Setup));
        golem.cleanUpFunctionMap.Add("A11", new Golem.CleanUpAfterAction(A11CleanUp));
    }

    void DoResetCastingAnimation(float delayTime)
    {
        StartCoroutine(ResetCastingAnimation(delayTime));
    }

    IEnumerator ResetCastingAnimation(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        golem.casting = false;
    }

    void DoThrowSpell(int hits, float delayTime)
    {
        StartCoroutine(ThrowSpell(hits, delayTime));
    }

    IEnumerator ThrowSpell(int hits, float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        Vector3 targetPosition;
        float distance = (golem.Position() - golem.TargetPosition()).magnitude;
        if (distance < floatDict["attackRange"]) {
            targetPosition = golem.TargetPosition();
        } else {
            targetPosition = golem.Position() + floatDict["attackRange"] * (golem.TargetPosition() - golem.Position()).normalized;
        }
        float actualDistance = (golem.Position() - targetPosition).magnitude;
        float flyingTime = 0.25f + 0.25f * actualDistance / floatDict["attackRange"];


        for (int i = 0; i < hits; i++) {
            GameObject runicSpell = Instantiate(runicSpellPrefab);
            runicSpell.GetComponent<RunicSpell>().damage = floatDict["damage"];
            runicSpell.GetComponent<RunicSpell>().whoThrow = golem.GUID();
            runicSpell.GetComponent<RunicSpell>().autoTarget = golem.GUID() == golem.target.GUID();
            runicSpell.GetComponent<RunicSpell>().Setup(flyingTime, launchOffset.position, targetPosition);
        }
    }

    void DoDelayedDamage(float delayTime)
    {
        StartCoroutine(DelayedDamage(delayTime));
    }

    IEnumerator DelayedDamage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        if (golem.targetType == Golem.TargetType.Self) {
            golem.target.TakeDamage(floatDict["damage"] * 0.3f);
        } else if (golem.targetType == Golem.TargetType.Friend) {
            golem.target.TakeDamage(floatDict["damage"] * 0.5f);
        } else {
            golem.target.TakeDamage(floatDict["damage"]);
        }
    }

    // A8
    private bool A8()
    {
        golem.LookToTheTarget();
        
        // if (!golem.runeExecuted || boolDict["success"]) return true;

        return true;
    }

    private void A8Setup()
    {
        float manaCost = A8_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A8_Execucao + A8_Recuperacao;
            floatDict.Add("damage", golem.strength * A8_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A8_Alcance);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
            DoThrowSpell(1, 1.0f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A8_Recuperacao;
        }
    }

    private void A8CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    // A9
    private bool A9()
    {
        golem.LookToTheTarget();
        if (golem.timeSinceLastAction < A9_Execucao * 0.25f) return true;
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["attackRange"]) {
            boolDict["success"] = true;

            GameObject ray = Instantiate(rayPrefab);
            Vector3 offset = new Vector3(0, 2.15f, 0);
            ray.transform.position = golem.TargetPosition() + offset;
            ray.GetComponent<Ray>().Setup(golem.GetTargetSortingOrder() + 1, A9_Ray_Color);

            DoDelayedDamage(8.0f / 24.0f);
            golem.speed = golem.baseSpeed * 0.75f;
        }

        return true;
    }

    private void A9Setup()
    {
        float manaCost = A9_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A9_Execucao + A9_Recuperacao;
            floatDict.Add("damage", golem.strength * A9_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A9_Alcance);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A9_Recuperacao;
        }
    }

    private void A9CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    // A10
    private bool A10()
    {
        golem.LookToTheTarget();
        
        // if (!golem.runeExecuted || boolDict["success"]) return true;

        return true;
    }

    private void A10Setup()
    {
        float manaCost = A10_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A10_Execucao + A10_Recuperacao;
            floatDict.Add("damage", golem.strength * A10_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A10_Alcance);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
            DoThrowSpell(3, 1.0f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A10_Recuperacao;
        }
    }

    private void A10CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    // A11
    private bool A11()
    {
        golem.LookToTheTarget();
        if (golem.timeSinceLastAction < A11_Execucao * 0.25f) return true;
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["attackRange"]) {
            boolDict["success"] = true;

            GameObject ray = Instantiate(rayPrefab);
            Vector3 offset = new Vector3(0, 2.15f, 0);
            ray.transform.position = golem.TargetPosition() + offset;
            ray.GetComponent<Ray>().Setup(golem.GetTargetSortingOrder() + 1, A11_Ray_Color);

            DoDelayedDamage(8.0f / 24.0f);
            golem.speed = golem.baseSpeed * 0.75f;
        }

        return true;
    }

    private void A11Setup()
    {
        float manaCost = A11_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A11_Execucao + A11_Recuperacao;
            floatDict.Add("damage", golem.strength * A11_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A11_Alcance);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A11_Recuperacao;
        }
    }

    private void A11CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }
}
