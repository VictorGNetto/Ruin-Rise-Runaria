using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
   public void PlayGame()
   {
        SceneManager.LoadScene("LevelSelection");
   }

   public void QuitGame()
   {
        Application.Quit();
   }

}
