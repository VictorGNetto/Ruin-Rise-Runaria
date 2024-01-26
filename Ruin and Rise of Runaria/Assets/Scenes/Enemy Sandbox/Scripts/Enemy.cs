using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public EnemyHealthBar enemyHealthBar;

    public bool isDead = false;

    private int guid;

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        enemyHealthBar.SetHealth(health, maxHealth);

        if (health <= 0) {
            isDead = true;
            enemyHealthBar.gameObject.SetActive(false);
            Die();
        }
    }

    public ICharacter Target()
    {
        return gameObject.GetComponent<Enemy>();
    }

    public Vector3 TargetPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public Vector3 Position()
    {
         return new Vector3(0, 0, 0);
    }

    public void Die()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }

    public void TakeDamage(float amount)
    {}

    public int GUID()
    {
        return guid;
    }

    public int GetSortingOrder()
    {
        return 0;
    }
}
