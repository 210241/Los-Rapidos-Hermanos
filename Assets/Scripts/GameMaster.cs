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

    public static Transform wallInstance;

    public static Quaternion noRotate;
    List<Transform> PossibleFloors;
    Queue<Transform> ListOfFloors;

    private Transform fogInstance;
    public static Transform orbInstancePlayer1;
    public static Transform orbInstancePlayer2;
    private Camera mainCameraInstance;
    private Transform blobShadowProjectorPlayerOne;
    private Transform blobShadowProjectorPlayerTwo;
    private int floorsWithoutWall;

    public static Vector3 AvgOrbsVelocity;
    public static Vector3 orbVelocity1;
    public static Vector3 orbVelocity2;
    public static float AveragePlayersZDimension;
    public static Stopwatch timer;
    public static int MaxWallHealth;
    public static int CurrentWallHealthPlayerOne;
    public static int CurrentWallHealthPlayerTwo;

    public static bool startSpawningCactie;

    public static bool GenerateFloorsRandomly = true;

    public int LevelCounter = 1;
    public int LevelLenght;
    public static int StaticLevelLenght;

    // Use this for initialization
    void Start()
    {
        orbInstancePlayer1 = Orb_Player1;
        orbInstancePlayer2 = Orb_Player2;
        AveragePlayersZDimension = (orbInstancePlayer1.position.z + orbInstancePlayer2.position.z) / 2.0f;

        Orb_Player1 = Resources.Load<Transform>("Sphere&Hat");
        Orb_Player2 = Resources.Load<Transform>("Sphere&Hat2");

        BlobShadowProjector = Resources.Load<Transform>("Projectors/Prefabs/BlobShadowProjector");

        noRotate = new Quaternion(0, 0, 0, 0);
        timer = new Stopwatch();
        timer.Start();

        PlayerOneControlReversedMultiplier = 1;
        PlayerTwoControlReversedMultiplier = 1;

        PlayerOneIsAlive = true;
        PlayerTwoIsAlive = true;

        MaxWallHealth = 5;
        CurrentWallHealthPlayerOne = 5;
        CurrentWallHealthPlayerTwo = 5;

        blobShadowProjectorPlayerOne = Instantiate(BlobShadowProjector, new Vector3(-1, 0, 0), noRotate);
        blobShadowProjectorPlayerTwo = Instantiate(BlobShadowProjector, new Vector3(1, 0, 0), noRotate);

        Orb_Player1.GetComponentInChildren<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        Orb_Player2.GetComponentInChildren<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;

        Orb_Player1.gameObject.layer = 2;
        Orb_Player2.gameObject.layer = 2;

        blobShadowProjectorPlayerOne.GetComponent<Transform>().Rotate(90,0,0);
        blobShadowProjectorPlayerTwo.GetComponent<Transform>().Rotate(90, 0, 0);

        blobShadowProjectorPlayerOne.GetComponent<Projector>().ignoreLayers = (1 << 2);
        blobShadowProjectorPlayerTwo.GetComponent<Projector>().ignoreLayers = (1 << 2);
        StaticLevelLenght = LevelLenght;

    }

    // Update is called once per frame
    private void Update()
    {
        
        var playerOnePosition = Orb_Player1.position;
        var playerTwoPosition = Orb_Player2.position;

        AveragePlayersZDimension = (orbInstancePlayer1.position.z + orbInstancePlayer2.position.z) / 2.0f;
        blobShadowProjectorPlayerOne.GetComponent<Transform>().position = playerOnePosition;
        blobShadowProjectorPlayerTwo.GetComponent<Transform>().position = playerTwoPosition;

        var playerVelocity1 = Orb_Player1.gameObject.GetComponent<Rigidbody>().velocity;
        var playerVelocity2 = Orb_Player2.gameObject.GetComponent<Rigidbody>().velocity;

        PlayerOneScore.text = PlayerOnePoints.ToString();
        PlayerTwoScore.text = PlayerTwoPoints.ToString();
        StaticLevelLenght = LevelLenght;

    }
}