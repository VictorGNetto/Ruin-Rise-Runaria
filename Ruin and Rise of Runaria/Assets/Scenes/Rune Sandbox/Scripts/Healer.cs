using UnityEngine;

public class Healer : MonoBehaviour
{
    private ICharacter target;
    private float totalHeal;
    private float healAmount;
    private float duration;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > duration) {
            this.Heal(totalHeal - healAmount);
            Destroy(gameObject);
            return;
        }

        float amount = totalHeal * Time.deltaTime / duration;
        healAmount += amount;
        this.Heal(amount);
    }

    public void Setup(ICharacter target, float totalHeal, float duration)
    {
        timer = 0;
        healAmount = 0;

        this.target = target;
        this.totalHeal = totalHeal;
        this.duration = duration;
    }

    private void Heal(float amount)
    {
        if (amount < 0) return;

        if (target != null && target.Alive()) {
            target.Heal(amount);
        }
    }
}
