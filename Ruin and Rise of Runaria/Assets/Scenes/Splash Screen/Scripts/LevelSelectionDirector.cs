using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionDirector : MonoBehaviour
{
    public GameObject elements;
    public GameObject shadow;
    public GameObject hardShadow;

    private float timer = 0;
    private bool enterAnimationDone = false;

    private bool goToMainMenu = false;
    private bool goToLevel = false;

    private void Start()
    {
        
        UnlockedLevels();
    }

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
            PlayExitAnimation(true);
        } else if (goToMainMenu) {
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

    private void PlayExitAnimation(bool insertHardShadow)
    {
        float fadeTime = 0.5f;
        float alpha = Mathf.Min(1, timer / fadeTime);
        elements.GetComponent<CanvasGroup>().alpha = 1 - Mathf.Pow(alpha, 3);
        if (insertHardShadow) {
            hardShadow.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 0.5f);
        } else {
            shadow.GetComponent<CanvasGroup>().alpha = Mathf.Pow(alpha, 3);
        }
    }

    public List <GameObject> buttons;

    public void UnlockedLevels ()
    {

        for (int i = 0; i < GameManager.unlockedLevels.Length; i++)
        {
            if (GameManager.unlockedLevels[i] == true)
            {
                buttons[i].SetActive(true);
            }
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
    public void Level2()
    {
        goToLevel = true;
        timer = 0;
        Invoke("LoadLevel2", 0.5f);
    }
    public void Level3()
    {
        goToLevel = true;
        timer = 0;
        Invoke("LoadLevel3", 0.5f);
    }
    public void Level4()
    {
        goToLevel = true;
        timer = 0;
        Invoke("LoadLevel4", 0.5f);
    }
    public void Level5()
    {
        goToLevel = true;
        timer = 0;
        Invoke("LoadLevel5", 0.5f);
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    private void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    private void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    private void LoadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }
    private void LoadLevel5()
    {
        SceneManager.LoadScene("Level5");
    }


}
