using System.Collections;
using System.Security.Policy;
using UnityEngine;
using EnumNamespace;

public class MoveOrb : MonoBehaviour
{

    private int JUMP_VEL = 5;


    public KeyCode jump;
    public KeyCode shoot;
    public float horizVel = 0;
    public float vertVel = 0;
    public float zVel = 0;
    public float zSpeed = 0;
    public int laneNum = 2;
    public bool controlLocked = false;
    public Shooting Bullet;


    private Vector3 BASE_GRAVITY = new Vector3(0f, -120.0F, 0f);

    private Coroutine slidingCoroutine;
    private Coroutine zCoroutine;
    private float timer = 0;

    public bool canJump;
    public bool canShoot;

    // Use this for initialization
    private void Start()
    {
        GameMaster.PlayerOnePoints = 0;
        GameMaster.PlayerTwoPoints = 0;
        Physics.gravity = BASE_GRAVITY;
    }

    void FixedUpdate()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        HandleStaticPoints();
        handleSideMovementPad();

        var orb = GetComponent<Rigidbody>();
        orb.velocity = new Vector3(horizVel, vertVel, 5 + zVel + zSpeed);

        if (Input.GetKeyDown(jump) && canJump)
        {
            canJump = false;
            vertVel = JUMP_VEL;
            StartCoroutine(stopJump());
        }
        if (Input.GetKeyDown(shoot) && canShoot)
        {
            var position = GetComponent<Transform>().position;
            var bullet = Instantiate(Bullet, new Vector3(position.x, position.y + 0.5f, position.z), GameMaster.noRotate);
            bullet.PlayerObject = GetComponent<Transform>().gameObject;
        }

        if (GetComponent<Transform>().position.y < 0)
        {
            DestroyAppropriatePlayer();
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == Tags.Ground.ToString())
        {
            canJump = true;
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
                StopShooting();
            else if (name == Perks.ReverseControls.ToString())
                ReverseMove();
            else if (name == Perks.GhostPerk.ToString())
                GhostOn();
        }

        if (other.gameObject.tag == Tags.Cactus.ToString())
        {
            DestroyAppropriatePlayer();
        }




    }

    private void GhostOn()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<SphereCollider>().isTrigger = true;
        canJump = false;
        StartCoroutine(GhostOff());
    }

    private IEnumerator GhostOff()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<SphereCollider>().isTrigger = false;
        canJump = true;

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
    private void StopShooting()
    {
        canShoot = false;
        StartCoroutine(startShooting());
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
            horizVel = 2 * Input.GetAxis(Axis.LeftRightPadOne.ToString()) * GameMaster.PlayerOneControlReversedMultiplier;
            zVel = 2 * Input.GetAxis(Axis.ForwardBackwardPadOne.ToString());

        }

        if (GetComponent<Transform>().gameObject.name == Players.PlayerTwo.ToString())
        {
            horizVel = 2 * Input.GetAxis(Axis.LeftRightPadTwo.ToString()) * GameMaster.PlayerTwoControlReversedMultiplier;
            zVel = 2 * Input.GetAxis(Axis.ForwardBackwardPadTwo.ToString());
        }
    }

    private IEnumerator stopSpeedUp()
    {
        yield return new WaitForSeconds(3f);
        zSpeed = 0;
    }

    private IEnumerator startShooting()
    {
        yield return new WaitForSeconds(5f);
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
        Destroy(player);
        if (player.name == Players.PlayerOne.ToString())
            GameMaster.PlayerOneIsAlive = false;
        else
            GameMaster.PlayerTwoIsAlive = false;
    }

}