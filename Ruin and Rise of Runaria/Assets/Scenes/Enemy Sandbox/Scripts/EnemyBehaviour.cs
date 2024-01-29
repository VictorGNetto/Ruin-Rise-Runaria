using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform launchOffset;
    public LevelDirector levelDirector;
    public GameObject projectile = null;
    public Animator anim;
    public Vector3 scale;
    public SpriteRenderer spriteRenderer;
    public Golem target = null;
    public float attackCooldown;
    private float nextAttackTime = 0.2f;
    private bool thereAreGolems = true;


    public float defense;
    public float health = 75;
    public float maxHealth = 100;
    public EnemyHealthBar enemyHealthBar;

    public bool alive = true;

    private int guid;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        scale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    Golem ClosestGolem()
    {
        
        Golem closestGolem = null;
        float leastDistance;
        leastDistance = Mathf.Infinity;
        for (int i = 0; i < levelDirector.golems.Count; i++)
        {
            if (levelDirector.golems[i]!= null && Vector3.Distance(transform.position, levelDirector.golems[i].transform.position) < leastDistance)
            {
                leastDistance = Vector3.Distance(transform.position, levelDirector.golems[i].transform.position);
                closestGolem = levelDirector.golems[i];
            }
        }
        if (closestGolem == null)
        {
            thereAreGolems = false;
        }
        return closestGolem;
    }

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

        if(thereAreGolems) { 
            
            if (target == null || !target.alive)
            {
                target = ClosestGolem();
            }

            if (Time.time >= nextAttackTime)
            {
                Fire();
                nextAttackTime = Time.time + attackCooldown;
            }
        }


    }

    public void Fire()
    {

        if(ClosestGolem().transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (target.alive && target != null)
        {
            anim.SetTrigger("Cast");
            Vector3 direction = ClosestGolem().transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject proj = Instantiate(projectile, launchOffset.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            proj.GetComponent<ProjectileScript>().target = ClosestGolem();
            
        }
        else
        {
            target = ClosestGolem();
        }
        
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

    public SelectedAndTargetUI SelectedAndTargetUI()
    {
        throw new System.NotImplementedException();
    }



}
