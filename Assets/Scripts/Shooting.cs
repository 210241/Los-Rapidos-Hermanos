using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject PlayerObject;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 20);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == Tags.Cactus.ToString())
        {
            Destroy(other.gameObject);
            // Destroy(GetComponent<Rigidbody>().gameObject);
        }
        else if (other.gameObject.tag == Tags.Wall.ToString())
        {
            if (PlayerObject.name == Players.PlayerOne.ToString())
            {
                if (GameMaster.CurrentWallHealthPlayerOne > 0)
                    GameMaster.CurrentWallHealthPlayerOne--;

            }
            else
            {
                if (GameMaster.CurrentWallHealthPlayerTwo > 0)
                    GameMaster.CurrentWallHealthPlayerTwo--;
            }



            if (GameMaster.CurrentWallHealthPlayerOne == 0 && GameMaster.CurrentWallHealthPlayerTwo == 0)
            {
                Destroy(other.gameObject);
                GameMaster.wallInstance = null;
                GameMaster.IsWallOnTheScreen = false;
                GameMaster.CurrentWallHealthPlayerOne = GameMaster.MaxWallHealth;
                GameMaster.CurrentWallHealthPlayerTwo = GameMaster.MaxWallHealth;

                GameMaster.NeedToPopulateFloorQueue = true;
            }
        }


        Destroy(GetComponent<Rigidbody>().gameObject);
    }


}
