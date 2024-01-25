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
<<<<<<< Updated upstream
=======
    

    

>>>>>>> Stashed changes
    Golem ClosestGolem()
    {
        Golem closestGolem = null;
        float leastDistance;
        leastDistance = Mathf.Infinity;
<<<<<<< Updated upstream
        for(int i = 0; i < levelDirector.golems.Count; i++)
        {
            if (Vector3.Distance(transform.position, levelDirector.golems[i].transform.position) < leastDistance) {
=======
        for (int i = 0; i < levelDirector.golems.Count; i++)
        {
            if (Vector3.Distance(transform.position, levelDirector.golems[i].transform.position) < leastDistance)
            {
>>>>>>> Stashed changes
                leastDistance = Vector3.Distance(transform.position, levelDirector.golems[i].transform.position);
                closestGolem = levelDirector.golems[i];
            }
        }
        return closestGolem;
    }

    void Fire()
    {
        GameObject proj = Instantiate(projectile, launchOffset.position, transform.rotation);
<<<<<<< Updated upstream
=======
        //Animator anim = proj.GetComponent<Animator>();
        //anim.SetTrigger("Attack");
>>>>>>> Stashed changes
        proj.GetComponent<ProjectileScript>().target = ClosestGolem();
    }

    private void Start()
    {
        InvokeRepeating("Fire", 0, 1f);
    }

<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes
