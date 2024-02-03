using UnityEngine;

public class AttackBuff : MonoBehaviour
{
    private ICharacter target;
    private float timer;
    private float duration = 8.0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > duration) {
            RemoveBuff();
            Destroy(gameObject);
        }
    }

    public void Setup(ICharacter target)
    {
        timer = 0;
        this.target = target;

        GiveBuff();
    }

    private void GiveBuff()
    {
        target.IncreaseAttack(7.5f);
    }

    private void RemoveBuff()
    {
        target.IncreaseAttack(-7.5f);
    }
}
