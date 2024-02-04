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

    public float S4_Cura = 0.35f;
    public float S4_Alcance = 1.0f;
    public float S4_Execucao = 2.0f;
    public float S4_Recuperacao = 0.5f;
    public float S4_Mana = 40.0f;

    public float S5_Cura = 0.55f;
    public float S5_Alcance = 1.0f;
    public float S5_Execucao = 2.0f;
    public float S5_Recuperacao = 0.5f;
    public float S5_Mana = 55.0f;

    public float S6_Alcance = 1.0f;
    public float S6_Execucao = 1.0f;
    public float S6_Recuperacao = 0.5f;
    public float S6_Mana = 30.0f;

    public float S7_Execucao = 0.5f;
    public float S7_Recuperacao = 0.5f;
    public float S7_Mana = 65.0f;

    public float S8_Execucao = 0.5f;
    public float S8_Recuperacao = 0.5f;
    public float S8_Mana = 75.0f;

    public GameObject brightEffectPrefab;
    public GameObject agroEffectPrefab;
    public GameObject healerPrefab;
    public GameObject defenseBuffPrefab;
    public GameObject attackBuffPrefab;

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

        golem.runeFunctionMap.Add("S4", new Golem.RuneFunction(S4));
        golem.setupFunctionMap.Add("S4", new Golem.SetupBeforeAction(S4Setup));
        golem.cleanUpFunctionMap.Add("S4", new Golem.CleanUpAfterAction(S4CleanUp));

        golem.runeFunctionMap.Add("S5", new Golem.RuneFunction(S5));
        golem.setupFunctionMap.Add("S5", new Golem.SetupBeforeAction(S5Setup));
        golem.cleanUpFunctionMap.Add("S5", new Golem.CleanUpAfterAction(S5CleanUp));

        golem.runeFunctionMap.Add("S6", new Golem.RuneFunction(S6));
        golem.setupFunctionMap.Add("S6", new Golem.SetupBeforeAction(S6Setup));
        golem.cleanUpFunctionMap.Add("S6", new Golem.CleanUpAfterAction(S6CleanUp));

        golem.runeFunctionMap.Add("S7", new Golem.RuneFunction(S7));
        golem.setupFunctionMap.Add("S7", new Golem.SetupBeforeAction(S7Setup));
        golem.cleanUpFunctionMap.Add("S7", new Golem.CleanUpAfterAction(S7CleanUp));

        golem.runeFunctionMap.Add("S8", new Golem.RuneFunction(S8));
        golem.setupFunctionMap.Add("S8", new Golem.SetupBeforeAction(S8Setup));
        golem.cleanUpFunctionMap.Add("S8", new Golem.CleanUpAfterAction(S8CleanUp));
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
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Heal");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem.target);
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
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Heal");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem.target);
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
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            golem.target.Heal(floatDict["totalHeal"]);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Heal");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem.target);
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

    // S4
    private bool S4()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            GameObject healer = Instantiate(healerPrefab);
            healer.GetComponent<Healer>().Setup(golem.target, floatDict["totalHeal"], 4.0f);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Heal");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem.target);
        }

        return true;
    }

    private void S4Setup()
    {
        float manaCost = S4_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", S4_Cura * golem.target.MaxHealth());
            floatDict.Add("range", golem.basicRange + golem.distanceRange * S4_Alcance);
            golem.cooldown =  S4_Execucao + S4_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S4_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S4CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }

    // S5
    private bool S5()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["range"]) {
            boolDict["success"] = true;

            GameObject healer = Instantiate(healerPrefab);
            healer.GetComponent<Healer>().Setup(golem.target, floatDict["totalHeal"], 6.0f);
            golem.speed = golem.baseSpeed * 0.75f;

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Heal");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem.target);
        }

        return true;
    }

    private void S5Setup()
    {
        float manaCost = S5_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            floatDict.Add("totalHeal", S5_Cura * golem.target.MaxHealth());
            floatDict.Add("range", golem.basicRange + golem.distanceRange * S5_Alcance);
            golem.cooldown =  S5_Execucao + S5_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S5_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S5CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }

    // S6
    private bool S6()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;
        boolDict["success"] = true;
        
        List<Enemy> enemys =  golem.levelDirector.GetEnemysInsideCircle(golem.Position(), floatDict["range"]);
        foreach (Enemy e in enemys)
        {
            if (e != null && e.Alive()) {
                e.target = golem;
            }
        }

        return true;
    }

    private void S6Setup()
    {
        float manaCost = S6_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.cooldown =  S6_Execucao + S6_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.25f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
            floatDict.Add("range", golem.basicRange + S6_Alcance * golem.meleeRange);

            GameObject agro = Instantiate(agroEffectPrefab);
            agro.GetComponent<Agro>().SetTarget(golem);
        } else {
            golem.cooldown =  S6_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S6CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }

    // S7
    private bool S7()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

        if (golem.timeSinceLastAction > S7_Execucao) {
            boolDict["success"] = true;

            golem.speed = golem.baseSpeed * 0.75f;

            GameObject buff = Instantiate(defenseBuffPrefab);
            buff.GetComponent<DefenseBuff>().Setup(golem);

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Defense");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem);
        }

        return true;
    }

    private void S7Setup()
    {
        float manaCost = S7_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.cooldown =  S7_Execucao + S7_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.25f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S7_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S7CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }

    // S8
    private bool S8()
    {
        if (!golem.runeExecuted || boolDict["success"]) return true;

        if (golem.timeSinceLastAction > S8_Execucao) {
            boolDict["success"] = true;

            golem.speed = golem.baseSpeed * 0.75f;

            GameObject buff = Instantiate(attackBuffPrefab);
            buff.GetComponent<AttackBuff>().Setup(golem);

            GameObject healEffect = Instantiate(brightEffectPrefab);
            healEffect.GetComponent<BrightEffect>().SetAnimation("Attack");
            healEffect.GetComponent<BrightEffect>().SetTarget(golem);
        }

        return true;
    }

    private void S8Setup()
    {
        float manaCost = S8_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.cooldown =  S8_Execucao + S8_Recuperacao;
            golem.runeExecuted = true;
            golem.speed = golem.baseSpeed * 0.25f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.cooldown =  S8_Recuperacao;
            golem.runeExecuted = false;
        }
    }

    private void S8CleanUp()
    {
        golem.casting = false;
        golem.speed = golem.baseSpeed;
    }
}
