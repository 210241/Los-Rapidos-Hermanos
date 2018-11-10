using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCactusOnFloor : MonoBehaviour
{

    public Transform Cactus;

    private Quaternion noRotate = new Quaternion(0, 0, 0, 0);

    private List<Transform> Cactie = new List<Transform>();

    // Use this for initialization
    void Start ()
    {
        var position = GetComponent<Transform>().position;
        int numberOfCactie = Random.Range(1, 4);
        for (int i = 0; i < numberOfCactie; i++)
        {
            Cactie.Add(
                Instantiate(Cactus,
                    new Vector3(position.x + Random.Range(-2, 3), position.y, position.z + Random.Range(0, 5)),
                    noRotate)
            );
        }
    }

    void OnDestroy()
    {
        Cactie.ForEach(cactus => Destroy(cactus.gameObject));
    }
}
