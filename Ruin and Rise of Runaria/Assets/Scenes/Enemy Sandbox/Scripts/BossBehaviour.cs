using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour: MonoBehaviour
{
    public GameObject enemyPrefab;
    public Golem target = null;
    public Transform launchOffset;
    public GameObject projectile = null;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    
    private float nextInvokeTime;
    private float nextMeleeTime;
    private float nextRengedTime;
    

    public float Meleerange;
    public float Ragedrange;


    public float InvokeCooldown;
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

        target = enemy.target;
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
            if(Time.time >= nextInvokeTime)
            {
                Invoke();
                nextInvokeTime = Time.time + InvokeCooldown;
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
    
    public void Invoke()
    {
        animator.SetTrigger("Invocation");
    }

    public void Ranged()
    {
        if(RandomGolem().transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (target.alive && target != null)
        {
            animator.SetTrigger("Long Distance");
            Vector3 direction = RandomGolem().transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject proj = Instantiate(projectile, launchOffset.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            proj.GetComponent<ProjectileScript>().target = RandomGolem();
            
        }
        else
        {
            target = RandomGolem();
        }
    }

    public void InvokeEnemies()
    {
        int numberOfEnemies = 3;

        for(int i = 0; i< numberOfEnemies; i++)
        {
            float offsetX = Random.Range(-2f, 2f);
            float offsetY = Random.Range(-2f, 2f);

            Vector3 spawnPosition = transform.position + new Vector3(offsetX, offsetY, 0);

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
