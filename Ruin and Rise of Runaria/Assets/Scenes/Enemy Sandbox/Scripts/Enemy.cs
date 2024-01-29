using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    // Health
    public float health = 75;
    public float maxHealth = 100;
    public EnemyHealthBar healthBar;

    public Golem target;
    public float strength;
    public float defense;
    private int guid;
    public bool alive = false;
    public LevelDirector levelDirector;

    // Select logic
    public SelectedAndTargetUI selectedAndTargetUI;

    // Flash Effect
    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;
    public Material stdMaterial;
    public Material flashMaterial;
    public float flashDuration;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        healthBar.SetHealth(health, maxHealth);
    }

    public ICharacter Target()
    {
        return target;
    }

    public Vector3 TargetPosition()
    {
        return target.Position();
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        selectedAndTargetUI.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        alive = false;
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }

    public void TakeDamage(float amount)
    {
        if (!alive) return;

        amount = (1 - defense) * amount;
        health = Mathf.Max(0, health - amount);
        healthBar.SetHealth(health, maxHealth);
        Flash();
        if (health == 0) Die();
    }

    public void Heal(float amount)
    {
        if (!alive) return;

        health = Mathf.Min(maxHealth, health + amount);
        healthBar.SetHealth(health, maxHealth);
    }

    public float MaxHealth()
    {
        return maxHealth;
    }

    public bool Alive()
    {
        return alive;
    }

    public int GUID()
    {
        return guid;
    }

    public int GetSortingOrder()
    {
        return GetComponent<SpriteRenderer>().sortingOrder;
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