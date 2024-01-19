using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    private Golem golem;

    private String movingDirection = "R";
    private float timeMoving = 0;

    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();
    private Dictionary<String, float> intDict = new Dictionary<string, float>();

    private void Awake()
    {
        golem = gameObject.GetComponent<Golem>();

        // Movement Behaviors
        golem.runeFunctionMap.Add("MB-None", new Golem.RuneFunction(SetMovementBehaviorToNone));
        golem.movementBehaviorFunctionMap.Add("NoMovementBehavior", new Golem.MovementBehavior(NoMovementBehavior));

        golem.runeFunctionMap.Add("MB-BackForward", new Golem.RuneFunction(SetMovementBehaviorToBackAndForward));
        golem.movementBehaviorFunctionMap.Add("BackAndForwardMovementBehavior", new Golem.MovementBehavior(BackAndForwardMovementBehavior));
    }

    // Movement Behavior Runes
    private bool SetMovementBehaviorToNone()
    {
        golem.cooldown = 1;
        golem.movementBehavior = "NoMovementBehavior";
        golem.GetComponent<Animator>().SetBool("Walking", false);

        return true;
    }

    private void NoMovementBehavior()
    {
        // do nothing
    }

    private bool SetMovementBehaviorToBackAndForward()
    {
        golem.cooldown = 2;
        golem.movementBehavior = "BackAndForwardMovementBehavior";
        golem.GetComponent<Animator>().SetBool("Walking", true);

        return true;
    }

    private void BackAndForwardMovementBehavior()
    {
        this.timeMoving += Time.deltaTime;

        if (this.timeMoving > 0.75f) {
            this.timeMoving = 0;

            if (this.movingDirection.Equals("R")) {
                this.movingDirection = "L";
            } else {
                this.movingDirection = "R";
            }
        }

        float d = 2 * Time.deltaTime / 0.75f;

        if (this.movingDirection.Equals("R")) {
            transform.Translate(d, 0 , 0);
            transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
        } else {
            transform.Translate(-d, 0 , 0);
            transform.localScale = new Vector3(-0.2f, 0.2f, 1.0f);
        }
    }
}
