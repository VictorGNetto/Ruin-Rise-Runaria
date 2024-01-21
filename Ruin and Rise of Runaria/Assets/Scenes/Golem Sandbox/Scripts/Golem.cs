using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Golem : MonoBehaviour
{
    // Bullet
    public Bullet bulletPrefab;
    public Transform launchOffset;

    public LevelDirector levelDirector;
    public int guid;

    // Target
    public enum TargetType { Friend, Enemy };
    public TargetType targetType = TargetType.Friend;
    public Enemy targetEnemy;
    public Golem targetFriend;

    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public float mana = 35;
    public float maxMana = 70;
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
    public String movementBehavior = "M1";
    public NavMeshAgent navMeshAgent;
    public bool runeExecuted;
    public SelectedAndTargetUI selectedAndTargetUI;

    void Awake()
    {
        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Without any rune
        this.runeFunctionMap.Add("NoCommand", new RuneFunction(NoCommand));

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    private bool gameOver = false;
    void Update()
    {
        if (gameOver) return;
        if (!levelDirector.levelStartedRunning) return;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        UpdateTarget();
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

    private void UpdateTarget()
    {
        if (targetFriend == null) {
            targetFriend = levelDirector.GetRandomFriend(guid);
        }
    }

    public void Setup()
    {
        timeSinceLastAction = 0;

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

    public Vector3 GetTargetPosition()
    {
        if (targetType == TargetType.Friend) {
            return targetFriend.transform.position;
        } else {
            return targetEnemy.transform.position;
        }
    }
}
