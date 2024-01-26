using UnityEngine;

public class Heal : MonoBehaviour
{
    public ICharacter target;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.Position();
        spriteRenderer.sortingOrder = target.GetSortingOrder() - 1;
    }
}
