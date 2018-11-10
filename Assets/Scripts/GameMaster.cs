using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Camera MainCamera;

    public Text PlayerOneScore;
    public Text PlayerTwoScore;

    public Transform BasicFloor;
    public Transform CrossHoleFloor;
    public Transform BigCrossHoleFloor;
    public Transform SatelliteHoleFloor;
    public Transform BaseCactus;
    public Transform Perk;
    public Transform Orb_Player1;
    public Transform Orb_Player2;
    public Transform Wall;

    public static int PlayerOnePoints;
    public static int PlayerTwoPoints;

    public static bool PlayerOneIsAlive = true;
    public static bool PlayerTwoIsAlive = true;
    public static bool IsWallOnTheScreen = false;

    public static int PlayerOneControlReversedMultiplier = 1;
    public static int PlayerTwoControlReversedMultiplier = 1;

    public static Quaternion noRotate = new Quaternion(0, 0, 0, 0);
    List<Transform> PossibleFloors = new List<Transform>();
    Queue<Transform> ListOfFloors = new Queue<Transform>();

    private Transform orbInstancePlayer1;
    private Transform orbInstancePlayer2;
    private Camera mainCameraInstance;
    private int floorsWithoutWall;

    

    public static bool startSpawningCactie = false;

    //private Random random = new Random();
    private void PopulatePossibleFloorsList()
    {
        PossibleFloors.Add(BasicFloor);
        PossibleFloors.Add(CrossHoleFloor);
        PossibleFloors.Add(BigCrossHoleFloor);
        PossibleFloors.Add(SatelliteHoleFloor);
    }

    private Transform GetRandomBlock()
    {
        int randInt = Random.Range(0, PossibleFloors.Count);
        return PossibleFloors[randInt];
    }

    // Use this for initialization
    void Start()
    {
        startSpawningCactie = false;

        mainCameraInstance = Instantiate(MainCamera, new Vector3(0f, 3.14f, -2.34f), noRotate);
        mainCameraInstance.transform.Rotate(42.15f, 0, 0);

        orbInstancePlayer1 = Instantiate(Orb_Player1, new Vector3(-1, 0, 0), noRotate);
        orbInstancePlayer2 = Instantiate(Orb_Player2, new Vector3(1, 0, 0), noRotate);
        orbInstancePlayer1.name = Players.PlayerOne.ToString();
        orbInstancePlayer2.name = Players.PlayerTwo.ToString();


        ListOfFloors.Enqueue(Instantiate(BasicFloor, new Vector3(0, 0, 0), noRotate));
        ListOfFloors.Enqueue(Instantiate(BasicFloor, new Vector3(0, 0, 5), noRotate));


        PopulatePossibleFloorsList();


        for (int i = 2; i < 10; i++)
        {
            ListOfFloors.Enqueue(Instantiate(GetRandomBlock(), new Vector3(0, 0, i * 5), noRotate));
        }

    }

    // Update is called once per frame
    private void Update()
    {
        startSpawningCactie = true;
        if (orbInstancePlayer1 == null)
        {
            orbInstancePlayer1 = orbInstancePlayer2;
        }
        if (orbInstancePlayer2 == null)
        {
            orbInstancePlayer2 = orbInstancePlayer1;
        }

        if (orbInstancePlayer1.position.z > ListOfFloors.Peek().position.z + 20)
        {
            var firstFloor = ListOfFloors.Dequeue();
            Destroy(firstFloor.gameObject);
            ListOfFloors.Enqueue(Instantiate(GetRandomBlock(), new Vector3(0, 0, orbInstancePlayer1.position.z + 25), noRotate));

            if (!IsWallOnTheScreen)
                floorsWithoutWall++;

        }
        var averageOrbZ = (orbInstancePlayer1.position.z + orbInstancePlayer2.position.z) / 2;
        mainCameraInstance.transform.position = new Vector3(mainCameraInstance.transform.position.x, mainCameraInstance.transform.position.y, averageOrbZ - 2.5f);

        PlayerOneScore.text = PlayerOnePoints.ToString();
        PlayerTwoScore.text = PlayerTwoPoints.ToString();

        if (floorsWithoutWall == 10)
        {
            IsWallOnTheScreen = true;
            Instantiate(Wall, new Vector3(0, 1.8f, orbInstancePlayer1.position.z + 10 ), noRotate);          

            floorsWithoutWall = 0;
        }

    }
}