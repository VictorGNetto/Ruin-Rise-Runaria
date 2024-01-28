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

    void Awake()
    {
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
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }

    public void TakeDamage(float amount)
    {
        if (!alive) return;

        amount = (1 - defense) * amount;
        health = Mathf.Max(0, health - amount);
        healthBar.SetHealth(health, maxHealth);
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
}