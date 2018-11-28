using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    public Image levelProgress;
    private float progress;

    public void Start()
    {
        progress = 0.0f;
        levelProgress.rectTransform.localScale = new Vector3(progress, 1, 1);
        UpdateProgressBar();
    }

    public void Update()
    {
        UpdateProgressBar();
    }
    public void UpdateProgressBar()
    {
        progress = (GameMaster.AveragePlayersZDimension) / (float) GameMaster.StaticLevelLenght;
        levelProgress.rectTransform.localScale = new Vector3(progress, 1, 1);
    }
}
