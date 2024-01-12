using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // Dictionaries to allow variable sharing among related functions
    private Dictionary<String, float> floatDict = new Dictionary<string, float>();

    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public float mana = 20;
    public float maxMana = 30;
    public HealthManaBar healthManaBar;

    // Attack, support, conditional and target runes
    private delegate bool RuneFunction();
    private Dictionary<String, RuneFunction> runeFunctionMap = new Dictionary<string, RuneFunction>();

    // Setup funtions
    private delegate void SetupBeforeAction();
    private Dictionary<String, SetupBeforeAction> setupFunctionMap = new Dictionary<string, SetupBeforeAction>();

    // Clean up funtions
    private delegate void CleanUpAfterAction();
    private Dictionary<String, CleanUpAfterAction> cleanUpFunctionMap = new Dictionary<string, CleanUpAfterAction>();

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
        this.setupFunctionMap.Add("H", new SetupBeforeAction(HealSetup));
        this.cleanUpFunctionMap.Add("H", new CleanUpAfterAction(HealCleanUp));

        // Movement Behaviors
        this.runeFunctionMap.Add("MB-None", new RuneFunction(SetMovementBehaviorToNone));
        this.movementBehaviorFunctionMap.Add("NoMovementBehavior", new MovementBehavior(NoMovementBehavior));

        this.runeFunctionMap.Add("MB-BackForward", new RuneFunction(SetMovementBehaviorToBackAndForward));
        this.movementBehaviorFunctionMap.Add("BackAndForwardMovementBehavior", new MovementBehavior(BackAndForwardMovementBehavior));

        // Setup the very first action
        if (this.setupFunctionMap.ContainsKey(this.golemProgram.GetCommand())) {
            this.setupFunctionMap[this.golemProgram.GetCommand()]();
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction > cooldown) {
            // Clean up last action
            if (cleanUpFunctionMap.ContainsKey(golemProgram.GetCommand())) {
                cleanUpFunctionMap[golemProgram.GetCommand()]();
            }

            golemProgram.UpdatePC();

            // Setup the next action
            if (setupFunctionMap.ContainsKey(golemProgram.GetCommand())) {
                setupFunctionMap[golemProgram.GetCommand()]();
            }

            timeSinceLastAction = 0;
        }

        movementBehaviorFunctionMap[movementBehavior]();
        runeFunctionMap[golemProgram.GetCommand()]();
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
        this.cooldown = floatDict["cooldown"];

        float totalHeal = floatDict["totalHeal"];
        float amount = totalHeal * Time.deltaTime / this.cooldown;
        this.health = Math.Min(this.health + amount, this.maxHealth);

        return true;
    }

    private void HealSetup()
    {
        float manaCost = 10.0f;

        floatDict.Clear();

        floatDict.Add("health", health);

        if (manaCost <= mana) {
            mana -= manaCost;
            floatDict.Add("totalHeal", 20.0f);
            floatDict.Add("cooldown", 2.0f);
        } else {
            floatDict.Add("totalHeal", 0.0f);
            floatDict.Add("cooldown", 0.5f);
        }
    }

    private void HealCleanUp()
    {
        this.health = floatDict["health"] + floatDict["totalHeal"];
    }

    // Conditional Runes

    // Target Runes
}
