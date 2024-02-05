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

    public float A5_Dano = 2.0f;
    public float A5_Area = 2.5f;
    public float A5_Execucao = 1.0f;
    public float A5_Recuperacao = 0.5f;
    public float A5_Mana = 20.0f;

    public float A6_Dano = 1.5f;
    public float A6_Alcance = 1.0f;
    public float A6_Area = 2.5f;
    public float A6_Execucao = 1.0f;
    public float A6_Recuperacao = 0.5f;
    public float A6_Mana = 20.0f;

    public float A7_Dano = 1.5f;
    public float A7_Alcance = 2.0f;
    public float A7_Area = 2.5f;
    public float A7_Execucao = 0.5f;
    public float A7_Recuperacao = 0.5f;
    public float A7_Mana = 40.0f;

    public GameObject clubHitingGroundPrefab;
    public GameObject hitAreaPrefab;
    public GameObject flyingClubPrefab;

    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, int> intDict = new Dictionary<string, int>();
    private Dictionary<String, bool> boolDict = new Dictionary<string, bool>();
    
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

        golem.runeFunctionMap.Add("A5", new Golem.RuneFunction(A5));
        golem.setupFunctionMap.Add("A5", new Golem.SetupBeforeAction(A5Setup));
        golem.cleanUpFunctionMap.Add("A5", new Golem.CleanUpAfterAction(A5CleanUp));

        golem.runeFunctionMap.Add("A6", new Golem.RuneFunction(A6));
        golem.setupFunctionMap.Add("A6", new Golem.SetupBeforeAction(A6Setup));
        golem.cleanUpFunctionMap.Add("A6", new Golem.CleanUpAfterAction(A6CleanUp));

        golem.runeFunctionMap.Add("A7", new Golem.RuneFunction(A7));
        golem.setupFunctionMap.Add("A7", new Golem.SetupBeforeAction(A7Setup));
        golem.cleanUpFunctionMap.Add("A7", new Golem.CleanUpAfterAction(A7CleanUp));
    }

    private void SafeTakeDamage()
    {
        if (golem.targetType == Golem.TargetType.Self) {
            golem.target.TakeDamage(floatDict["damage"] * 0.3f);
        } else if (golem.targetType == Golem.TargetType.Friend) {
            golem.target.TakeDamage(floatDict["damage"] * 0.5f);
        } else {
            golem.target.TakeDamage(floatDict["damage"]);
        }
    }

    // A1
    private bool A1()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

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
            SafeTakeDamage();
        }
    }

    // A2
    private bool A2()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

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
            SafeTakeDamage();
        }
    }

    // A3
    private bool A3()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

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
            SafeTakeDamage();
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
        golem.LookToTheTarget();
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

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
        yield return new WaitForSeconds(delayTime);

        intDict["hits"] -= 1;

        if (boolDict["success"]) {
            SafeTakeDamage();
            boolDict["success"] = false;
        }

        if (intDict["hits"] == 0) {
            //Do the action after the delay time has finished.
            golem.speed = golem.baseSpeed * 0.75f;
            golem.animator.speed = 1;
            golem.attacking = false;
        } else {
            DoA4Damage(A4_Execucao / floatDict["playsCount"]);
        }
    }

    // A5
    private bool A5()
    {
        golem.LookToTheTarget();

        return true;
    }

    private void A5Setup()
    {
        float manaCost = A5_Mana;

        floatDict.Clear();
        boolDict.Clear();

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A5_Execucao + A5_Recuperacao;
            floatDict.Add("damage", golem.strength * A5_Dano);
            floatDict.Add("area", golem.basicRange + golem.meleeRange * A5_Area);
            floatDict["centerX"] = golem.Position().x;
            floatDict["centerY"] = golem.Position().y;

            golem.speed = golem.baseSpeed * 0.25f;
            golem.attacking = true;
            golem.animator.speed = 2;
            DoA5Damage(0.5f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A5_Recuperacao;
        }
    }

    private void A5CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    void DoA5Damage(float delayTime)
    {
        StartCoroutine(A5Damage(delayTime));
    }

    IEnumerator A5Damage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        golem.attacking = false;
        golem.animator.speed = 1;
        golem.speed = golem.baseSpeed * 0.75f;

        GameObject hitArea = Instantiate(hitAreaPrefab);
        hitArea.transform.position = golem.Position();
        hitArea.GetComponent<CircleHitArea>().SetDamage(floatDict["damage"]);
        hitArea.GetComponent<CircleHitArea>().SetRadius(floatDict["area"]);
        hitArea.GetComponent<CircleHitArea>().DestroyHitArea(0.5f);
        hitArea.GetComponent<CircleHitArea>().AddNotHitableCharacter(golem.guid);

        GameObject effect = Instantiate(clubHitingGroundPrefab);
        Vector3 position = new Vector3(golem.Position().x, golem.Position().y + 0.3f, 0);
        effect.transform.position = position;
        effect.GetComponent<ClubHitingGround>().SetOrderInLayer(golem.GetSortingOrder() + 10);
    }

    // A6
    private bool A6()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted) return true;

        if (golem.timeSinceLastAction < 0.5f) {
            if (boolDict["successPhase1"]) return true;

            float distance;
            distance = (golem.Position() - golem.TargetPosition()).magnitude;

            if (distance < floatDict["attackRange"]) {
                boolDict["successPhase1"] = true;
            }
        } else if (golem.timeSinceLastAction < 1.0f) {
            if (boolDict["successPhase2"]) return true;

            boolDict["successPhase2"] = true;

            golem.speed = golem.baseSpeed * 0.75f;
            golem.animator.speed = 1;
            golem.attacking = false;

            if (boolDict["successPhase1"]) {
                GameObject hitArea = Instantiate(hitAreaPrefab);
                hitArea.transform.position = golem.TargetPosition();
                hitArea.GetComponent<CircleHitArea>().SetDamage(floatDict["damage"]);
                hitArea.GetComponent<CircleHitArea>().SetRadius(floatDict["area"]);
                hitArea.GetComponent<CircleHitArea>().DestroyHitArea(0.5f);
                hitArea.GetComponent<CircleHitArea>().AddNotHitableCharacter(golem.guid);

                GameObject effect = Instantiate(clubHitingGroundPrefab);
                Vector3 position = new Vector3(golem.TargetPosition().x, golem.TargetPosition().y + 0.3f, 0);
                effect.transform.position = position;
                effect.GetComponent<ClubHitingGround>().SetOrderInLayer(golem.GetTargetSortingOrder() + 10);
            }
        }

        return true;
    }

    private void A6Setup()
    {
        float manaCost = A6_Mana;

        floatDict.Clear();
        boolDict.Clear();
        intDict.Clear();
        boolDict.Add("successPhase1", false);
        boolDict.Add("successPhase2", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A6_Execucao + A6_Recuperacao;
            floatDict.Add("damage", golem.strength * A6_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.meleeRange * A6_Alcance);
            floatDict.Add("area", golem.basicRange + golem.meleeRange * A6_Area);

            golem.speed = golem.baseSpeed * 0.25f;
            golem.attacking = true;
            golem.animator.speed = 2;
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A6_Recuperacao;
        }
    }

    private void A6CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    // A7
    private bool A7()
    {
        golem.LookToTheTarget();
        if (!golem.runeExecuted) return true;

        if (golem.timeSinceLastAction < 0.5f) {
            if (boolDict["successPhase1"]) return true;

            float distance = (golem.Position() - golem.TargetPosition()).magnitude;

            if (distance < floatDict["attackRange"]) {
                boolDict["successPhase1"] = true;
            }
        } else if (golem.timeSinceLastAction < 1.0f) {
            if (boolDict["successPhase2"]) return true;

            boolDict["successPhase2"] = true;

            golem.speed = golem.baseSpeed * 0.75f;
            golem.animator.speed = 1;
            golem.throwing = false;

            if (boolDict["successPhase1"]) {
                GameObject flyingClub = Instantiate(flyingClubPrefab);
                flyingClub.transform.position = golem.Position();
                flyingClub.GetComponent<FlyingClub>().throwPosition = golem.Position();
                flyingClub.GetComponent<FlyingClub>().target = golem.Target();
                flyingClub.GetComponent<FlyingClub>().SetFlyingTime(0.5f);

                flyingClub.GetComponent<FlyingClub>().clubHitingGroundPrefab = clubHitingGroundPrefab;
                flyingClub.GetComponent<FlyingClub>().hitAreaPrefab = hitAreaPrefab;
                flyingClub.GetComponent<FlyingClub>().explosionDamage = floatDict["damage"];
                flyingClub.GetComponent<FlyingClub>().explosionArea = floatDict["area"];
                flyingClub.GetComponent<FlyingClub>().explosionDuration = 0.5f;
                flyingClub.GetComponent<FlyingClub>().whoThrow = golem.GUID();

            }
        }

        return true;
    }

    private void A7Setup()
    {
        float manaCost = A7_Mana;

        floatDict.Clear();
        boolDict.Clear();
        intDict.Clear();
        boolDict.Add("successPhase1", false);
        boolDict.Add("successPhase2", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A7_Execucao + A7_Recuperacao;
            floatDict.Add("damage", golem.strength * A7_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.meleeRange * A7_Alcance);
            floatDict.Add("area", golem.basicRange + golem.meleeRange * A7_Area);

            golem.speed = golem.baseSpeed * 0.25f;
            golem.throwing = true;
            golem.animator.speed = 2;
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A7_Recuperacao;
        }
    }

    private void A7CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }
}
