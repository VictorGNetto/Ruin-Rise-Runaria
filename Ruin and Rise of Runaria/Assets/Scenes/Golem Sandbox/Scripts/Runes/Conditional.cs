using System;
using System.Collections.Generic;
using UnityEngine;

public class Conditional : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.setupFunctionMap.Add("L2-1", new Golem.SetupBeforeAction(L2xSetup));
        golem.setupFunctionMap.Add("L2-2", new Golem.SetupBeforeAction(L2xSetup));
        golem.setupFunctionMap.Add("L2-3", new Golem.SetupBeforeAction(L2xSetup));
        golem.setupFunctionMap.Add("L2-4", new Golem.SetupBeforeAction(L2xSetup));
        golem.setupFunctionMap.Add("L2-5", new Golem.SetupBeforeAction(L2xSetup));
        
        golem.runeFunctionMap.Add("L2-1", new Golem.RuneFunction(L21));
        golem.runeFunctionMap.Add("L2-2", new Golem.RuneFunction(L22));
        golem.runeFunctionMap.Add("L2-3", new Golem.RuneFunction(L23));
        golem.runeFunctionMap.Add("L2-4", new Golem.RuneFunction(L24));
        golem.runeFunctionMap.Add("L2-5", new Golem.RuneFunction(L25));
    }

    private void L2xSetup()
    {
        golem.cooldown = 1.0f;
    }

    private bool L21()
    {
        return golem.health / golem.maxHealth > 0.50f;
    }

    private bool L22()
    {
        return golem.mana / golem.maxMana > 0.50f;
    }

    private bool L23()
    {
        return golem.target.Health() / golem.target.MaxHealth() > 0.50f;
    }

    private bool L24()
    {
        float range1 = golem.basicRange + golem.meleeRange;
        float range2 = (golem.basicRange + golem.distanceRange) * 2;

        int c1 = golem.levelDirector.GetGolemsInsideCircle(golem.GUID(), golem.Position(), range1).Count;
        int c2 = golem.levelDirector.GetGolemsInsideCircle(golem.GUID(), golem.Position(), range2).Count - c1;

        return c1 * 2 + c2 * 1 > 2;
    }

    private bool L25()
    {
        float range1 = golem.basicRange + golem.meleeRange;
        float range2 = (golem.basicRange + golem.distanceRange) * 2;

        int c1 = golem.levelDirector.GetEnemysInsideCircle(golem.Position(), range1).Count;
        int c2 = golem.levelDirector.GetEnemysInsideCircle(golem.Position(), range2).Count - c1;

        return c1 * 2 + c2 * 1 > 3;
    }
}