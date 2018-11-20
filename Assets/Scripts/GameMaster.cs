using System;
using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public Transform Orb_Player1;
    public Transform Orb_Player2;
    public Transform Wall;
    public Transform Fog;
    public Transform BlobShadowProjector;

    public static int PlayerOnePoints;
    public static int PlayerTwoPoints;

    public static bool PlayerOneIsAlive;
    public static bool PlayerTwoIsAlive;
    public static bool IsWallOnTheScreen;

    public static int PlayerOneControlReversedMultiplier;
    public static int PlayerTwoControlReversedMultiplier;

    public static int PlayerOneLives;
    public static int PlayerTwoLives;

    public static Transform wallInstance;

    public static Quaternion noRotate;
    List<Transform> PossibleFloors;
    Queue<Transform> ListOfFloors;

    private Transform fogInstance;
    private Transform orbInstancePlayer1;
    private Transform orbInstancePlayer2;
    private Camera mainCameraInstance;
    private Transform blobShadowProjectorPlayerOne;
    private Transform blobShadowProjectorPlayerTwo;
    private int floorsWithoutWall;

    public static Vector3 AvgOrbsVelocity;
    public static Vector3 orbVelocity1;
    public static Vector3 orbVelocity2;
    public static Stopwatch timer;
    public static int MaxWallHealth;
    public static int CurrentWallHealthPlayerOne;
    public static int CurrentWallHealthPlayerTwo;

    public static bool startSpawningCactie;


    // Use this for initialization
    void Start()
    {
        BasicFloor = Resources.Load<Transform>("Floor/BasicFloorBlock(5x5)");
        CrossHoleFloor = Resources.Load<Transform>("Floor/CrossHoleFloorBlock(5x5)");
        BigCrossHoleFloor = Resources.Load<Transform>("Floor/BigCrossHoleFloorBlock(5x5)");
        SatelliteHoleFloor = Resources.Load<Transform>("Floor/SatelliteHoleFloorBlock(5x5)");

        BaseCactus = Resources.Load<Transform>("BasicCactus");
        Orb_Player1 = Resources.Load<Transform>("Sphere");
        Orb_Player2 = Resources.Load<Transform>("Sphere2");
        Wall = Resources.Load<Transform>("Cube");
        Fog = Resources.Load<Transform>("Fog");

        MainCamera = Resources.Load<Camera>("Main Camera");
        BlobShadowProjector = Resources.Load<Transform>("Projectors/Prefabs/BlobShadowProjector");

        noRotate = new Quaternion(0, 0, 0, 0);
        PossibleFloors = new List<Transform>();
        ListOfFloors = new Queue<Transform>();
        timer = new Stopwatch();
        timer.Start();

        BasicFloor.name = Floor.BasicFloor.ToString();
        CrossHoleFloor.name = Floor.CrossHoleFloor.ToString(); ;
        BigCrossHoleFloor.name = Floor.BigCrossHoleFloor.ToString(); ;
        SatelliteHoleFloor.name = Floor.SatelliteHoleFloor.ToString(); ;

        startSpawningCactie = false;
        IsWallOnTheScreen = false;

        mainCameraInstance = Instantiate(MainCamera, new Vector3(0f, 3.14f, -2.34f), noRotate);
        mainCameraInstance.transform.Rotate(32.15f, 0, 0);

        PlayerOneControlReversedMultiplier = 1;
        PlayerTwoControlReversedMultiplier = 1;

        PlayerOneIsAlive = true;
        PlayerTwoIsAlive = true;

        PlayerOneLives = 3;
        PlayerTwoLives = 3;
        MaxWallHealth = 5;
        CurrentWallHealthPlayerOne = 5;
        CurrentWallHealthPlayerTwo = 5;

        orbInstancePlayer1 = Instantiate(Orb_Player1, new Vector3(-1, 0, 0), noRotate);
        orbInstancePlayer2 = Instantiate(Orb_Player2, new Vector3(1, 0, 0), noRotate);

        blobShadowProjectorPlayerOne = Instantiate(BlobShadowProjector, new Vector3(-1, 0, 0), noRotate);
        blobShadowProjectorPlayerTwo = Instantiate(BlobShadowProjector, new Vector3(1, 0, 0), noRotate);

        orbInstancePlayer1.name = Players.PlayerOne.ToString();
        orbInstancePlayer2.name = Players.PlayerTwo.ToString();
        var floor1 = Instantiate(BasicFloor, new Vector3(0, 0, 0), noRotate);
        var floor2 = Instantiate(BasicFloor, new Vector3(0, 0, 5), noRotate);
        floor1.name= Floor.BasicFloor.ToString();
        floor2.name= Floor.BasicFloor.ToString();
        ListOfFloors.Enqueue(floor1);
        ListOfFloors.Enqueue(floor2);

        //PopulatePossibleFloorsList();

        for (int i = 2; i < 10; i++)
        {
            Assets.Scripts.Tuple<Transform, Floor> floorTransform = GetFloor();
            var floor3 = Instantiate(floorTransform.Item1, new Vector3(0, 0, i * 5), noRotate);
            floor3.name = floorTransform.Item2.ToString();
            ListOfFloors.Enqueue(floor3);
        }

        fogInstance = Instantiate(Fog, new Vector3(0, 0, 40), noRotate);

        orbInstancePlayer1.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        orbInstancePlayer2.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;

        orbInstancePlayer1.gameObject.layer = 2;
        orbInstancePlayer2.gameObject.layer = 2;

        blobShadowProjectorPlayerOne.GetComponent<Transform>().Rotate(90,0,0);
        blobShadowProjectorPlayerTwo.GetComponent<Transform>().Rotate(90, 0, 0);

        blobShadowProjectorPlayerOne.GetComponent<Projector>().ignoreLayers = (1 << 2);
        blobShadowProjectorPlayerTwo.GetComponent<Projector>().ignoreLayers = (1 << 2);

    }

    private Assets.Scripts.Tuple<Transform,Floor> GetFloor()
    {
        int counter = 0;
        int probabilityCounter = 0;
        var childrenMap = StaticMappings.FloorNameToChildrenMap[ListOfFloors.Last().name].Select(t =>
        {
            counter += t.Item2;
            return new Assets.Scripts.Tuple<Floor, int>(t.Item1, t.Item2);
        }).ToList();
        int rand = Random.Range(0, counter);
        Floor chosenFloor = childrenMap.Find(elem =>
        {
            probabilityCounter += elem.Item2;
            return probabilityCounter >= rand;
        }).Item1;

        switch (chosenFloor)
        {
            case Floor.BasicFloor:
                return new Assets.Scripts.Tuple<Transform, Floor>( BasicFloor,Floor.BasicFloor);
            case Floor.CrossHoleFloor:
                return new Assets.Scripts.Tuple<Transform, Floor>(CrossHoleFloor, Floor.CrossHoleFloor);
            case Floor.BigCrossHoleFloor:
                return new Assets.Scripts.Tuple<Transform, Floor>(BigCrossHoleFloor, Floor.BigCrossHoleFloor);
            case Floor.SatelliteHoleFloor:
                return new Assets.Scripts.Tuple<Transform, Floor>(SatelliteHoleFloor, Floor.SatelliteHoleFloor);
            default:
                return new Assets.Scripts.Tuple<Transform, Floor>(BasicFloor, Floor.BasicFloor);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        
        var playerOnePosition = orbInstancePlayer1.position;
        var playerTwoPosition = orbInstancePlayer2.position;

        blobShadowProjectorPlayerOne.GetComponent<Transform>().position = playerOnePosition;
        blobShadowProjectorPlayerTwo.GetComponent<Transform>().position = playerTwoPosition;

        fogInstance.position = new Vector3(0, 2.5f, playerOnePosition.z + 25.0f);

        startSpawningCactie = true;
        //if (orbInstancePlayer2 == null || orbInstancePlayer1 == null)
        //{
        //    GameOver.Game_Over();
        //}

        if (orbInstancePlayer1.position.z > ListOfFloors.Peek().position.z + 20)
        {

            var firstFloor = ListOfFloors.Dequeue();
            Destroy(firstFloor.gameObject);
            var floorTransform = GetFloor();
            var floor = Instantiate(floorTransform.Item1, new Vector3(0, 0, orbInstancePlayer1.position.z + 25),
                noRotate);
            floor.name = floorTransform.Item2.ToString();
            ListOfFloors.Enqueue(floor);

            if (!IsWallOnTheScreen)
                floorsWithoutWall++;

        }
        var averageOrbZ = (orbInstancePlayer1.position.z + orbInstancePlayer2.position.z) / 2;
        mainCameraInstance.transform.position = new Vector3(mainCameraInstance.transform.position.x, mainCameraInstance.transform.position.y, averageOrbZ - 4.5f);

        var playerVelocity1 = orbInstancePlayer1.gameObject.GetComponent<Rigidbody>().velocity;
        var playerVelocity2 = orbInstancePlayer2.gameObject.GetComponent<Rigidbody>().velocity;

        AvgOrbsVelocity = new Vector3(0, 0, (playerVelocity1.z + playerVelocity2.z) / 2);

        PlayerOneScore.text = PlayerOnePoints.ToString();
        PlayerTwoScore.text = PlayerTwoPoints.ToString();

        if (floorsWithoutWall >= 20)
        {
            IsWallOnTheScreen = true;
            wallInstance = Instantiate(Wall, new Vector3(0, 1.8f, orbInstancePlayer1.position.z + 25), noRotate);

            floorsWithoutWall = 0;
        }

    }
}