using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementBehavior : MonoBehaviour
{
    private Golem golem;

    private float timeMoving = 0;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        // Movement Behaviors
        golem.runeFunctionMap.Add("M1", new Golem.RuneFunction(SetMovementBehaviorToM1));
        golem.movementBehaviorFunctionMap.Add("M1", new Golem.MovementBehavior(M1));
    }

    // Movement Behavior Runes
    private bool SetMovementBehaviorToM1()
    {
        golem.cooldown = 0.5f;
        golem.runeExecuted = true;
        golem.movementBehavior = "M1";

        return true;
    }

    private void M1()
    {
        if (!floatDict.ContainsKey("M1")) {
            SetupM1();
        }

        floatDict["timeMoving"] += Time.deltaTime;

        if (floatDict["timeMoving"] > floatDict["nextMoveDuration"]) {
            SetupM1();
        }

        Vector3 destination = new Vector3(floatDict["horizontalFactor"], floatDict["verticalFactor"], 0);
        golem.navMeshAgent.speed = floatDict["speed"];
        golem.navMeshAgent.SetDestination(golem.transform.position + destination.normalized);
    }

    private void SetupM1()
    {
        floatDict.Clear();
        floatDict["M1"] = 0.0f;
        floatDict["timeMoving"] = 0;
        floatDict["nextMoveDuration"] = UnityEngine.Random.Range(0.75f, 3.0f);
        floatDict["speed"] = UnityEngine.Random.Range(0.75f, 1.5f);

        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.2) {
            floatDict["horizontalFactor"] = UnityEngine.Random.Range(-1.0f, 1.0f);
            floatDict["verticalFactor"] = UnityEngine.Random.Range(-1.0f, 1.0f);
            golem.gameObject.GetComponent<Animator>().SetBool("Walking", true);
        } else {
            floatDict["horizontalFactor"] = 0;
            floatDict["verticalFactor"] = 0;
            golem.gameObject.GetComponent<Animator>().SetBool("Walking", false);
        }

        if (floatDict["horizontalFactor"] < 0) {
            golem.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            golem.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
