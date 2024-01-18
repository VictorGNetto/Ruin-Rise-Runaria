using System;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // Bullet
    public Bullet bulletPrefab;
    public Transform launchOffset;

    // Target
    public Golem target;
    public LevelDirector levelDirector;

    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public float mana = 20;
    public float maxMana = 30;
    public HealthManaBar healthManaBar;

    // Attack, support, conditional and target runes
    public delegate bool RuneFunction();
    public Dictionary<String, RuneFunction> runeFunctionMap = new Dictionary<string, RuneFunction>();

    // Setup funtions
    public delegate void SetupBeforeAction();
    public Dictionary<String, SetupBeforeAction> setupFunctionMap = new Dictionary<string, SetupBeforeAction>();

    // Clean up funtions
    public delegate void CleanUpAfterAction();
    public Dictionary<String, CleanUpAfterAction> cleanUpFunctionMap = new Dictionary<string, CleanUpAfterAction>();

    // Movement Behavior runes
    public delegate void MovementBehavior();
    public Dictionary<String, MovementBehavior> movementBehaviorFunctionMap = new Dictionary<string, MovementBehavior>();

    public GolemProgram golemProgram;
    public float cooldown = 0;
    public float timeSinceLastAction = 0;
    public String movementBehavior = "NoMovementBehavior";

    void Awake()
    {
        // Target
        target = levelDirector.GetEnemy();

        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Without any rune
        this.runeFunctionMap.Add("NoCommand", new RuneFunction(NoCommand));
    }

    // Update is called once per frame
    void Update()
    {
        mana = Math.Min(maxMana, mana + Time.deltaTime * 5);

        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction > cooldown) {
            // Clean up last action
            CleanUp();

            golemProgram.UpdatePC();

            // Setup the next action
            Setup();

            timeSinceLastAction = 0;
        }

        movementBehaviorFunctionMap[movementBehavior]();
        golemProgram.actionResult = runeFunctionMap[golemProgram.GetCommand()]();
    }

    public void Setup()
    {
        if (setupFunctionMap.ContainsKey(golemProgram.GetCommand())) {
            setupFunctionMap[golemProgram.GetCommand()]();
        }
    }

    private void CleanUp()
    {
        if (cleanUpFunctionMap.ContainsKey(golemProgram.GetCommand())) {
            cleanUpFunctionMap[golemProgram.GetCommand()]();
        }
    }

    private bool NoCommand()
    {
        this.cooldown = 10.0f;

        // do nothing

        return true;
    }
}
