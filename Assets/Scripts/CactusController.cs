using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumNamespace;

public class CactusController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collision other)
    {
        if(other.gameObject.name== Objects.Bullet.ToString())
        {
            Destroy(GetComponent<Transform>().gameObject);
        }
    }
}
