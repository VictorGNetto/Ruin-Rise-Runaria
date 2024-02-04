using System;
using UnityEngine;

public class RunicSpell : MonoBehaviour
{
    public float damage;
    public int whoThrow;
    public bool autoTarget;

    private Vector3 v0;

    private float flyingTime;
    private float timeSinceThrow;
    private Vector3 throwPosition;
    private Vector3 targetPosition;
    private bool ready = false;

    private void Update()
    {
        timeSinceThrow += Time.deltaTime;

        if (timeSinceThrow > flyingTime) {
            Destroy(gameObject);
        }

        float p = timeSinceThrow / flyingTime;
        float q = Mathf.Pow(p, 0.5f);
        Vector3 velocityBased = throwPosition +  timeSinceThrow * v0;
        Vector3 targetBased = (1 - p) * throwPosition + p * targetPosition;
        transform.position = (1 - q) * velocityBased + q * targetBased;

    }

    public void Setup(float flyingTime, Vector3 throwPosition, Vector3 targetPosition)
    {
        timeSinceThrow = 0;
        this.flyingTime = flyingTime;

        this.throwPosition = throwPosition;
        this.targetPosition = targetPosition;
        transform.position = throwPosition;

        // Compute v0
        float a = UnityEngine.Random.Range(-45.0f, 45.0f);  // angle dispersion
        float b = UnityEngine.Random.Range(15.0f, 20.0f);     // initial velocity (absolute)
        float toDegreeFactor = (180.0f / Mathf.PI);
        float angle = Mathf.Atan2(targetPosition.y - throwPosition.y, targetPosition.x - throwPosition.x) * toDegreeFactor;

        angle += a;
        float angleRad = angle / toDegreeFactor;
        float v0x = Mathf.Cos(angleRad) * b;
        float v0y = Mathf.Sin(angleRad) * b;
        v0 = new Vector3(v0x, v0y, 0);
        if (autoTarget) {
            v0 = -v0;
        }

        ready = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (timeSinceThrow / flyingTime < 0.10f || !ready) return;

        ICharacter character = collider.gameObject.GetComponent<ICharacter>();
        int guid = character.GUID();

        if (guid == whoThrow) {
            character.TakeDamage(damage * 0.3f);
        } else if (IsGolem(guid)) {
            character.TakeDamage(damage * 0.5f);
        } else {
            character.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private bool IsGolem(int guid)
    {
        return guid < 100;
    }
}
