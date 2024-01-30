using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectionMenu : MonoBehaviour
{

    public void Level1()
    {
        SceneManager.LoadScene(2);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
