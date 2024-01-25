using UnityEngine;

public class ComputeOrderInLayerStatic : MonoBehaviour
{
    public float maxY = 3.7f;
    public float minY = -3.7f;
    public int orderInLayer = 0;

    private void Awake()
    {
        UpdateOrderInLayer();
    }

    private void UpdateOrderInLayer()
    {
        float scale = 1000;
        orderInLayer = (int) -Mathf.Floor(scale * transform.position.y / (maxY - minY));

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
    }
}
