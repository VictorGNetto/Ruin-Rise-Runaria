using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeOrderInLayer : MonoBehaviour
{
    public float maxY = 3.7f;
    public float minY = -3.7f;
    public int orderInLayer = 0;
    public GameObject[] parts;

    // Update is called once per frame
    void Update()
    {
        this.UpdateOrderInLayer();
        this.ApplyOrderInLayer();
    }

    void UpdateOrderInLayer()
    {
        float scale = 1000;
        this.orderInLayer = (int) -Mathf.Floor(scale * transform.position.y / (maxY - minY));
    }

    void ApplyOrderInLayer()
    {
        foreach (GameObject part in this.parts)
        {
            part.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
        }
    }
}
