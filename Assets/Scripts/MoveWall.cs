using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;

public class MoveWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    var orb = GetComponent<Rigidbody>();
	    orb.velocity = new Vector3(0, 0, 4.5f);
    }
	
	// Update is called once per frame
	void Update () {
	    
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




    }

}
