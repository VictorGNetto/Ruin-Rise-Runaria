using UnityEngine;

public class Effect : MonoBehaviour
{
    private GameObject animationGO;

    private void Awake()
    {
        animationGO = transform.GetChild(0).gameObject;
    }

    public void SetOrderInLayer(int orderInLayer)
    {
        animationGO.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
    }
}
