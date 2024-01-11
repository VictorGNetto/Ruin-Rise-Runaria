using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public float mana = 20;
    public float maxMana = 30;
    public HealthManaBar healthManaBar;

    // Attack, support, conditional and target runes
    private delegate bool RuneFunction();
    private Dictionary<String, RuneFunction> runeFunctionMap = new Dictionary<string, RuneFunction>();

    // Movement Behavior runes
    private delegate void MovementBehavior();
    private Dictionary<String, MovementBehavior> movementBehaviorFunctionMap = new Dictionary<string, MovementBehavior>();
    private String movementBehavior = "NoMovementBehavior";
    private String movingDirection = "R";
    private float timeMoving = 0;


    public GolemProgram golemProgram;
    private float cooldown = 1f;
    private float timeSinceLastAction = 0;

    void Start()
    {
        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Support
        this.runeFunctionMap.Add("H", new RuneFunction(Heal));

        // Movement Behaviors
        this.runeFunctionMap.Add("MB-None", new RuneFunction(SetMovementBehaviorToNone));
        this.movementBehaviorFunctionMap.Add("NoMovementBehavior", new MovementBehavior(NoMovementBehavior));

        this.runeFunctionMap.Add("MB-BackForward", new RuneFunction(SetMovementBehaviorToBackAndForward));
        this.movementBehaviorFunctionMap.Add("BackAndForwardMovementBehavior", new MovementBehavior(BackAndForwardMovementBehavior));
    }

    // Update is called once per frame
    void Update()
    {
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        this.timeSinceLastAction += Time.deltaTime;

        if (this.timeSinceLastAction > this.cooldown) {
            this.golemProgram.UpdatePC();

            timeSinceLastAction = 0;
        }

        this.movementBehaviorFunctionMap[this.movementBehavior]();
        this.runeFunctionMap[this.golemProgram.GetCommand()]();
    }

    // Movement Behavior Runes
    private bool SetMovementBehaviorToNone()
    {
        this.cooldown = 3;
        this.movementBehavior = "NoMovementBehavior";
        this.GetComponent<Animator>().SetBool("Walking", false);

        return true;
    }

    private void NoMovementBehavior()
    {
        // do nothing
    }

    private bool SetMovementBehaviorToBackAndForward()
    {
        this.cooldown = 1;
        this.movementBehavior = "BackAndForwardMovementBehavior";
        this.GetComponent<Animator>().SetBool("Walking", true);

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
            transform.localScale = new Vector3(0.3f, 0.3f, 0);
        } else {
            transform.Translate(-d, 0 , 0);
            transform.localScale = new Vector3(-0.3f, 0.3f, 0);
        }
    }

    // Attack Runes

    // Support Runes

    private bool Heal()
    {
        this.cooldown = 2.0f;
        float total = 10.0f;
        float amount = total * Time.deltaTime / this.cooldown;
        this.health = Math.Min(this.health + amount, this.maxHealth);

        return true;
    }

    // Conditional Runes

    // Target Runes
}
