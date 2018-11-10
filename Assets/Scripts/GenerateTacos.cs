using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTacos : MonoBehaviour {

    public Transform Taco;
    private Transform tacoInstance;

    // Use this for initialization
    void Start () {
		if(Random.Range(0,10) == 1)
        {
            var position = GetComponent<Transform>().position;
            var posX = position.x + Random.Range(-2,3); 
            var posY = position.y + (float)0.5;
            var posZ = position.z + Random.Range(0, 5);

            //noRotate.SetFromToRotation(new Vector3(0, 0, 0), new Vector3(0, 10, 0));

             tacoInstance = Instantiate(
                  Taco,
                  new Vector3(posX, posY, posZ),
                  new Quaternion(0,180,80,0)
            );
            tacoInstance.name = Perks.TacoIncreaseSpeed.ToString();
            tacoInstance.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 3, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {

    }
}
