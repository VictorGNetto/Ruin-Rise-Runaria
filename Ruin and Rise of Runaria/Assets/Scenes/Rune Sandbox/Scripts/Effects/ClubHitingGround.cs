using UnityEngine;

public class ClubHitingGround : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        animator.speed = 2;
        Destroy(gameObject, 0.5f);
    }

    public void SetOrderInLayer(int orderInLayer)
    {
        spriteRenderer.sortingOrder = orderInLayer;
    }
}
