using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionDirector : MonoBehaviour
{
    public GameObject elements;
    public GameObject shadow;

    private float timer = 0;
    private bool enterAnimationDone = false;

    private bool goToMainMenu = false;
    private bool goToLevel = false;

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

        if (goToLevel) {
            PlayExitAnimation(false);
        } else if (goToMainMenu) {
            PlayExitAnimation(true);
        }
    }

    private void PlayEnterAnimation()
    {
        float fadeTime = 0.75f;
        float alpha = Mathf.Min(1, (timer) / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 3);

        if (alpha == 1) enterAnimationDone = true;
    }

    private void PlayExitAnimation(bool insertShadow)
    {
        float fadeTime = 0.5f;
        float alpha = Mathf.Min(1, timer / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = 1 - Mathf.Pow(alpha, 3);
        if (insertShadow) {
            shadow.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 3);
        }
    }

    public void MainMenu()
    {
        goToMainMenu = true;
        timer = 0;
        Invoke("LoadMainMenu", 0.5f);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Level1()
    {
        goToLevel = true;
        timer = 0;
        Invoke("LoadLevel1", 0.5f);
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {}

    public void Level3()
    {}

    public void Level4()
    {}

    public void Level5()
    {}
}
