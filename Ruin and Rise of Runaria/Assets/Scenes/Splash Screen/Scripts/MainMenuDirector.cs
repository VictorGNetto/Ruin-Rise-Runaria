using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuDirector : MonoBehaviour
{
    public GameObject elements;
    public GameObject shadow;

    private float timer = 0;
    private bool enterAnimationDone = false;

    private bool exitScene = false;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!enterAnimationDone) {
            PlayEnterAnimation();
        }

        if (exitScene) {
            PlayExitAnimation();
        }
    }

    private void PlayEnterAnimation()
    {
        float fadeTime = 1.0f;
        float alpha = Mathf.Min(1, (timer) / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 3);

        if (alpha == 1) enterAnimationDone = true;
    }

    private void PlayExitAnimation()
    {
        float fadeTime = 0.5f;
        float alpha = Mathf.Min(1, timer / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = 1 - Mathf.Pow(alpha, 3);
        shadow.GetComponent<CanvasGroup>().alpha = 1 - Mathf.Pow(alpha, 3);
    }

    public void Play()
    {
        exitScene = true;
        timer = 0;
        Invoke("LoadLevelSelectionScene", 0.75f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void LoadLevelSelectionScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
