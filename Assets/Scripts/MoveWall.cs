using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;

public class MoveWall : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        var orb = GetComponent<Rigidbody>();
        orb.velocity = new Vector3(0, 0, GameMaster.AvgOrbsVelocity.z - 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        var orb = GetComponent<Rigidbody>();
        orb.velocity = new Vector3(0, 0, GameMaster.AvgOrbsVelocity.z - 1.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == Tags.Cactus.ToString())
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == Perks.TacoIncreaseSpeed.ToString())
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == Perks.BottlePoint.ToString())
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == Perks.BlockShooting.ToString())
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == Perks.ReverseControls.ToString())
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == Perks.GhostPerk.ToString())
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == Players.PlayerOne.ToString() ||
            other.gameObject.name == Players.PlayerTwo.ToString())
        {
            Destroy(other.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == Players.PlayerOne.ToString() || other.gameObject.name == Players.PlayerTwo.ToString())
        {
            var position = other.gameObject.transform.position;
            other.gameObject.transform.position = new Vector3(position.x, position.y, position.z - 5);
        }
    }
}
