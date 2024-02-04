using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    // Public Attacks Parameters
    public float A8_Dano = 1.0f;
    public float A8_Alcance = 1.0f;
    public float A8_Execucao = 1.0f;
    public float A8_Recuperacao = 0.5f;
    public float A8_Mana = 15.0f;

    public Transform launchOffset;
    public GameObject runicSpellPrefab;

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
}
