using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealthBars : MonoBehaviour
{
    public Transform Background;
    public Transform ForegroundRed;
    public Transform ForegroundBlue;

    private Transform backgroundInstance1;
    private Transform backgroundInstance2;
    private Transform foregroundRedInstance;
    private Transform foregroundBlueInstance;



    // Use this for initialization
    void Start ()
    {

        //backgroundInstance1 = Instantiate(Background, new Vector3(-3.0f, 1f, 0f), GameMaster.noRotate);
        //foregroundRedInstance = Instantiate(ForegroundRed, new Vector3(-3f, 1, 0f), GameMaster.noRotate);

        //backgroundInstance2 = Instantiate(Background, new Vector3(3.0f, 5f, 0f), GameMaster.noRotate);
        //foregroundBlueInstance = Instantiate(ForegroundBlue, new Vector3(3f, 5f, 0f), GameMaster.noRotate);

    }
	
	// Update is called once per frame
	void Update ()
	{
        if (GameMaster.wallInstance == null)
        {
            Destroy(backgroundInstance1);
            Destroy(foregroundRedInstance);
            Destroy(backgroundInstance2);
            Destroy(foregroundBlueInstance);
            backgroundInstance1 = null;

        }
        else
        {
            if (backgroundInstance1 == null)
            {
                backgroundInstance1 = Instantiate(Background, new Vector3(-3.0f, 1f, 0f), GameMaster.noRotate);
                foregroundRedInstance = Instantiate(ForegroundRed, new Vector3(-3f, 1, 0f), GameMaster.noRotate);

                backgroundInstance2 = Instantiate(Background, new Vector3(3.0f, 5f, 0f), GameMaster.noRotate);
                foregroundBlueInstance = Instantiate(ForegroundBlue, new Vector3(3f, 5f, 0f), GameMaster.noRotate);
            }
        }


        backgroundInstance1.GetComponent<Transform>().position = new Vector3(-3f, 1f, GameMaster.wallInstance.transform.position.z - 1f + 0.1f);
        foregroundRedInstance.GetComponent<Transform>().position = new Vector3(-3f, 1f, GameMaster.wallInstance.transform.position.z - 1f);

	    backgroundInstance2.GetComponent<Transform>().position = new Vector3(3f, 1f, GameMaster.wallInstance.transform.position.z - 1f + 0.1f);
	    foregroundBlueInstance.GetComponent<Transform>().position = new Vector3(3f, 1f, GameMaster.wallInstance.transform.position.z - 1f);

        float ratioRed = ((float)GameMaster.CurrentWallHealthPlayerOne / (float)GameMaster.MaxWallHealth);
        foregroundRedInstance.GetComponent<Transform>().localScale = new Vector3(ratioRed, 0.2f, 0.1f);

	    float ratioBlue = ((float)GameMaster.CurrentWallHealthPlayerTwo / (float)GameMaster.MaxWallHealth);
	    foregroundRedInstance.GetComponent<Transform>().localScale = new Vector3(ratioBlue, 0.2f, 0.1f);
    }
}
