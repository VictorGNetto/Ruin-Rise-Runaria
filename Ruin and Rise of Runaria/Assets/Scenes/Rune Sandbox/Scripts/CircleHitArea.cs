using System.Collections.Generic;
using UnityEngine;

public class CircleHitArea : MonoBehaviour
{
    public float damage;

    private HashSet<int> hitedEntities = new HashSet<int>();

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetRadius(float radius)
    {
        gameObject.GetComponent<CircleCollider2D>().radius = radius;
    }

    public void DestroyHitArea(float timeToLive)
    {
        Destroy(gameObject, timeToLive);
    }

    public void AddNotHitableCharacter(int guid)
    {
        hitedEntities.Add(guid);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ICharacter character = collider.gameObject.GetComponent<ICharacter>();
        int guid = character.GUID();

        if (!hitedEntities.Contains(guid)) {
            hitedEntities.Add(guid);

            character.TakeDamage(damage);
        }
    }
}
