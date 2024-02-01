using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningDirector : MonoBehaviour
{
    public float[] boxEnterTimes = {0.5f, 10, 20, 30, 35};

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
    }

    public bool OpeningStarted()
    {
        return openingStarted;
    }

    public float Timer()
    {
        return timer;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
