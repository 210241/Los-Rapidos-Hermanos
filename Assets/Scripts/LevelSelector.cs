using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

    public void Select_Level_1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Select_Level_2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Select_Level_3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Back_To_Main_Menu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
