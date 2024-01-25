using UnityEngine;

public class DestroyAfterPlay : MonoBehaviour
{
    public float frames;
    public float samples;

    private void Awake()
    {
        Destroy(gameObject, frames / samples);
    }
}
