using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectionMenu : MonoBehaviour
{

    public void Back()
    {
        SceneManager.LoadScene("SplashScreen");
    }
}
