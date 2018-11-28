using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    private static int currentLevel = 1;
    public void Next_level()
    {
        currentLevel++;
        if(currentLevel < 4)
            SceneManager.LoadScene(currentLevel);
        else
        {
            currentLevel = 1;
            SceneManager.LoadScene("GameOver");
        }
    }
}
