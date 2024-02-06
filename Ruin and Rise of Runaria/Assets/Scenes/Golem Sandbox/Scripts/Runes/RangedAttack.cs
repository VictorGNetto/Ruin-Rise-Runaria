using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    // Public Attacks Parameters
    private float A8_Dano = 1.0f;
    private float A8_Alcance = 1.0f;
    private float A8_Execucao = 1.0f;
    private float A8_Recuperacao = 0.5f;
    private float A8_Mana = 15.0f;

    private float A9_Dano = 2.0f;
    private float A9_Alcance = 1.5f;
    private float A9_Execucao = 1.5f;
    private float A9_Recuperacao = 0.5f;
    private float A9_Mana = 25.0f;
    public Color A9_Ray_Color;

    private float A10_Dano = 0.75f;
    private float A10_Alcance = 1.5f;
    private float A10_Execucao = 1.5f;
    private float A10_Recuperacao = 0.5f;
    private float A10_Mana = 30.0f;

    private float A11_Dano = 4.0f;
    private float A11_Alcance = 1.5f;
    private float A11_Execucao = 1.5f;
    private float A11_Recuperacao = 0.5f;
    private float A11_Mana = 50.0f;
    public Color A11_Ray_Color;

    private float A12_Dano = 0.85f;
    private float A12_Alcance = 1.0f;
    private float A12_Area = 2.0f;
    private float A12_Execucao = 1.5f;
    private float A12_Recuperacao = 0.5f;
    private float A12_Mana = 20.0f;

    private float A13_Dano = 1.0f;
    private float A13_Alcance = 1.5f;
    private float A13_Area = 2.5f;
    private float A13_Execucao = 2.0f;
    private float A13_Recuperacao = 0.5f;
    private float A13_Mana = 40.0f;
    public Color A13_Explosion_Color;

    private float A14_Dano = 1.5f;
    private float A14_Alcance = 1.5f;
    private float A14_Area = 3.0f;
    private float A14_Execucao = 2.0f;
    private float A14_Recuperacao = 0.5f;
    private float A14_Mana = 60.0f;
    public Color A14_Explosion_Color;

    public Transform launchOffset;
    public GameObject runicSpellPrefab;
    public GameObject rayPrefab;
    public GameObject hitAreaPrefab;
    public GameObject explosion3Prefab;
    public GameObject explosion6Prefab;

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

        golem.runeFunctionMap.Add("A12", new Golem.RuneFunction(A12));
        golem.setupFunctionMap.Add("A12", new Golem.SetupBeforeAction(A12Setup));
        golem.cleanUpFunctionMap.Add("A12", new Golem.CleanUpAfterAction(A12CleanUp));

        golem.runeFunctionMap.Add("A13", new Golem.RuneFunction(A13));
        golem.setupFunctionMap.Add("A13", new Golem.SetupBeforeAction(A13Setup));
        golem.cleanUpFunctionMap.Add("A13", new Golem.CleanUpAfterAction(A13CleanUp));

        golem.runeFunctionMap.Add("A14", new Golem.RuneFunction(A14));
        golem.setupFunctionMap.Add("A14", new Golem.SetupBeforeAction(A14Setup));
        golem.cleanUpFunctionMap.Add("A14", new Golem.CleanUpAfterAction(A14CleanUp));
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
            runicSpell.GetComponent<RunicSpell>().doExplosion = false;
            runicSpell.GetComponent<RunicSpell>().Setup(flyingTime, launchOffset.position, targetPosition);
        }
    }

    void DoThrowExplosionSpell(float delayTime)
    {
        StartCoroutine(ThrowExplosionSpell(delayTime));
    }

    IEnumerator ThrowExplosionSpell(float delayTime)
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

        GameObject runicSpell = Instantiate(runicSpellPrefab);
        runicSpell.GetComponent<RunicSpell>().damage = floatDict["damage"];
        runicSpell.GetComponent<RunicSpell>().whoThrow = golem.GUID();
        runicSpell.GetComponent<RunicSpell>().autoTarget = golem.GUID() == golem.target.GUID();
        runicSpell.GetComponent<RunicSpell>().doExplosion = true;
        runicSpell.GetComponent<RunicSpell>().explosionRadius = floatDict["attackArea"];
        runicSpell.GetComponent<RunicSpell>().hitAreaPrefab = hitAreaPrefab;
        runicSpell.GetComponent<RunicSpell>().explosion3Prefab = explosion3Prefab;
        runicSpell.GetComponent<RunicSpell>().Setup(flyingTime, launchOffset.position, targetPosition);
    }

    void DoDelayedDamage(float delayTime)
    {
        StartCoroutine(DelayedDamage(delayTime));
    }

    IEnumerator DelayedDamage(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        SafeTakeDamage();
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
            DoThrowSpell(1, A8_Execucao/2);
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
            DoThrowSpell(3, A10_Execucao/2);
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

    // A12
    private bool A12()
    {
        golem.LookToTheTarget();
        
        // if (!golem.runeExecuted || boolDict["success"]) return true;

        return true;
    }

    private void A12Setup()
    {
        float manaCost = A12_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A12_Execucao + A12_Recuperacao;
            floatDict.Add("damage", golem.strength * A12_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A12_Alcance);
            floatDict.Add("attackArea", golem.basicRange + golem.meleeRange * A12_Area);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
            DoThrowExplosionSpell(A12_Execucao/2);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A12_Recuperacao;
        }
    }

    private void A12CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    // A13
    private bool A13()
    {
        golem.LookToTheTarget();
        if (golem.timeSinceLastAction < A13_Execucao * 0.25f) return true;
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["attackRange"]) {
            boolDict["success"] = true;

            GameObject explosion6 = Instantiate(explosion6Prefab);
            Vector3 offset = new Vector3(0, 0.65f, 0);
            explosion6.transform.position = golem.TargetPosition() + offset;
            explosion6.GetComponent<Explosion6>().Setup(golem.GetTargetSortingOrder() + 1, A13_Explosion_Color);

            GameObject hitArea = Instantiate(hitAreaPrefab);
            hitArea.transform.position = golem.TargetPosition();
            hitArea.GetComponent<CircleHitArea>().SetRadius(floatDict["attackArea"]);
            hitArea.GetComponent<CircleHitArea>().DestroyHitArea(0.5f);
            hitArea.GetComponent<CircleHitArea>().AddNotHitableCharacter(golem.GUID());
            hitArea.GetComponent<CircleHitArea>().SetDamage(floatDict["damage"]);

            golem.speed = golem.baseSpeed * 0.75f;
        }

        return true;
    }

    private void A13Setup()
    {
        float manaCost = A13_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A13_Execucao + A13_Recuperacao;
            floatDict.Add("damage", golem.strength * A13_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A13_Alcance);
            floatDict.Add("attackArea", golem.basicRange + golem.meleeRange * A13_Area);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A13_Recuperacao;
        }
    }

    private void A13CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }

    // A14
    private bool A14()
    {
        golem.LookToTheTarget();
        if (golem.timeSinceLastAction < A14_Execucao * 0.25f) return true;
        if (!golem.runeExecuted || boolDict["success"]) return true;

        float distance = (golem.Position() - golem.TargetPosition()).magnitude;

        if (distance < floatDict["attackRange"]) {
            boolDict["success"] = true;

            GameObject explosion6 = Instantiate(explosion6Prefab);
            Vector3 offset = new Vector3(0, 0.65f, 0);
            explosion6.transform.position = golem.TargetPosition() + offset;
            explosion6.GetComponent<Explosion6>().Setup(golem.GetTargetSortingOrder() + 1, A14_Explosion_Color);

            GameObject hitArea = Instantiate(hitAreaPrefab);
            hitArea.transform.position = golem.TargetPosition();
            hitArea.GetComponent<CircleHitArea>().SetRadius(floatDict["attackArea"]);
            hitArea.GetComponent<CircleHitArea>().DestroyHitArea(0.5f);
            hitArea.GetComponent<CircleHitArea>().AddNotHitableCharacter(golem.GUID());
            hitArea.GetComponent<CircleHitArea>().SetDamage(floatDict["damage"]);

            golem.speed = golem.baseSpeed * 0.75f;
        }

        return true;
    }

    private void A14Setup()
    {
        float manaCost = A14_Mana;

        floatDict.Clear();
        boolDict.Clear();
        boolDict.Add("success", false);

        if (manaCost <= golem.mana) {
            golem.mana -= manaCost;
            golem.runeExecuted = true;
            golem.cooldown = A14_Execucao + A14_Recuperacao;
            floatDict.Add("damage", golem.strength * A14_Dano);
            floatDict.Add("attackRange", golem.basicRange + golem.distanceRange * A14_Alcance);
            floatDict.Add("attackArea", golem.basicRange + golem.meleeRange * A14_Area);

            golem.speed = golem.baseSpeed * 0.5f;
            golem.animator.speed = 1;
            golem.casting = true;
            DoResetCastingAnimation(1.0f);
        } else {
            golem.runeExecuted = false;
            golem.cooldown = A14_Recuperacao;
        }
    }

    private void A14CleanUp()
    {
        golem.speed = golem.baseSpeed;
    }
}
