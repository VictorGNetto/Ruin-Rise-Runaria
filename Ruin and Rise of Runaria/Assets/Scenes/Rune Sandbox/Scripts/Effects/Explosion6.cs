using UnityEngine;

public class Explosion6 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Destroy(gameObject, 10.0f / 15.0f);
    }

    public void Setup(int orderInLayer, Color color)
    {
        spriteRenderer.sortingOrder = orderInLayer;
        spriteRenderer.color = color;
    }
}
