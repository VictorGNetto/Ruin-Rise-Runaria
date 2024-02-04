using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ReaperBehaviour : MonoBehaviour
{
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    

    public float explosionRadius;
    private bool canAttack = true;
    private bool thereAreGolems = true;

    public void Start()
    {
        golemColliders = new Collider2D[6];
    }

    public LayerMask mask;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy.target = SelectTarget();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        mask = LayerMask.GetMask("Golem");
    }

    private void Update()
    {
        
        if (!enemy.alive) return;

        Golem target = enemy.target;
        
        if (thereAreGolems)
        {
            
            if (target == null || !target.Alive())
            {
                enemy.target = SelectTarget();
                target = enemy.target;
                
            }
            if (Vector2.Distance(transform.position, target.Position()) >= explosionRadius)
            {
                Walk();
                
            }
            else
            {
                animator.SetBool("Running", false);
            }

            if (Vector2.Distance(transform.position, target.Position()) <= explosionRadius && canAttack == true)
            {
                Attack();
            }
        }
    }

    

    Golem SelectTarget()
    {

        Golem bestTarget = null;
        int mostGolemsInArea;
        int golemsInArea;        
        LevelDirector levelDirector = enemy.levelDirector;
        golemColliders = new Collider2D[levelDirector.golems.Count];
        mostGolemsInArea = 0;

        for (int i = 0; i < enemy.levelDirector.golems.Count; i++)
        {
            
            golemsInArea = Physics2D.OverlapCircleNonAlloc(levelDirector.golems[i].Position(), explosionRadius, golemColliders);
            if (levelDirector.golems[i] != null && mostGolemsInArea < golemsInArea && levelDirector.golems[i].alive)
            {
                bestTarget = levelDirector.golems[i];
                mostGolemsInArea = golemsInArea;
            }
            
        }

        //if (bestTarget == null)
        //{
        //    thereAreGolems = false;
        //}
        
        return bestTarget;
    }

    public void Walk()
    {
        Golem target = enemy.target;
        animator.SetBool("Running", true);
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
    private Collider2D[] golemColliders;
    public void DealDamage()
    {
        
        int numberOfGolemsInArea = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRadius, golemColliders, mask);

        for (int i = 0; i < numberOfGolemsInArea; i++)
        {
            Debug.Log( golemColliders[i].name);
        }

        for (int i = 0; i < numberOfGolemsInArea; i++)
        {
            golemColliders[i].gameObject.GetComponent<Golem>().TakeDamage(enemy.strength);
        }
        
    }
    
    public void Attack()
    {
        navMeshAgent.speed = 0.5f;
        animator.SetTrigger("Charge");
        canAttack = false;
    }

    public GameObject explosionAnim;
    public void Explosion()
    {
        
        spriteRenderer.enabled = false;
        Destroy(animator);
        explosionAnim.SetActive(true);
        explosionAnim.GetComponent<Animator>().SetTrigger("Explode");
        DealDamage();
        enemy.Die();
        Destroy(gameObject, 1.5f);
    }
}
