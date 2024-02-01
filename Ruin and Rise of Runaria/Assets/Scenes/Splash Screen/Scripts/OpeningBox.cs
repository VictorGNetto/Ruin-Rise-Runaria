using UnityEngine;

public class OpenningBox : MonoBehaviour
{
    public OpeningDirector openingDirector;
    public int boxIndex;
    private float fadeTime = 1.0f;
    private bool animationDone = false;

    // Update is called once per frame
    void Update()
    {
        if (openingDirector.OpeningStarted()) {
            PlayFadeinAnimation();
        }
    }

    private void PlayFadeinAnimation()
    {
        float startTime = openingDirector.boxEnterTimes[boxIndex];
        float time = openingDirector.Timer();
        if (time < startTime || animationDone) return;

        float alpha = Mathf.Min(1, (time - startTime) / fadeTime);
        gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 3);

        if (alpha == 1) animationDone = true;
    }
}
