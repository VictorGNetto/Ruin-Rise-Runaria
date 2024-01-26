using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MinotaurBehaviour: MonoBehaviour
{
    // Start is called before the first frame update
    public Transform launchOffset;
    public LevelDirector levelDirector;
    public GameObject projectile = null;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public NavMeshAgent agent;
    public float range;
    public float damage;
    public float attackCooldown;

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
            if (levelDirector.golems[i] != null && levelDirector.golems[i].health < leastHealth)
            {
                leastHealth = levelDirector.golems[i].health;
                lowestGolem = levelDirector.golems[i];
            }
        }

        return lowestGolem;
    }

    public void Walk()
    {
        anim.SetBool("Walking", true);
        agent.SetDestination(LowestGolem().transform.position);
        if (LowestGolem().transform.position.x < transform.position.x)
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
        if (Vector2.Distance(transform.position, LowestGolem().transform.position) >= range)
        {
            Walk();
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (Vector2.Distance(transform.position, LowestGolem().transform.position) <= range && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }


    }

    public void Attack()
    {

        


        anim.SetTrigger("Attack");
        LowestGolem().health = Mathf.Max(0, LowestGolem().health - damage);
        
        
    }

    

}
