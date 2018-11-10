using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumNamespace;

public class GeneratePoints : MonoBehaviour
{

    public Transform Point;
    private Transform bottleInstance;

    private Quaternion noRotate = new Quaternion(0, 0, 0, 0);


    // Use this for initialization
    void Start()
    {
        if (!GameMaster.IsWallOnTheScreen)
        {
            var position = GetComponent<Transform>().position;
            var posX = position.x + Random.Range(-2, 3);
            var posY = position.y + (float)0.5;
            var posZ = position.z + Random.Range(0, 5);

            //noRotate.SetFromToRotation(new Vector3(0, 0, 0), new Vector3(0, 10, 0));

            bottleInstance = Instantiate(
                Point,
                new Vector3(posX, posY, posZ),
                new Quaternion(5, 30, 0, 0)
            );
            bottleInstance.name = Perks.BottlePoint.ToString();
            bottleInstance.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 3, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
