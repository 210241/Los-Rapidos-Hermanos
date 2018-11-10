using System.Collections;
using System.Collections.Generic;
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
}
