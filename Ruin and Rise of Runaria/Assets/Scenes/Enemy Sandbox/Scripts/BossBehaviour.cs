using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour: MonoBehaviour
{
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float nextMeleeTime;
    private float nextRengedTime;

    public float Meleerange;
    public float Ragedrange;


    public float MeleeCooldown;
    public float RengedCooldown;

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

        Golem target = enemy.target;
        if (thereAreGolems)
        {
            if (target == null || !target.Alive())
            {
                enemy.target = RandomGolem();
                target = enemy.target;
            }

            if (Vector2.Distance(transform.position, target.Position()) >= Meleerange)
            {
                Walk();
            }
            else
            {
                animator.SetBool("Walking", false);
            }

            if (Vector2.Distance(transform.position, target.Position()) <= Meleerange && Time.time >= nextMeleeTime)
            {
                Melee();
                nextMeleeTime = Time.time + MeleeCooldown;
            }
            if (Vector2.Distance(transform.position, target.Position()) <= Ragedrange && Time.time >= nextRengedTime)
            {
                Ranged();
                nextRengedTime = Time.time + RengedCooldown;
            }
        }
    }


    Golem RandomGolem()
    {
        Golem RandomGolem = null;
        LevelDirector levelDirector = enemy.levelDirector;
        int i = Random.Range(0,(enemy.levelDirector.golems.Count)-1);
        RandomGolem = levelDirector.golems[i];
        
        if (RandomGolem == null)
        {
            thereAreGolems = false;
        }

        return RandomGolem;
    }

    public void Walk()
    {
        Golem target = enemy.target;
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
        enemy.target.TakeDamage(enemy.strength);
    }

    public void DealDamageRange()
    {
        enemy.target.TakeDamage(enemy.strength - 5);
    }

    public void Melee()
    {
        animator.SetTrigger("Melee");
    }
     public void Ranged()
    {
        animator.SetTrigger("Long Distance");
    }
}
