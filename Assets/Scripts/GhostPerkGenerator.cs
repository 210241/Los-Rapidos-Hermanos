using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;

public class GhostPerkGenerator : MonoBehaviour
{

    public Transform GhostPerk;
    private Transform ghostGeneratorInstance;


    // Use this for initialization
    void Start()
    {

        if (!GameMaster.IsWallOnTheScreen)
        {
            if (Random.Range(0, 10) == 1)
            {
                var position = GetComponent<Transform>().position;
                var posX = position.x + Random.Range(-2, 3);
                var posY = position.y + (float)0.5;
                var posZ = position.z + Random.Range(0, 5);

                //noRotate.SetFromToRotation(new Vector3(0, 0, 0), new Vector3(0, 10, 0));

                ghostGeneratorInstance = Instantiate(
                    GhostPerk,
                    new Vector3(posX, posY, posZ),
                    new Quaternion(5, 30, 0, 0)
                );
                ghostGeneratorInstance.name = Perks.GhostPerk.ToString();
                ghostGeneratorInstance.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 3, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
