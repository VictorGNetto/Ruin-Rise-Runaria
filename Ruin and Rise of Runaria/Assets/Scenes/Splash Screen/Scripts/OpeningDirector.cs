using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningDirector : MonoBehaviour
{
    public float[] boxEnterTimes = {0, 15, 30, 45};
    public GameObject continueButton;

    private bool openingStarted = false;
    private float timer = 0;
    private bool animationDone = false;

    private void Awake()
    {
        openingStarted = true;
        timer = 0;
    }

    private void Update()
    {
        if (animationDone) return;

        timer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit")) {
            timer += boxEnterTimes[3] / 3;
        }

        if (Input.GetButtonDown("Cancel")) {
            timer = boxEnterTimes[3] + 10;
        }

        if (timer > boxEnterTimes[3] + 5) {
            ShowContinueButton();
            animationDone = true;
        }
    }

    public bool OpeningStarted()
    {
        return openingStarted;
    }

    public float Timer()
    {
        return timer;
    }

    public void ShowContinueButton()
    {
        continueButton.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
