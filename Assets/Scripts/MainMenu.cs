﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play_Game()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Quit_Game()
    {
        Application.Quit();
    }
}
