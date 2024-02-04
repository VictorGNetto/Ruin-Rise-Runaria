using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public HUD hud;
    public Tutorial tutorial;

    public void Open()
    {
        hud.StopTicTac();
        gameObject.SetActive(true);
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        hud.ResumeTicTac();
    }

    public void LevelSelection()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("LevelSelection");
    }

    public void Quit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenTips()
    {
        gameObject.SetActive(false);
        tutorial.Open();
    }
}
