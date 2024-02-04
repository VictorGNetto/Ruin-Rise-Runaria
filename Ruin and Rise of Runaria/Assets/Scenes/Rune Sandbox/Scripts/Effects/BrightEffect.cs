using UnityEngine;

public class BrightEffect : MonoBehaviour
{
    public ICharacter target;

    private SpriteRenderer spriteRenderer;
    private bool ready;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ready = false;
    }

    private void Update()
    {
        if (!ready) return;

        transform.position = target.Position();
        spriteRenderer.sortingOrder = target.GetSortingOrder() + 1;
    }

    public void SetAnimation(string animation)
    {
        gameObject.GetComponent<Animator>().Play(animation);
    }

    public void SetTarget(ICharacter target)
    {
        ready = true;
        Destroy(gameObject, 1.67f);
        this.target = target;
    }
}
