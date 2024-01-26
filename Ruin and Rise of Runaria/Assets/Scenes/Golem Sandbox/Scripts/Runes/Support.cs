using System;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    private Golem golem;

    // Public Attacks Parameters
    public float S1_Cura = 0.25f;
    public float S1_Alcance = 1.0f;
    public float S1_Execucao = 1.5f;
    public float S1_Recuperacao = 0.5f;
    public float S1_Mana = 35.0f;

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

        golem.runeFunctionMap.Add("HealStronger", new Golem.RuneFunction(Heal));
        golem.setupFunctionMap.Add("HealStronger", new Golem.SetupBeforeAction(HealStrongerSetup));
        golem.cleanUpFunctionMap.Add("HealStronger", new Golem.CleanUpAfterAction(HealCleanUp));
    }

    private bool S1()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;
            golem.casting = true;

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
            golem.speed = golem.baseSpeed * 0.25f;
            golem.animator.speed = 1;
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

    private bool Heal()
    {
        float totalHeal = floatDict["totalHeal"];
        float amount = totalHeal * Time.deltaTime / golem.cooldown;
        golem.health = Math.Min(golem.health + amount, golem.maxHealth);

        return true;
    }

    private void HealSetup()
    {
        float manaCost = 35.0f;

        floatDict.Clear();

        floatDict.Add("health", golem.health);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", 0.15f * golem.maxHealth);
            golem.cooldown =  4f;
            golem.runeExecuted = true;
        } else {
            floatDict.Add("totalHeal", 0.0f);
            golem.cooldown =  0.4f;
            golem.runeExecuted = false;
        }
    }

    private void HealCleanUp()
    {
        golem.health = floatDict["health"] + floatDict["totalHeal"];
    }

    private void HealStrongerSetup()
    {
        float manaCost = 50.0f;

        floatDict.Clear();

        floatDict.Add("health", golem.health);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", 0.25f * golem.maxHealth);
            golem.cooldown =  5f;
        } else {
            floatDict.Add("totalHeal", 0.0f);
            golem.cooldown =  1f;
        }
    }
}
