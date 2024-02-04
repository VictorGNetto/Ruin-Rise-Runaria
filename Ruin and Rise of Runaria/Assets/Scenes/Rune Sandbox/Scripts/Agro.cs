using UnityEngine;
public class Agro : MonoBehaviour
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

        Vector3 position = target.Position();

        transform.position = new Vector3(position.x, position.y + 1.1f, 0);
        spriteRenderer.sortingOrder = target.GetSortingOrder() - 1;
    }

    public void SetTarget(ICharacter target)
    {
        ready = true;
        Destroy(gameObject, 1.0f);
        this.target = target;
    }
}
