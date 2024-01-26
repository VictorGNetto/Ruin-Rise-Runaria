using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform launchOffset;
    public LevelDirector levelDirector;
    public GameObject projectile = null;
    public Animator anim;
    public Vector3 scale;
    public SpriteRenderer spriteRenderer;
    


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
        
        return closestGolem;
    }

    IEnumerator Fire()
    {

        if(ClosestGolem().transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }


        anim.SetTrigger("Cast");

        yield return new WaitForSeconds(0.5f);

        Vector3 direction = ClosestGolem().transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject proj = Instantiate(projectile, launchOffset.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        
        //GameObject proj = Instantiate(projectile, launchOffset.position, Quaternion.identity);

        proj.GetComponent<ProjectileScript>().target = ClosestGolem();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Fire());
    }

    private void Start()
    {
        StartCoroutine(Fire());
        
        
    }

}
