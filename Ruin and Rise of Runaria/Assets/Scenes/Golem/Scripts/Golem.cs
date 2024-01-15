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
    private delegate void MovementBehavior();
    private Dictionary<String, MovementBehavior> movementBehaviorFunctionMap = new Dictionary<string, MovementBehavior>();
    private String movementBehavior = "NoMovementBehavior";
    private String movingDirection = "R";
    private float timeMoving = 0;


    public GolemProgram golemProgram;
    public float cooldown = 1f;
    public float timeSinceLastAction = 0;

    void Awake()
    {
        // Target
        target = levelDirector.GetEnemy();

        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Without any rune
        this.runeFunctionMap.Add("NoCommand", new RuneFunction(NoCommand));

        // Conditional
        this.runeFunctionMap.Add("C1", new RuneFunction(Conditional));
        this.runeFunctionMap.Add("C2", new RuneFunction(Conditional));
        this.runeFunctionMap.Add("C3", new RuneFunction(Conditional));

        // Movement Behaviors
        this.runeFunctionMap.Add("MB-None", new RuneFunction(SetMovementBehaviorToNone));
        this.movementBehaviorFunctionMap.Add("NoMovementBehavior", new MovementBehavior(NoMovementBehavior));

        this.runeFunctionMap.Add("MB-BackForward", new RuneFunction(SetMovementBehaviorToBackAndForward));
        this.movementBehaviorFunctionMap.Add("BackAndForwardMovementBehavior", new MovementBehavior(BackAndForwardMovementBehavior));
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
            transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
        } else {
            transform.Translate(-d, 0 , 0);
            transform.localScale = new Vector3(-0.2f, 0.2f, 1.0f);
        }
    }

    private bool NoCommand()
    {
        this.cooldown = 1.0f;

        // do nothing

        return true;
    }

    // Conditional Runes
    private bool Conditional()
    {
        bool b = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        Debug.Log(b);
        this.cooldown = 0;

        return b;
    }
}
