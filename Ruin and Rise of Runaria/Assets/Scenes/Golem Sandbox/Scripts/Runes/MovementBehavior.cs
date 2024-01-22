using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementBehavior : MonoBehaviour
{
    private Golem golem;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();
    private Dictionary<String, bool> boolDict = new Dictionary<string, bool>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        golem.runeFunctionMap.Add("M1", new Golem.RuneFunction(SetMovementBehaviorToM1));
        golem.movementBehaviorFunctionMap.Add("M1", new Golem.MovementBehavior(M1));

        golem.runeFunctionMap.Add("M2", new Golem.RuneFunction(SetMovementBehaviorToM2));
        golem.movementBehaviorFunctionMap.Add("M2", new Golem.MovementBehavior(M2));

        golem.runeFunctionMap.Add("M3", new Golem.RuneFunction(SetMovementBehaviorToM3));
        golem.movementBehaviorFunctionMap.Add("M3", new Golem.MovementBehavior(M3));

        golem.runeFunctionMap.Add("M4", new Golem.RuneFunction(SetMovementBehaviorToM4));
        golem.movementBehaviorFunctionMap.Add("M4", new Golem.MovementBehavior(M4));
    }

    // Rune M1
    private bool SetMovementBehaviorToM1()
    {
        golem.cooldown = 0.5f;
        golem.runeExecuted = true;
        golem.movementBehavior = "M1";

        return true;
    }

    private void M1()
    {
        if (!boolDict.ContainsKey("M1")) {
            InitM1();
            UpdateM1();
        }

        floatDict["timeMoving"] += Time.deltaTime;

        if (floatDict["timeMoving"] > floatDict["nextMoveDuration"]) {
            UpdateM1();
        }

        Vector3 destination = new Vector3(floatDict["horizontalFactor"], floatDict["verticalFactor"], 0);
        golem.navMeshAgent.speed = floatDict["speed"];
        golem.navMeshAgent.SetDestination(golem.transform.position + destination.normalized);
    }

    private void InitM1()
    {
        boolDict.Clear();
        floatDict.Clear();
        boolDict["M1"] = true;

        golem.navMeshAgent.stoppingDistance = 0f;
    }

    private void UpdateM1()
    {
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

    // Rune M2
    private bool SetMovementBehaviorToM2()
    {
        golem.cooldown = 0.5f;
        golem.runeExecuted = true;
        golem.movementBehavior = "M2";

        return true;
    }

    private void M2()
    {
        if (!boolDict.ContainsKey("M2")) {
            InitM2();
            UpdateM2();
        }

        floatDict["timeMoving"] += Time.deltaTime;

        if (floatDict["timeMoving"] > floatDict["nextMoveDuration"]) {
            UpdateM2();
        }

        Vector3 destination = new Vector3(floatDict["horizontalFactor"], floatDict["verticalFactor"], 0);
        golem.navMeshAgent.speed = floatDict["speed"];
        golem.navMeshAgent.SetDestination(golem.transform.position + destination.normalized);
    }

    private void InitM2()
    {
        boolDict.Clear();
        floatDict.Clear();
        boolDict["M2"] = true;

        floatDict["oldGolemPosX"] = golem.transform.position.x;
        floatDict["oldGolemPosY"] = golem.transform.position.y;
        floatDict["radius"] = 0.5f;
        golem.navMeshAgent.stoppingDistance = 0f;
    }

    private void UpdateM2()
    {
        floatDict["timeMoving"] = 0;
        floatDict["nextMoveDuration"] = UnityEngine.Random.Range(0.75f, 1.5f);
        floatDict["speed"] = UnityEngine.Random.Range(0.75f, 1.5f);


        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.2) {
            floatDict["horizontalFactor"] = UnityEngine.Random.Range(-1.0f, 1.0f);
            floatDict["verticalFactor"] = UnityEngine.Random.Range(-1.0f, 1.0f);
            golem.gameObject.GetComponent<Animator>().SetBool("Walking", true);

            Vector2 center = new Vector2(floatDict["oldGolemPosX"], floatDict["oldGolemPosY"]);
            Vector2 pos = new Vector2(golem.transform.position.x, golem.transform.position.y);

            if ((center - pos).magnitude > floatDict["radius"]) {
                floatDict["horizontalFactor"] = 0.5f * floatDict["horizontalFactor"] + 0.5f * (center - pos).x;
                floatDict["verticalFactor"] = 0.5f * floatDict["verticalFactor"] + 0.5f * (center - pos).y;
            }

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

    // Rune M3
    private bool SetMovementBehaviorToM3()
    {
        golem.cooldown = 0.5f;
        golem.runeExecuted = true;
        golem.movementBehavior = "M3";

        return true;
    }

    private void M3()
    {
        if (!boolDict.ContainsKey("M3")) {
            InitM3();
        }

        Vector3 destination = golem.TargetPosition();
        if (destination.x < golem.transform.position.x) {
            golem.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            golem.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (boolDict["awake"]) {
            if ((destination - golem.transform.position).magnitude < floatDict["stoppingDistance"]) {
                golem.gameObject.GetComponent<Animator>().SetBool("Walking", false);
                boolDict["awake"] = false;
            } else {
                golem.gameObject.GetComponent<Animator>().SetBool("Walking", true);
            }
            
            golem.navMeshAgent.SetDestination(destination);
        } else {
            if ((destination - golem.transform.position).magnitude > floatDict["awakeDistance"]) {
                boolDict["awake"] = true;
            }
        }
    }

    private void InitM3()
    {
        boolDict.Clear();
        floatDict.Clear();
        boolDict["M3"] = true;

        floatDict["stoppingDistance"] = 0.75f;
        floatDict["awakeDistance"] = 1.25f;
        boolDict["awake"] = true;
        golem.navMeshAgent.speed = 1.5f;
        golem.navMeshAgent.stoppingDistance = floatDict["stoppingDistance"];
    }

    // Rune M4
    private bool SetMovementBehaviorToM4()
    {
        golem.cooldown = 0.5f;
        golem.runeExecuted = true;
        golem.movementBehavior = "M4";

        return true;
    }

    private void M4()
    {
        if (!boolDict.ContainsKey("M4")) {
            InitM4();
        }

        Vector3 destination;
        if (golem.targetType == Golem.TargetType.Friend) {
            destination = golem.levelDirector.GetGolemsCentroid(golem.guid);
        } else {
            destination = golem.levelDirector.GetEnemysCentroid(golem.guid);
        }

        if (destination.x < golem.transform.position.x) {
            golem.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            golem.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (boolDict["awake"]) {
            if ((destination - golem.transform.position).magnitude < floatDict["stoppingDistance"]) {
                golem.gameObject.GetComponent<Animator>().SetBool("Walking", false);
                boolDict["awake"] = false;
            } else {
                golem.gameObject.GetComponent<Animator>().SetBool("Walking", true);
            }
            
            golem.navMeshAgent.SetDestination(destination);
        } else {
            if ((destination - golem.transform.position).magnitude > floatDict["awakeDistance"]) {
                boolDict["awake"] = true;
            }
        }
    }

    private void InitM4()
    {
        boolDict.Clear();
        floatDict.Clear();
        boolDict["M4"] = true;

        floatDict["stoppingDistance"] = 1.5f;
        floatDict["awakeDistance"] = 2.0f;
        boolDict["awake"] = true;
        golem.navMeshAgent.speed = 1.5f;
        golem.navMeshAgent.stoppingDistance = floatDict["stoppingDistance"];
    }
}
