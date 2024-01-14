using System;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // Bullet
    public Bullet bulletPrefab;
    public Transform launchOffset;

    // Target
    private Golem target;
    public LevelDirector levelDirector;

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

    void Awake()
    {
        // Target
        target = levelDirector.GetEnemy();

        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Without any rune
        this.runeFunctionMap.Add("NoCommand", new RuneFunction(NoCommand));

        // Attack
        this.runeFunctionMap.Add("RangedAttack", new RuneFunction(RangedAttack));
        this.setupFunctionMap.Add("RangedAttack", new SetupBeforeAction(RangedAttackSetup));

        // Support
        this.runeFunctionMap.Add("Heal", new RuneFunction(Heal));
        this.setupFunctionMap.Add("Heal", new SetupBeforeAction(HealSetup));
        this.cleanUpFunctionMap.Add("Heal", new CleanUpAfterAction(HealCleanUp));

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
        runeFunctionMap[golemProgram.GetCommand()]();
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

    // Attack Runes
    private bool RangedAttack()
    {
        cooldown = floatDict["cooldown"];

        if (floatDict["bulletCount"] > 0) {
            floatDict["bulletCount"] = floatDict["bulletCount"] - 1;

            Bullet bullet = Instantiate(bulletPrefab, launchOffset.position, transform.rotation);
            bullet.target = target;
            bullet.damage = floatDict["damage"];

            Debug.Log(bullet);
        }

        return true;
    }

    private void RangedAttackSetup()
    {
        float manaCost = 15.0f;

        floatDict.Clear();

        if (manaCost <= mana) {
            mana -= manaCost;
            floatDict.Add("damage", 15.0f);
            floatDict.Add("bulletCount", 1.0f);
            floatDict.Add("cooldown", 3.0f);
        } else {
            floatDict.Add("damage", 0.0f);
            floatDict.Add("bulletCount", 0.0f);
            floatDict.Add("cooldown", 0.5f);
        }
    }

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
        float manaCost = 20.0f;

        floatDict.Clear();

        floatDict.Add("health", health);

        if (manaCost <= mana) {
            mana -= manaCost;
            floatDict.Add("totalHeal", 25.0f);
            floatDict.Add("cooldown", 1.5f);
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
