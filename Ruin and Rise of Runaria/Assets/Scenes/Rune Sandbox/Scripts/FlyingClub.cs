using UnityEngine;

public class FlyingClub : MonoBehaviour
{
    public Vector3 throwPosition;
    public ICharacter target;

    public GameObject clubHitingGroundPrefab;
    public GameObject hitAreaPrefab;
    public float explosionDamage;
    public float explosionArea;
    public float explosionDuration;
    public int whoThrow;

    private float flyingTime;
    private float timeSinceThrow = 0;
    
    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2000;
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceThrow += Time.deltaTime;
        
        float p = timeSinceThrow / flyingTime;
        transform.position = (1 - p) * throwPosition + p * target.Position();
    }

    public void SetFlyingTime(float flyingTime)
    {
        this.flyingTime = flyingTime;
        Invoke("DoDamage", flyingTime);
    }

    private void DoDamage()
    {
        GameObject hitArea = Instantiate(hitAreaPrefab);
        hitArea.transform.position = transform.position;
        hitArea.GetComponent<CircleHitArea>().SetDamage(explosionDamage);
        hitArea.GetComponent<CircleHitArea>().SetRadius(explosionArea);
        hitArea.GetComponent<CircleHitArea>().DestroyHitArea(explosionDuration);
        hitArea.GetComponent<CircleHitArea>().AddNotHitableCharacter(whoThrow);

        GameObject effect = Instantiate(clubHitingGroundPrefab);
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0);
        effect.transform.position = position;
        effect.GetComponent<ClubHitingGround>().SetOrderInLayer(target.GetSortingOrder() + 10);

        Destroy(gameObject);
    }
}
