using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Golem : MonoBehaviour, ICharacter
{
    // Bullet
    public Bullet bulletPrefab;
    public Transform launchOffset;

    // Target
    public enum TargetType { Self, Friend, Enemy };
    public TargetType targetType;
    public Enemy targetEnemy;
    public Golem targetFriend;

    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public float mana = 35;
    public float maxMana = 70;
    public HealthManaBar healthManaBar;

    public float strength;

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

    // Rune execution logic
    public GolemProgram golemProgram;
    public float cooldown = 0;
    public float timeSinceLastAction = 0;
    public String movementBehavior = "M1";
    public bool runeExecuted;

    // Select logic
    public SelectedAndTargetUI selectedAndTargetUI;
    public RuneSelectionUI runeSelectionUI;
    public bool selected = false;

    // Flash Effect
    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;
    public Material stdMaterial;
    public Material flashMaterial;
    public float flashDuration;

    // Miscellaneous
    public LevelDirector levelDirector;
    public int guid;
    public NavMeshAgent navMeshAgent;
    public bool alive;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = stdMaterial;

        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Without any rune
        this.runeFunctionMap.Add("NoCommand", new RuneFunction(NoCommand));

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        UpdateTarget();

        alive = true;
    }

    // Update is called once per frame
    private bool gameOver = false;
    void Update()
    {
        if (gameOver) return;
        if (!levelDirector.levelStartedRunning) return;
        if (!alive) return;
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
            targetFriend = levelDirector.GetRandomFriend();

            if (guid == targetFriend.guid) {
                targetType = TargetType.Self;
            } else {
                targetType = TargetType.Friend;
            }

            if (selected) Select();
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

    public Vector3 TargetPosition()
    {
        if (targetType == TargetType.Self) {
            return transform.position;
        } else if (targetType == TargetType.Friend) {
            return targetFriend.transform.position;
        } else {
            return targetEnemy.transform.position;
        }
    }

    public void Unselect()
    {
        if (!selected) return;

        selected = false;
        selectedAndTargetUI.Hide();

        if (targetType == TargetType.Friend) {
            if (targetFriend != null) {
                targetFriend.selectedAndTargetUI.Hide();
            }
        } else if (targetType == TargetType.Enemy) {
            // targetEnemy.selectedAndTargetUI.Hide();
        }
    }

    public void Select()
    {
        selected = true;

        if (targetType == TargetType.Self) {
            selectedAndTargetUI.PlayAutoTarget();
        }
        if (targetType == TargetType.Friend) {
            selectedAndTargetUI.PlaySelected();
            targetFriend.selectedAndTargetUI.PlayFriendTarget();
        } else if (targetType == TargetType.Enemy) {
            selectedAndTargetUI.PlaySelected();
            // targetEnemy.selectedAndTargetUI.PlayTarget();
        }
    }

    public void OpenRuneSelectionUI()
    {
        runeSelectionUI.OpenRuneSelectionUI();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Flash();
        if (health < 0 && alive) Die();
    }

    public void Die()
    {
        // TODO: disable collider2D (That don't exist yet)
        alive = false;
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }

    // Code from 
    // https://github.com/BarthaSzabolcs/Tutorial-SpriteFlash/blob/main/Assets/Scripts/FlashEffects/SimpleFlash.cs
    public void Flash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(flashDuration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = stdMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }
}
