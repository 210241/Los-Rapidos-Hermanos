using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    public Transform target;
	
	// Update is called once per frame
	void Update ()
	{
	    var targetpos = target.position;
		transform.LookAt(target);
        GetComponent<Transform>().position = new Vector3(targetpos.x, targetpos.y + 5, targetpos.z - 5);
	}
}
