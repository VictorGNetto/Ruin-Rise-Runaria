using System;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 5f;
    public Golem target;
    public float damage;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        target.health = Math.Max(0, target.health - damage);
        Destroy(gameObject);
    }
}
