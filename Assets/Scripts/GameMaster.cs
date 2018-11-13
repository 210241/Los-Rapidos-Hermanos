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

    public static int PlayerOneLives = 3;
    public static int PlayerTwoLives = 3;

    public static Transform wallInstance;

    public static Quaternion noRotate = new Quaternion(0, 0, 0, 0);
    List<Transform> PossibleFloors = new List<Transform>();
    Queue<Transform> ListOfFloors = new Queue<Transform>();

    private Transform orbInstancePlayer1;
    private Transform orbInstancePlayer2;
    private Camera mainCameraInstance;
    private int floorsWithoutWall;

    public static Vector3 AvgOrbsVelocity; 
    public static Vector3 orbVelocity1;
    public static Vector3 orbVelocity2;

    public static int MaxWallHealth = 5;

    public static int CurrentWallHealthPlayerOne = 5;

    public static int CurrentWallHealthPlayerTwo = 5;
    //public static float StandardVerticalOrbPositon;

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

        orbInstancePlayer1 = Orb_Player1;
        orbInstancePlayer2 = Orb_Player2;
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

        var playerVelocity1 = orbInstancePlayer1.gameObject.GetComponent<Rigidbody>().velocity;
        var playerVelocity2 = orbInstancePlayer2.gameObject.GetComponent<Rigidbody>().velocity;

        AvgOrbsVelocity = new Vector3(0, 0, (playerVelocity1.z + playerVelocity2.z) / 2);

        PlayerOneScore.text = PlayerOnePoints.ToString();
        PlayerTwoScore.text = PlayerTwoPoints.ToString();

        if (floorsWithoutWall == 20)
        {
            IsWallOnTheScreen = true;
            wallInstance = Instantiate(Wall, new Vector3(0, 1.8f, orbInstancePlayer1.position.z + 10), noRotate);

            floorsWithoutWall = 0;
        }

    }
}