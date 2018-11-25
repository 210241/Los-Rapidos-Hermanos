using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{

    public Image currentHealthbarPlayerOne;
    public Image currentHealthbarPlayerTwo;
    public TextMeshProUGUI numberOfLivesPlayerOne;
    public TextMeshProUGUI numberOfLivesPlayerTwo;

    public void Start()
    {
        UpdateHealthbarPlayerOne();
        UpdateHealthbarPlayerTwo();
    }

    public void Update()
    {
        UpdateHealthbarPlayerOne();
        UpdateHealthbarPlayerTwo();
    }
    public void UpdateHealthbarPlayerOne()
    {
        float hit = (float)GameMaster.PlayerOneLives / 3.0f;
        currentHealthbarPlayerOne.rectTransform.localScale = new Vector3(hit, 1, 1);
        UpdateHealthTextPlayerOne();
    }

    public void UpdateHealthbarPlayerTwo()
    {
        float hit = (float)GameMaster.PlayerTwoLives / 3.0f;
        currentHealthbarPlayerTwo.rectTransform.localScale = new Vector3(hit, 1, 1);
        UpdateHealthTextPlayerTwo();
    }

    public void UpdateHealthTextPlayerOne()
    {
        numberOfLivesPlayerOne.text = GameMaster.PlayerOneLives.ToString();
    }

    public void UpdateHealthTextPlayerTwo()
    {
        numberOfLivesPlayerTwo.text = GameMaster.PlayerTwoLives.ToString();
    }
}
