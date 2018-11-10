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
    public int laneNum = 2;
    public bool controlLocked = false;
    public Transform Bullet;
    

    private Vector3 BASE_GRAVITY = new Vector3(0f, -120.0F, 0f);

    private Coroutine slidingCoroutine;
    private Coroutine zCoroutine;
    private float timer = 0;
    
    public bool canJump;

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
        orb.velocity = new Vector3(horizVel, vertVel, 5 + zVel);

        if (Input.GetKeyDown(jump) && canJump)
        {
            canJump = false;
            vertVel = JUMP_VEL;
            StartCoroutine(stopJump());
        }
        if(Input.GetKeyDown(shoot))
        {
            var position = GetComponent<Transform>().position;
            Instantiate(Bullet, new Vector3(position.x, position.y + 0.5f, position.z), GameMaster.noRotate);
            
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
        }

        if (other.gameObject.tag == Tags.Cactus.ToString())
        {
            DestroyAppropriatePlayer();
        }

    }

    private void SpeedUp()
    {
        zVel = 3;
        StartCoroutine(stopSpeedUp());
    }

    private void handleSideMovementPad()
    {
        if (GetComponent<Transform>().gameObject.name == Players.PlayerOne.ToString())
        {
            horizVel = 2 * Input.GetAxis(Axis.LeftRightPadOne.ToString());
            zVel = 2 * Input.GetAxis(Axis.ForwardBackwardPadOne.ToString());
            
        }

        if (GetComponent<Transform>().gameObject.name == Players.PlayerTwo.ToString())
        {
            horizVel = 2 * Input.GetAxis(Axis.LeftRightPadTwo.ToString());
            zVel = 2 * Input.GetAxis(Axis.ForwardBackwardPadTwo.ToString());
        }
    }

    private IEnumerator stopSpeedUp()
    {
        yield return new WaitForSeconds(1f);
        zVel = 0;
    }

    private IEnumerator stopSlide(KeyCode moveButton)
    {
        yield return new WaitWhile(() => Input.GetKey(moveButton));
        horizVel = 0;
        controlLocked = false;
    }

    private IEnumerator stopZMovement(KeyCode moveButton)
    {
        yield return new WaitWhile(() => Input.GetKey(moveButton));
        zVel = 0;
        controlLocked = false;
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