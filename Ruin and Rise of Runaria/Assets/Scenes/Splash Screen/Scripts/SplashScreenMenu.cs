using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenMenu : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
