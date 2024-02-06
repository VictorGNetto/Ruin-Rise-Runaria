using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuDirector : MonoBehaviour
{
    public GameObject elements;
    public GameObject shadow;

    private float timer = 0;
    private bool enterAnimationDone = false;

    private bool goToLevelSelection = false;
    private bool goToOpening = false;

    private void Awake()
    {
        timer = 0;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!enterAnimationDone) {
            PlayEnterAnimation();
        }

        if (goToLevelSelection) {
            PlayExitAnimation(true);
        } else if (goToOpening) {
            PlayExitAnimation(false);
        }
    }

    private void PlayEnterAnimation()
    {
        float fadeTime = 0.75f;
        float alpha = Mathf.Min(1, (timer) / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 3);

        if (alpha == 1) enterAnimationDone = true;
    }

    private void PlayExitAnimation(bool removeShadow)
    {
        float fadeTime = 0.5f;
        float alpha = Mathf.Min(1, timer / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = 1 - Mathf.Pow(alpha, 3);
        if (removeShadow) {
            shadow.GetComponent<CanvasGroup>().alpha = 1 - Mathf.Pow(alpha, 3);
        }
    }

    public void Opening()
    {
        goToOpening = true;
        timer = 0;
        Invoke("LoadOpening", 0.5f);
    }

    private void LoadOpening()
    {
        SceneManager.LoadScene("Opening");
    }

    public void Play()
    {
        goToLevelSelection = true;
        timer = 0;
        Invoke("LoadLevelSelectionScene", 0.5f);
    }

    private void LoadLevelSelectionScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
