using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MinotaurBehaviour: MonoBehaviour, ICharacter
{
    
    public Transform launchOffset;
    public LevelDirector levelDirector;
    public GameObject projectile = null;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public NavMeshAgent agent;
    public float range;
    public float damage;
    public float attackCooldown;
    public Golem target = null;
    private bool thereAreGolems = true;

    public float defense;
    public float health = 75;
    public float maxHealth = 100;
    public EnemyHealthBar enemyHealthBar;

    public bool alive = true;

    private int guid;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }



    Golem LowestGolem()
    {
        
        Golem lowestGolem = null;
        float leastHealth;
        leastHealth= Mathf.Infinity;
        for (int i = 0; i < levelDirector.golems.Count; i++)
        {
            if (levelDirector.golems[i] != null && levelDirector.golems[i].health < leastHealth && levelDirector.golems[i].alive)
            {
                leastHealth = levelDirector.golems[i].health;
                lowestGolem = levelDirector.golems[i];
            }
        }

        if (lowestGolem == null)
        {
            thereAreGolems = false;
        }
        return lowestGolem;

    }

    public void Walk()
    {
        anim.SetBool("Walking", true);
        agent.SetDestination(target.Position());
        if (target.Position().x < Position().x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    private float nextAttackTime;
    private void Update()
    {

        if (!alive) return;

        enemyHealthBar.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            alive = false;
            enemyHealthBar.gameObject.SetActive(false);
            Die();
        }




        if (thereAreGolems)
        {

            if (target == null || !target.alive)
            {
                target = LowestGolem();
            }

            if (Vector2.Distance(transform.position, target.transform.position) >= range)
            {
                Walk();
            }
            else
            {
                anim.SetBool("Walking", false);
            }

            if (Vector2.Distance(transform.position, target.transform.position) <= range && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        
        



    }

    public void DealDamage()
    {
        target.TakeDamage(damage);
    }

    public void Attack()
    {

        anim.SetTrigger("Attack");

    }





    public ICharacter Target()
    {
        return target;
    }

    public int GetSortingOrder()
    {
        return spriteRenderer.sortingOrder;
    }

    public Vector3 TargetPosition()
    {
        return target.transform.position;
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
        enemyHealthBar.SetHealth(health, maxHealth);
        if (health == 0) Die();
    }

    public void Heal(float amount)
    {
        if (!alive) return;

        health = Mathf.Min(maxHealth, health + amount);
        enemyHealthBar.SetHealth(health, maxHealth);
    }

    public float MaxHealth()
    {
        return maxHealth;
    }

    public void Die()
    {
        alive = false;
        anim.SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }

    public int GUID()
    {
        throw new System.NotImplementedException();
    }
}
