using System;
using EnumNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameMaster : MonoBehaviour
{
    public Camera MainCamera;

    public TextMeshProUGUI PlayerOneScore;
    public TextMeshProUGUI PlayerTwoScore;

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
    public Transform RampFloor;
    public Transform PipeLineFloor;
    

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

    public static bool GenerateFloorsRandomly = true;

    public int LevelCounter;

    private List<FloorData> Level1Map;


    private FloorData LastFloorData;

    // Use this for initialization
    void Start()
    {
        BasicFloor = Resources.Load<Transform>("Floor/BasicFloorBlock(5x5)");
        CrossHoleFloor = Resources.Load<Transform>("Floor/CrossHoleFloorBlock(5x5)");
        BigCrossHoleFloor = Resources.Load<Transform>("Floor/BigCrossHoleFloorBlock(5x5)");
        SatelliteHoleFloor = Resources.Load<Transform>("Floor/SatelliteHoleFloorBlock(5x5)");
        RampFloor = Resources.Load<Transform>("Floor/Ramp");
        PipeLineFloor = Resources.Load<Transform>("Floor/Arch");

        BaseCactus = Resources.Load<Transform>("BasicCactus");
        Orb_Player1 = Resources.Load<Transform>("Sphere&Hat");
        Orb_Player2 = Resources.Load<Transform>("Sphere&Hat2");
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

        InitializeLevel1();

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
        LevelCounter = 1;
        //PopulatePossibleFloorsList();
        FloorData floorTransform = new FloorData(BasicFloor, 69, Floor.DoubleHorizontalSlidingFloor, new Rotation(0,0,0));
        for (int i = 2; i < 10; i++)
        {
            floorTransform = GetFloor();
            var floor3 = Instantiate(floorTransform.FloorTransform, new Vector3(0, 0, i * 5), noRotate);
            floor3.name = floorTransform.FloorType.ToString();
            ListOfFloors.Enqueue(floor3);
        }

        LastFloorData = floorTransform;

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

    private void InitializeLevel1()
    {
        var hole = 1;

        var tempRamp = Instantiate(RampFloor, new Vector3(-100, -100, -100), noRotate);

        var rampLen = tempRamp.GetComponent<Renderer>().bounds.size.y * 2;

        Destroy(tempRamp.gameObject);

        var tempPipe = Instantiate(PipeLineFloor, new Vector3(-100, -100, -100), noRotate);

        var pipeLen = tempRamp.GetComponent<Renderer>().bounds.size.y * 5/3;

        Destroy(tempPipe.gameObject);

        Rotation myNoRotate = new Rotation(0,0,0); 

        Level1Map = new List<FloorData>()
     {
        new FloorData(BasicFloor, 10, Floor.BasicFloor, myNoRotate),
        new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
         new FloorData(RampFloor, rampLen, Floor.BasicFloor, myNoRotate, 3f),
        new FloorData(RampFloor, 0, Floor.BasicFloor, myNoRotate, 0f),
        new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,0,190), new Vector3(-2.5f, -1f), 0.1f),
        new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,5,190), new Vector3(-1.5f, 0), 0.1f),
        new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,10,190), new Vector3(-0.5f, 0), 0.1f),
         new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,15,190), new Vector3(1.5f, 0), 0.1f),
         new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,20,190), new Vector3(2.5f, 0), 0.1f),
         new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,25,190), new Vector3(3.5f, 0), 0.1f),
         new FloorData(PipeLineFloor, pipeLen + hole, Floor.CrossHoleFloor, new Rotation(0,30,190), new Vector3(4.5f, 0), 0.1f),
        new FloorData(CrossHoleFloor, 6, Floor.CrossHoleFloor, myNoRotate),
        new FloorData(CrossHoleFloor, 6, Floor.CrossHoleFloor, myNoRotate),
        new FloorData(CrossHoleFloor, 6, Floor.CrossHoleFloor, myNoRotate),
        new FloorData(CrossHoleFloor, 6, Floor.CrossHoleFloor, myNoRotate),
        new FloorData(CrossHoleFloor, 6, Floor.CrossHoleFloor, myNoRotate),
        new FloorData(CrossHoleFloor, 6, Floor.CrossHoleFloor, myNoRotate),
        new FloorData(BasicFloor, 5, Floor.BasicFloor, myNoRotate),
        new FloorData(BasicFloor, 5, Floor.BasicFloor, myNoRotate),
    };
    }

    private FloorData GetFloor()
    {
        if(!GenerateFloorsRandomly)
        {
            
            if(StaticFloorQueue.Count != 0)
            {   FloorData next = StaticFloorQueue.Dequeue();
                return next;
            }
        }
        // If there are no floors left in the static queue, we get random floor
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
                return new FloorData(BasicFloor, 5, Floor.BasicFloor, new Rotation(0,0,0));
            case Floor.CrossHoleFloor:
                return new FloorData(CrossHoleFloor, 5, Floor.CrossHoleFloor, new Rotation(0, 0, 0));
            case Floor.BigCrossHoleFloor:
                return new FloorData(BigCrossHoleFloor, 5, Floor.BigCrossHoleFloor, new Rotation(0, 0, 0));
            case Floor.SatelliteHoleFloor:
                return new FloorData(SatelliteHoleFloor, 5, Floor.SatelliteHoleFloor, new Rotation(0, 0, 0));
            default:
                return new FloorData(BasicFloor, 5, Floor.BasicFloor, new Rotation(0, 0, 0));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        orbInstancePlayer1.LookAt(orbInstancePlayer1);

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
        if(NeedToPopulateFloorQueue)
            PopulateStaticFloorQueue();

        RandomlyGenerateFloors();
        
        var averageOrbZ = (orbInstancePlayer1.position.z + orbInstancePlayer2.position.z) / 2;
        var averageOrbY = (orbInstancePlayer1.position.y + orbInstancePlayer2.position.y) / 2;
        mainCameraInstance.transform.position = new Vector3(mainCameraInstance.transform.position.x, averageOrbY + 3.14f, averageOrbZ - 4.5f);
        var playerVelocity1 = orbInstancePlayer1.gameObject.GetComponent<Rigidbody>().velocity;
        var playerVelocity2 = orbInstancePlayer2.gameObject.GetComponent<Rigidbody>().velocity;

        AvgOrbsVelocity = new Vector3(0, 0, (playerVelocity1.z + playerVelocity2.z) / 2);

        PlayerOneScore.text = PlayerOnePoints.ToString();
        PlayerTwoScore.text = PlayerTwoPoints.ToString();

        if (floorsWithoutWall >= 1)
        {
            IsWallOnTheScreen = true;
            wallInstance = Instantiate(Wall, new Vector3(0, 1.8f, orbInstancePlayer1.position.z + 25), noRotate);
            Destroy(wallInstance.gameObject);
            IsWallOnTheScreen = false;
            NeedToPopulateFloorQueue = true;

            floorsWithoutWall = 0;
        }

    }

    private void RandomlyGenerateFloors()
    {
        var lastFloorPosition = ListOfFloors.Last().position;

        if (orbInstancePlayer1.position.z > ListOfFloors.Peek().position.z + 20)
        {
            var firstFloor = ListOfFloors.Dequeue();
            Destroy(firstFloor.gameObject);

            var floorData = GetFloor();
            var floor = Instantiate(
                floorData.FloorTransform,
                new Vector3(
                    floorData.PositionOffset.x,
                    lastFloorPosition.y + LastFloorData.Height - 0.1f + floorData.PositionOffset.y,
                    lastFloorPosition.z + LastFloorData.Length + floorData.PositionOffset.z
                ),
                noRotate
            );
            floor.name = floorData.FloorType.ToString();
            floor.GetComponent<Transform>().Rotate(new Vector3(floorData.Rotation.XAngle, floorData.Rotation.YAngle, floorData.Rotation.ZAngle));
            ListOfFloors.Enqueue(floor);
            LastFloorData = floorData;

            if (!IsWallOnTheScreen && GenerateFloorsRandomly)
                floorsWithoutWall++;
        }
    }

    private Queue<FloorData> StaticFloorQueue;

    public void PopulateStaticFloorQueue()
    {
        NeedToPopulateFloorQueue = false;
        switch (LevelCounter)
        {
            case 1:
                StaticFloorQueue = new Queue<FloorData>(Level1Map);
                break;

        }
        LevelCounter++;
        GenerateFloorsRandomly = false;
    }

    public static bool NeedToPopulateFloorQueue = false;

   
}