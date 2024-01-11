using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeOrderInLayer : MonoBehaviour
{
    public float maxY = 3.7f;
    public float minY = -3.7f;
    public int orderInLayer = 0;
    public GameObject Head;
    public GameObject Body;
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject LeftLeg;
    public GameObject RightLeg;

    void Update()
    {
        this.UpdateOrderInLayer();
        this.ApplyOrderInLayer();
    }

    void UpdateOrderInLayer()
    {
        float scale = 1000;
        this.orderInLayer = (int) -Mathf.Floor(scale * transform.position.y / (maxY - minY));
        this.orderInLayer = 3 * this.orderInLayer;
    }

    void ApplyOrderInLayer()
    {
        // Back layer
        LeftArm.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 0;
        LeftLeg.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 0;

        // Central layer
        Body.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 1;

        // Front layer
        Head.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 2;
        RightArm.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 2;
        RightLeg.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 2;
    }
}
