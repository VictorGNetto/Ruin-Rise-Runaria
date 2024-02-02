using UnityEngine;
using UnityEngine.AI;

public class SatyrBehaviour : MonoBehaviour
{
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime;

    public float healing;
    public float range;
    public float attackCooldown;
    private bool thereAreGolems = true;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (!enemy.alive) return;

        Enemy target = enemy.friendlyTarget;
        if (thereAreGolems)
        {
            if (target == null || !target.Alive())
            {
                enemy.friendlyTarget = LowestEnemy();
                target = enemy.friendlyTarget;
            }

            if (Vector2.Distance(transform.position, target.Position()) >= range)
            {
                Walk();
            }
            else
            {
                animator.SetBool("Walking", false);
            }

            if (Vector2.Distance(transform.position, target.Position()) <= range && Time.time >= nextAttackTime)
            {
                CastHeal();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    Enemy LowestEnemy()
    {
        Enemy lowestEnemy = null;
        LevelDirector levelDirector = enemy.levelDirector;
        float leastHealth;
        leastHealth = Mathf.Infinity;
        for (int i = 0; i < enemy.levelDirector.enemys.Count; i++)
        {
            if (levelDirector.enemys[i] != null && levelDirector.enemys[i].health < leastHealth && levelDirector.golems[i].alive)
            {
                leastHealth = levelDirector.enemys[i].health;
                lowestEnemy = levelDirector.enemys[i];
            }
        }

        if (lowestEnemy == null)
        {
            thereAreGolems = false;
        }

        return lowestEnemy;
    }

    //Golem LowestGolemPercent()
    //{
    //    Golem lowestGolem = null;
    //    LevelDirector levelDirector = enemy.levelDirector;
    //    double leastHealth;
    //    leastHealth = 2.0;
    //    for (int i = 0; i < enemy.levelDirector.golems.Count; i++)
    //    {
    //        double golemHealthPercent = levelDirector.golems[i].health / levelDirector.golems[i].maxHealth;
    //        if (levelDirector.golems[i] != null && golemHealthPercent < leastHealth && levelDirector.golems[i].alive)
    //        {
    //            leastHealth = golemHealthPercent;
    //            lowestGolem = levelDirector.golems[i];
    //        }
    //    }

    //    if (lowestGolem == null)
    //    {
    //        thereAreGolems = false;
    //    }

    //    return lowestGolem;
    //}

    public void Walk()
    {
        Enemy target = enemy.friendlyTarget;
        animator.SetBool("Walking", true);
        navMeshAgent.SetDestination(target.Position());
        if (target.Position().x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void HealFriendly()
    {
        //enemy.target.TakeDamage(enemy.strength);
        enemy.friendlyTarget.Heal(healing);
    }

    public void CastHeal()
    {
        //animator.SetTrigger("Attack");
        animator.SetTrigger("Heal");
    }
}
