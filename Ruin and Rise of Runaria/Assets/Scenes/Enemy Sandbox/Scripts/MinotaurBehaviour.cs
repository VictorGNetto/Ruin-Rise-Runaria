using UnityEngine;
using UnityEngine.AI;

public class MinotaurBehaviour: MonoBehaviour
{
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime;

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
        if (enemy.levelDirector.levelDone) return;
        if (!enemy.alive) return;

        Golem target = enemy.target;
        if (thereAreGolems)
        {
            if (target == null || !target.Alive())
            {
                enemy.target = LowestGolemPercent();
                target = enemy.target;
            }

            if (enemy.levelDirector.levelDone || target == null) return;

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
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    Golem LowestGolem()
    {
        Golem lowestGolem = null;
        LevelDirector levelDirector = enemy.levelDirector;
        float leastHealth;
        leastHealth= Mathf.Infinity;
        for (int i = 0; i < enemy.levelDirector.golems.Count; i++)
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

    Golem LowestGolemPercent()
    {
        Golem lowestGolem = null;
        LevelDirector levelDirector = enemy.levelDirector;
        double leastHealth;
        leastHealth = 2.0;
        for (int i = 0; i < enemy.levelDirector.golems.Count; i++)
        {
            double golemHealthPercent = levelDirector.golems[i].health / levelDirector.golems[i].maxHealth;
            if (levelDirector.golems[i] != null && golemHealthPercent < leastHealth && levelDirector.golems[i].alive)
            {
                leastHealth = golemHealthPercent;
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
        Golem target = enemy.target;
        if (enemy.levelDirector.levelDone || target == null) return;
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

    public void DealDamage()
    {
        Golem target = enemy.target;
        if (enemy.levelDirector.levelDone || target == null) return;
        target.TakeDamage(enemy.strength);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
