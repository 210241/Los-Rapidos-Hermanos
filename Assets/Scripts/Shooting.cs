using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,10);

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == Tags.Cactus.ToString())
        {
            Destroy(other.gameObject);
           // Destroy(GetComponent<Rigidbody>().gameObject);
        }

            Destroy(GetComponent<Rigidbody>().gameObject);
    }


    }
