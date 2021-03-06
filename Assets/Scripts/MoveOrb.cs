﻿using System.Collections;
using System.Security.Policy;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using EnumNamespace;

public class MoveOrb : MonoBehaviour
{

    private int JUMP_VEL;
    public KeyCode jump;
    public KeyCode shoot;
    public float horizVel;
    public float vertVel;
    public float zVel;
    public static float zSpeed;
    public int laneNum;
    public bool controlLocked;
    public Shooting Bullet;
    private Vector3 BASE_GRAVITY;
    private Coroutine slidingCoroutine;
    private Coroutine zCoroutine;
    private float timer;
    public bool canJump;
    public bool canShoot;
    public bool ghostOn;
    public long timerGhost;
    public float slow;

    // Use this for initialization
    private void Start()
    {
        BASE_GRAVITY = new Vector3(0f, -120.0F, 0f);
        JUMP_VEL = 5;
        horizVel = 0;
        vertVel = 0;
        zVel = 0;
        zSpeed = 0;
        laneNum = 2;
        controlLocked = false;
        timer = 0;
        canJump = true;
        GameMaster.PlayerOnePoints = 0;
        GameMaster.PlayerTwoPoints = 0;
        Physics.gravity = BASE_GRAVITY;
        //GetComponent<Renderer>().enabled = false;
    }

    void FixedUpdate()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        timer++;


        HandleStaticPoints();
        handleSideMovementPad();
        HandleShootingTrigger();

        var orb = GetComponent<Rigidbody>();
        orb.velocity = new Vector3(horizVel, vertVel, 10 + zVel + zSpeed + slow);

        if (PauseMenu.GameIsPaused)
        {
            canJump = false;
        }

        if (Input.GetKeyDown(jump) && canJump)
        {
            canJump = false;
            vertVel = JUMP_VEL;
            StartCoroutine(stopJump());
        }
        //if (Input.GetKeyDown(shoot) && canShoot)
        //{
        //    var position = GetComponent<Transform>().position;
        //    var bullet = Instantiate(Bullet, new Vector3(position.x, position.y + 0.5f, position.z), GameMaster.noRotate);
        //    bullet.PlayerObject = GetComponent<Transform>().gameObject;
        //}

