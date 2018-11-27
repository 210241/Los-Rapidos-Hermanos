using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{

    public Transform PlayerOne;
    public Transform PlayerTwo;

    public Transform AveragePlayer;

    // Update is called once per frame
    void LateUpdate ()
    {

        float avgZ = (PlayerOne.position.z);// + PlayerTwo.position.z) / 2;
        float avgY = (PlayerOne.position.y);// + PlayerTwo.position.y) / 2;
        float avgX = (PlayerOne.position.x);// + PlayerTwo.position.x) / 2;

        transform.position = new Vector3(avgX, avgY + 3, avgZ - 15);
        AveragePlayer.position = new Vector3(avgX, avgY, avgZ);
        transform.LookAt(AveragePlayer);

    }
}
