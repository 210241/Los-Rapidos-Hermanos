using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePerks : MonoBehaviour
{

    public Transform Reverse;
    private Transform ReverseInstance;

    public Transform BlockShooting;
    private Transform BlockShootingInstance;

    public Transform GhostPerk;
    private Transform ghostGeneratorInstance;

    public Transform Taco;
    private Transform tacoInstance;

    // Use this for initialization
    void Start()
    {
        if (!GameMaster.IsWallOnTheScreen)
        {
            var position = GetComponent<Transform>().position;

            #region ReverseControls
            if (Random.Range(0, 10) == 1)
            {
                var posX = position.x + Random.Range(-2, 3);
                var posY = position.y + 0.7f;
                var posZ = position.z + Random.Range(0, 5);

                ReverseInstance = Instantiate(
                    Reverse,
                    new Vector3(posX, posY, posZ),
                    new Quaternion(0, 0, 0, 0)
                );
                ReverseInstance.name = Perks.ReverseControls.ToString();
                ReverseInstance.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 3, 0);
            }
            #endregion

            #region BlockShooting
            if (Random.Range(0, 10) == 2)
            {
                var posX = position.x + Random.Range(-2, 3);
                var posY = position.y + 0.7f;
                var posZ = position.z + Random.Range(0, 5);

                BlockShootingInstance = Instantiate(
                    BlockShooting,
                    new Vector3(posX, posY, posZ),
                    new Quaternion(0, 0, 0, 0)
                );
                BlockShootingInstance.name = Perks.BlockShooting.ToString();
                BlockShootingInstance.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 3, 3);
            }
            #endregion

            #region Ghost
            if (Random.Range(0, 10) == 1)
            {
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
            #endregion

            #region TacoSpeed
            if (Random.Range(0, 10) == 1)
            {
                var posX = position.x + Random.Range(-2, 3);
                var posY = position.y + (float)0.5;
                var posZ = position.z + Random.Range(0, 5);

                //noRotate.SetFromToRotation(new Vector3(0, 0, 0), new Vector3(0, 10, 0));

                tacoInstance = Instantiate(
                    Taco,
                    new Vector3(posX, posY, posZ),
                    new Quaternion(0, 180, 80, 0)
                );
                tacoInstance.name = Perks.TacoIncreaseSpeed.ToString();
                tacoInstance.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 3, 0);
            }
            #endregion

        }
    }
}