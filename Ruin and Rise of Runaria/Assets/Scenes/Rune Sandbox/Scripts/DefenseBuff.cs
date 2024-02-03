using UnityEngine;

public class DefenseBuff : MonoBehaviour
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
        target.IncreaseDefense(0.2f);
    }

    private void RemoveBuff()
    {
        target.IncreaseDefense(-0.2f);
    }
}
