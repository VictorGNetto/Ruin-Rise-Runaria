using UnityEngine;

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


    public void Quit()
    {
    }

    public void OpenTips()
    {
        gameObject.SetActive(false);
        tutorial.Open();
    }
}
