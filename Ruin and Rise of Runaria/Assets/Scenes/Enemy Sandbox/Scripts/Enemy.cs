using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Health and Mana
    public float health = 75;
    public float maxHealth = 100;
    public EnemyHealthBar enemyHealthBar;

    public bool isDead = false;

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

    private void Die()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }
}