        if (GetComponent<Transform>().position.y < 0)
        {
            DestroyAppropriatePlayer();
        }
        if (ghostOn)
        {
            timerGhost++;
            if (timerGhost % 15 == 0)
            {
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = !renderer.enabled;
                }
            }
            
        }
        else
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == Tags.Ground.ToString())
        {
            canJump = true;
        }

        if (other.gameObject.tag == Tags.Wall.ToString())
        {
            DestroyAppropriatePlayer();
        }

        if (other.gameObject.tag == Tags.Perk.ToString())
        {
            var name = other.gameObject.name;
            Destroy(other.gameObject);


            if (name == Perks.TacoIncreaseSpeed.ToString())
                SpeedUp();
            else if (name == Perks.BottlePoint.ToString())
                AddPointToAppropriatePlayer();
            else if (name == Perks.BlockShooting.ToString())
                StopShooting(5f);
            else if (name == Perks.ReverseControls.ToString())
                ReverseMove();
            else if (name == Perks.GhostPerk.ToString())
                GhostOn();
        }

        if (other.gameObject.tag == Tags.Cactus.ToString())
        {
            DestroyAppropriatePlayer();
        }

        if (other.gameObject.tag == Objects.Bullet.ToString())
        {
            SlowOn();
        }
    }

    private void SlowOn()
    {
        slow = -5f;
        StartCoroutine(SlowOff());
    }

    private IEnumerator SlowOff()
    {
        yield return new WaitForSeconds(1f);
        slow = 0f;
    }

    private void GhostOn()
    {
        //canJump = false;
        ghostOn = true;
        vertVel = 0;
        var position = GetComponent<Transform>().position;
        GetComponent<Transform>().position = new Vector3(position.x, 0.3516032f, position.z);
        //GetComponent<Rigidbody>().useGravity = false;
        GetComponent<SphereCollider>().isTrigger = true;
        StartCoroutine(GhostOff());
    }

    private IEnumerator GhostOff()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<SphereCollider>().isTrigger = false;
        //canJump = true;
        ghostOn = false;

    }

    private void SpeedUp()
    {
        zSpeed = 3;
        StartCoroutine(stopSpeedUp());
    }
    private void ReverseMove()
    {

        if (GetComponent<Transform>().gameObject.name == Players.PlayerOne.ToString())
        {
            GameMaster.PlayerTwoControlReversedMultiplier = -1; //true
            StartCoroutine(StopReverseMode());
        }

        if (GetComponent<Transform>().gameObject.name == Players.PlayerTwo.ToString())
        {
            GameMaster.PlayerOneControlReversedMultiplier = -1; //true
            StartCoroutine(StopReverseMode());
        }
    }
    private void StopShooting(float pauseTime)
    {
        canShoot = false;
        StartCoroutine(startShooting(pauseTime));
    }

    private IEnumerator StopReverseMode()
    {

        yield return new WaitForSeconds(3f);
        GameMaster.PlayerTwoControlReversedMultiplier = 1; //true
        GameMaster.PlayerOneControlReversedMultiplier = 1; //true

    }


    private void handleSideMovementPad()
    {
        if (GetComponent<Transform>().gameObject.name == Players.PlayerOne.ToString())
        {
            horizVel = 2 * Mathf.Round(Input.GetAxis(Axis.LeftRightPadOne.ToString())) * GameMaster.PlayerOneControlReversedMultiplier;
            //zVel = 2 * Input.GetAxis(Axis.ForwardBackwardPadOne.ToString());

        }

        if (GetComponent<Transform>().gameObject.name == Players.PlayerTwo.ToString())
        {
            horizVel = 2 * Mathf.Round(Input.GetAxis(Axis.LeftRightPadTwo.ToString())) * GameMaster.PlayerTwoControlReversedMultiplier;
            //zVel = 2 * Input.GetAxis(Axis.ForwardBackwardPadTwo.ToString());
        }
    }

    private void HandleShootingTrigger()
    {
        var transform = GetComponent<Transform>();

        if (canShoot)
        {
            if (transform.gameObject.name == Players.PlayerOne.ToString())
            {
                if (Input.GetAxis(Axis.PrimaryAttackOne.ToString()) < 0)
                {
                    StopShooting(0.25f);
                    var position = transform.position;
                    var bullet = Instantiate(Bullet, new Vector3(position.x, position.y + 0.25f, position.z + 0.5f), GameMaster.noRotate);
                    bullet.PlayerObject = transform.gameObject;
                }

            }

            if (transform.gameObject.name == Players.PlayerTwo.ToString())
            {
                if (Input.GetAxis(Axis.PrimaryAttackTwo.ToString()) < 0)
                {
                    StopShooting(0.25f);
                    var position = transform.position;
                    var bullet = Instantiate(Bullet, new Vector3(position.x, position.y + 0.25f, position.z + 0.5f), GameMaster.noRotate);
                    bullet.PlayerObject = transform.gameObject;
                }
            }
        }
    }

    private IEnumerator stopSpeedUp()
    {
        yield return new WaitForSeconds(3f);
        zSpeed = 0;
    }

    private IEnumerator startShooting(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
        canShoot = true;
    }

    private IEnumerator stopJump()
    {
        yield return new WaitForSeconds(.3f);
        //vertVel = -JUMP_VEL;
        vertVel = 0;
    }

    private void HandleStaticPoints()
    {
        var playerTransform = GetComponent<Transform>();

        if (playerTransform.position.z % 5 < 0.1)
        {
            AddPointToAppropriatePlayer();
        }
    }

    private void AddPointToAppropriatePlayer()
    {
        var playerTransform = GetComponent<Transform>();

        if (playerTransform.gameObject.name == Players.PlayerOne.ToString())
        {
            if (GameMaster.PlayerOneIsAlive == true)
                GameMaster.PlayerOnePoints += 1;
        }
        else
        {
            if (GameMaster.PlayerTwoIsAlive == true)
                GameMaster.PlayerTwoPoints += 1;
        }
    }

    private void DestroyAppropriatePlayer()
    {
        var player = GetComponent<Rigidbody>().gameObject;
        var position = player.GetComponent<Transform>().position;

        if (player.name == Players.PlayerOne.ToString())
        {
            GameMaster.PlayerOneLives--;
            if (GameMaster.PlayerOneLives > 0)
            {
                GhostOn();
                player.GetComponent<Transform>().position = new Vector3(0, 0.35f, position.z);

            }
            else
            {
                GameOver.Game_Over();
            }
        }
        else
        {
            GameMaster.PlayerTwoLives--;
            if (GameMaster.PlayerTwoLives > 0)
            {
                GhostOn();
                player.GetComponent<Transform>().position = new Vector3(0, 0.35f, position.z);
            }
            else
            {
                GameOver.Game_Over();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.Ground.ToString())
        {
            var positionOrb = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(positionOrb.x, other.GetComponent<Transform>().position.y + 0.3516032f, positionOrb.z);
            canJump = true;
        }
    }

}