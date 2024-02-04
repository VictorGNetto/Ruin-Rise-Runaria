using UnityEngine;

public class Ray : MonoBehaviour
{
    public Color rayColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Destroy(gameObject, 8.0f / 12.0f);
    }

    public void Setup(int orderInLayer, Color color)
    {
        spriteRenderer.sortingOrder = orderInLayer;
        spriteRenderer.color = color;
    }
}
