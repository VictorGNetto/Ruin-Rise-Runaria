using System;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    private bool hit;
    private BoxCollider2D boxCollider;
    private Animator anim;
>>>>>>> Stashed changes
    public float speed = 5f;
    public Golem target;
    public float damage;

<<<<<<< Updated upstream
=======

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }


>>>>>>> Stashed changes
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
<<<<<<< Updated upstream
        target.health = Math.Max(0, target.health - damage);
        Destroy(gameObject);
    }
}
=======
        boxCollider.enabled = false;
        anim.SetTrigger("Explode");


        target.health = Math.Max(0, target.health - damage);
        Destroy(gameObject, 0.2f);
    }
}
>>>>>>> Stashed changes
