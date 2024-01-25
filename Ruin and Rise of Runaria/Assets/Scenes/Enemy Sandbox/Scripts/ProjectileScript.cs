using System;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private bool hit;
    private BoxCollider2D boxCollider;
    private Animator anim;
    public float speed = 5f;
    public Golem target;
    public float damage;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        boxCollider.enabled = false;
        anim.SetTrigger("Explode");


        target.health = Math.Max(0, target.health - damage);
        Destroy(gameObject, 0.2f);
    }
}
