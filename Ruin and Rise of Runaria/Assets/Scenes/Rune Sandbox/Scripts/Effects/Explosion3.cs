using UnityEngine;

public class Explosion3 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Destroy(gameObject, 0.5f);
    }

    public void SetOrderInLayer(int orderInLayer)
    {
        spriteRenderer.sortingOrder = orderInLayer;
    }
}
