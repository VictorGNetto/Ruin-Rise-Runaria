using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    private Golem golem;

    // Public Attacks Parameters
    public float S1_Cura = 0.15f;
    public float S1_Alcance = 1.0f;
    public float S1_Execucao = 1.5f;
    public float S1_Recuperacao = 0.5f;
    public float S1_Mana = 35.0f;

    public float S2_Cura = 0.40f;
    public float S2_Alcance = 1.0f;
    public float S2_Execucao = 2.0f;
    public float S2_Recuperacao = 0.5f;
    public float S2_Mana = 50.0f;

    public float S3_Cura = 0.60f;
    public float S3_Alcance = 1.0f;
    public float S3_Execucao = 2.0f;
    public float S3_Recuperacao = 0.5f;
    public float S3_Mana = 70.0f;

    public GameObject HealPrefab;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, int> intDict = new Dictionary<string, int>();
    private Dictionary<String, bool> boolDict = new Dictionary<string, bool>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("S1", new Golem.RuneFunction(S1));
        golem.setupFunctionMap.Add("S1", new Golem.SetupBeforeAction(S1Setup));
        golem.cleanUpFunctionMap.Add("S1", new Golem.CleanUpAfterAction(S1CleanUp));

        golem.runeFunctionMap.Add("S2", new Golem.RuneFunction(S2));
        golem.setupFunctionMap.Add("S2", new Golem.SetupBeforeAction(S2Setup));
        golem.cleanUpFunctionMap.Add("S2", new Golem.CleanUpAfterAction(S2CleanUp));

        golem.runeFunctionMap.Add("S3", new Golem.RuneFunction(S3));
        golem.setupFunctionMap.Add("S3", new Golem.SetupBeforeAction(S3Setup));
        golem.cleanUpFunctionMap.Add("S3", new Golem.CleanUpAfterAction(S3CleanUp));
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

    // S1
    private bool S1()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(HealPrefab);
            healEffect.GetComponent<Heal>().target = golem.target;
        }

        return true;
    }

    private void S1Setup()
    {
        float manaCost = S1_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", S1_Cura * golem.target.MaxHealth());
            floatDict.Add("range", golem.basicRange + golem.distanceRange * S1_Alcance);
            golem.cooldown =  S1_Execucao + S1_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S1_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S1CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }

    // S2
    private bool S2()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(HealPrefab);
            healEffect.GetComponent<Heal>().target = golem.target;
        }

        return true;
    }

    private void S2Setup()
    {
        float manaCost = S2_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", S2_Cura * golem.target.MaxHealth());
            floatDict.Add("range", golem.basicRange + golem.distanceRange * S2_Alcance);
            golem.cooldown =  S2_Execucao + S2_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S2_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S2CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }

    // S3
    private bool S3()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(HealPrefab);
            healEffect.GetComponent<Heal>().target = golem.target;
        }

        return true;
    }

    private void S3Setup()
    {
        float manaCost = S3_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", S3_Cura * golem.target.MaxHealth());
            floatDict.Add("range", golem.basicRange + golem.distanceRange * S3_Alcance);
            golem.cooldown =  S3_Execucao + S3_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S3_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S3CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }
}
