using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : MonoBehaviour, ICharacter
{
    // Bullet
    public Bullet bulletPrefab;
    public Transform launchOffset;

    // Target
    public enum TargetType { Self, Friend, Enemy };
    public TargetType targetBias;  // Self or Friend bias will make the golem to chose a golem as default target; Enemy will do the opposite
    public TargetType targetType;
    public ICharacter target;

    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public float mana = 35;
    public float maxMana = 70;
    public float manaRecovery = 10;
    public HealthManaBar healthManaBar;

    public float strength;
    public float defense;
    public float baseSpeed;
    public float speed;

    // range = basicRange + [skillRange * meleeRange | skillRange * distanceRange]
    public float basicRange = 0.5f;
    public float meleeRange = 1.0f;
    public float distanceRange = 1.0f;

    // Animation
    private string currentAnimation;
    public Animator animator;
    public bool walking = false;
    public bool attacking = false;
    public bool throwing = false;
    public bool casting = false;

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
    public String movementBehavior;
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
        // Health and Mana Bar setup
        healthManaBar.SetHealth(health, maxHealth);
        healthManaBar.SetMana(mana, maxMana);

        // Animation
        animator = gameObject.GetComponent<Animator>();

        // Flash Effect
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = stdMaterial;

        // Without any rune
        this.runeFunctionMap.Add("NoCommand", new RuneFunction(NoCommand));

        movementBehavior = "M0";

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        ChooseFirstTarget();

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

        UpdateTargetToDefaulWhenNeeded();
        mana = Math.Min(maxMana, mana + Time.deltaTime * manaRecovery);

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

        ResolveAnimation();
    }

    private void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private void ResolveAnimation()
    {
        if (!alive) {
            ChangeAnimation("Dying");
            return;
        }

        if (throwing || casting) {
            ChangeAnimation("Throw");
            return;
        } else if (attacking) {
            ChangeAnimation("Attack");
            return;
        }

        if (walking) {
            ChangeAnimation("Walk");
        } else {
            ChangeAnimation("Idle");
        }
    }

    private void ChooseFirstTarget()
    {
        float p = 0.5f;

        if (UnityEngine.Random.Range(0.0f, 1.0f) > p) {
            target = levelDirector.GetRandomEnemy();
            targetType = TargetType.Enemy;
            targetBias = TargetType.Enemy;
        } else {
            target = levelDirector.GetRandomFriend();
            if (guid == target.GUID()) {
                targetType = TargetType.Self;
            } else {
                targetType = TargetType.Friend;
            }
            targetBias = TargetType.Friend;
        }
    }

    private void UpdateTargetToDefaulWhenNeeded()
    {
        if (target != null && target.Alive()) return;

        // Choose a target based on the bias
        if (targetBias == TargetType.Self || targetBias == TargetType.Friend) {
            target = levelDirector.GetRandomFriend();
            if (guid == target.GUID()) {
                targetType = TargetType.Self;
            } else {
                targetType = TargetType.Friend;
            }
        } else {
            target = levelDirector.GetRandomEnemy();
            targetType = TargetType.Enemy;
        }

        // It can happen that the choose target are null or dead
        // In those case, the golem selects itself as target
        if (target == null || !target.Alive()) {
            target = this;
            targetType = TargetType.Self;
        }

        if (selected) Select();
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

    public void Unselect()
    {
        if (!selected) return;

        selected = false;
        selectedAndTargetUI.Hide();
        target.SelectedAndTargetUI().Hide();
    }

    public void Select()
    {
        selected = true;

        if (targetType == TargetType.Self) {
            selectedAndTargetUI.PlayAutoTarget();
        }
        if (targetType == TargetType.Friend) {
            selectedAndTargetUI.PlaySelected();
            target.SelectedAndTargetUI().PlayFriendTarget();
        } else if (targetType == TargetType.Enemy) {
            selectedAndTargetUI.PlaySelected();
            target.SelectedAndTargetUI().PlayEnemyTarget();
        }
    }

    public void OpenRuneSelectionUI()
    {
        runeSelectionUI.OpenRuneSelectionUI();
    }

    public void LookToTheTarget()
    {
        if (target == null) return;

        float targetX = TargetPosition().x;
        float golemX = Position().x;

        if (golemX > targetX) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    // ICharacter Interface
    private ICharacter SafeTarget()
    {
        if (target == null || !target.Alive()) {
            target = this;
            targetType = TargetType.Self;

            if (selected) Select();
        }

        return target;
    }
    
    public ICharacter Target()
    {
        return SafeTarget();
    }

    public int GetSortingOrder()
    {
        return gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    }

    public int GetTargetSortingOrder()
    {
        return SafeTarget().GetSortingOrder();
    }

    public Vector3 TargetPosition()
    {
        return SafeTarget().Position();
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public void TakeDamage(float amount)
    {
        if (!alive) return;

        amount = (1 - defense) * amount;
        health = Mathf.Max(0, health - amount);
        healthManaBar.SetHealth(health, maxHealth);
        Flash();
        if (health == 0) Die();
    }

    public void Heal(float amount)
    {
        if (!alive) return;
        
        health = Mathf.Min(maxHealth, health + amount);
        healthManaBar.SetHealth(health, maxHealth);
    }

    public float MaxHealth()
    {
        return maxHealth;
    }

    public bool Alive()
    {
        return alive;
    }

    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        healthManaBar.gameObject.SetActive(false);
        alive = false;
        ResolveAnimation();
        Destroy(gameObject, 2.0f);
    }

    public int GUID()
    {
        return guid;
    }

    public SelectedAndTargetUI SelectedAndTargetUI()
    {
        return selectedAndTargetUI;
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
