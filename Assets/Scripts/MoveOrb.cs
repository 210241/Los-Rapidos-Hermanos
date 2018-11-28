using System.Collections;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using EnumNamespace;
using Timer = System.Timers.Timer;

public class MoveOrb : MonoBehaviour
{
    #region Fields

    private float JUMP_VEL;
    public float FALL_SPEED;
    public KeyCode jump;
    public KeyCode shoot;
    public float horizVel;
    public float vertVel;
    public float zVel;
    public float yVel;
    public float xVel;
    public static float zSpeed;
    public int laneNum;
    public bool controlLocked;
    public AudioSource MainSound;
    //public AudioClip
    public Shooting Bullet;
    private Vector3 BASE_GRAVITY;
    private Vector3 LAST_POSITION;
    private Vector3 GREAT_GRAVITY;
    private Coroutine slidingCoroutine;
    private Coroutine zCoroutine;
    private float timer;
    public bool canJump;
    public bool canShoot;
    public bool ghostOn;
    public long timerGhost;
    public float slow;
    private float DeathZone;
    private Vector3 Checkpoint;

    #endregion

    #region Start & Update Methods

    private void Start()
    {
        BASE_GRAVITY = new Vector3(0f, -150.0F, 0f);
        GREAT_GRAVITY = new Vector3(0f, -520f, 0f);
        LAST_POSITION = GetComponent<Transform>().position;
        JUMP_VEL = 7;
        FALL_SPEED = 6;
        horizVel = 0;
        vertVel = 0;
        zVel = 0;
        xVel = 0;
        yVel = 0;
        zSpeed = 0;
        laneNum = 2;
        controlLocked = false;
        timer = 0;
        canJump = true;
        GameMaster.PlayerOnePoints = 0;
        GameMaster.PlayerTwoPoints = 0;
        Physics.gravity = BASE_GRAVITY;
        StartCoroutine(PositionSaver());
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
        orb.velocity = new Vector3(horizVel + xVel, vertVel + yVel, 20 + zVel + zSpeed + slow);

        if (PauseMenu.GameIsPaused)
        {
            canJump = false;
        }

        if (Input.GetKeyDown(jump) && canJump)
        {
            canJump = false;
            vertVel = JUMP_VEL;
            //Physics.gravity = new Vector3(0, 150f, 0);
            StartCoroutine(stopJump(.3f));

        }

        if (GetComponent<Transform>().position.y < DeathZone-1.5f)
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

    #endregion

    #region Perk Methods

    private void SlowOn(float time)
    {
        slow = -5f;
        StartCoroutine(SlowOff(time));
    }

    private void GhostOn(float time)
    {
        ghostOn = true;
        vertVel = 0;
        var position = GetComponent<Transform>().position;
        GetComponent<Transform>().position = LAST_POSITION;
        GetComponent<SphereCollider>().isTrigger = true;
        StartCoroutine(GhostOff(time));
    }

    private void SpeedUp(float speed, float time)
    {
        zSpeed = speed;
        StartCoroutine(stopSpeedUp(time));
    }
    private void ReverseMove(float time)
    {

        if (GetComponent<Transform>().gameObject.name == Players.PlayerOne.ToString())
        {
            GameMaster.PlayerTwoControlReversedMultiplier = -1; //true
            StartCoroutine(StopReverseMode(time));
        }

        if (GetComponent<Transform>().gameObject.name == Players.PlayerTwo.ToString())
        {
            GameMaster.PlayerOneControlReversedMultiplier = -1; //true
            StartCoroutine(StopReverseMode(time));
        }
    }
    private void StopShooting(float pauseTime)
    {
        canShoot = false;
        StartCoroutine(startShooting(pauseTime));
    }

    #endregion

    #region Pad Handler

    private void handleSideMovementPad()
    {
        if (GetComponent<Transform>().gameObject.name == Players.PlayerOne.ToString())
        {
            horizVel = 3 * Mathf.Round(Input.GetAxis(Axis.LeftRightPadOne.ToString())) * GameMaster.PlayerOneControlReversedMultiplier;

        }

        if (GetComponent<Transform>().gameObject.name == Players.PlayerTwo.ToString())
        {
            horizVel = 3 * Mathf.Round(Input.GetAxis(Axis.LeftRightPadTwo.ToString())) * GameMaster.PlayerTwoControlReversedMultiplier;
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

    #endregion

    #region Coroutine

    private IEnumerator StopReverseMode(float time)
    {

        yield return new WaitForSeconds(time);
        GameMaster.PlayerTwoControlReversedMultiplier = 1; //true
        GameMaster.PlayerOneControlReversedMultiplier = 1; //true

    }
    private IEnumerator stopSpeedUp(float time)
    {
        yield return new WaitForSeconds(time);
        zSpeed = 0;
    }

    private IEnumerator startShooting(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
        canShoot = true;
    }

    private IEnumerator stopJump(float time)
    {
        yield return new WaitForSeconds(time);
        //Physics.gravity = GREAT_GRAVITY;
        vertVel = -FALL_SPEED;
    }
    private IEnumerator GhostOff(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<SphereCollider>().isTrigger = false;
        ghostOn = false;

    }
    private IEnumerator SlowOff(float time)
    {
        yield return new WaitForSeconds(time);
        slow = 0f;
    }

    private IEnumerator PositionSaver()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            LAST_POSITION = GetComponent<Transform>().position;
        }
    }

    #endregion

    #region Points Metods

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

    #endregion


    private void DestroyAppropriatePlayer()
    {
        var player = GetComponent<Rigidbody>().gameObject;
        var position = player.GetComponent<Transform>().position;

        if (player.name == Players.PlayerOne.ToString())
        {
            GameMaster.PlayerOneLives--;
            if (GameMaster.PlayerOneLives > 0)
            {
                player.GetComponent<Transform>().position = new Vector3(0, 1.2680794f, position.z);
                //GhostOn(3f);
                GameMaster.orbInstancePlayer1.position = Checkpoint;
                GameMaster.orbInstancePlayer2.position = Checkpoint;

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
                player.GetComponent<Transform>().position = new Vector3(0, 1.2680794f, position.z);
                //GhostOn(3f);
                GameMaster.orbInstancePlayer1.position = Checkpoint;
                GameMaster.orbInstancePlayer2.position = Checkpoint;
            }
            else
            {
                GameOver.Game_Over();
            }
        }
    }

    #region Trigger and Collision Enter

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.Ground.ToString())
        {
            var positionOrb = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(positionOrb.x, other.GetComponent<Transform>().position.y + 0.3516032f, positionOrb.z);
            zVel = 0;
            yVel = 0;
            canJump = true;
        }
        if (other.tag == Tags.Ramp.ToString())
        {
            var positionOrb = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(positionOrb.x, other.GetComponent<Transform>().position.y + 0.3516032f, positionOrb.z);
            zVel = 15;
            yVel = 2;
            canJump = true;
        }
        if (other.tag == Tags.SuperGriavity.ToString())
        {
            var positionOrb = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(positionOrb.x, other.GetComponent<Transform>().position.y + 0.3516032f, positionOrb.z);
            zVel = 15;
            yVel = -20;
            canJump = true;
        }
        if (other.tag == Tags.HalfPipe.ToString())
        {
            var positionOrb = GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(positionOrb.x, other.GetComponent<Transform>().position.y + 0.3516032f, positionOrb.z);
            zVel = 15;
            yVel = -4;
            canJump = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if (tag == Tags.Ground.ToString())
        {
            Physics.gravity = BASE_GRAVITY;
            canJump = true;
            zVel = 0;
            vertVel = 0;
            yVel = 0;
            DeathZone = other.gameObject.transform.position.y;
            if (other.gameObject.name.Split(' ')[0] == "BasicFloorBlock(5x5)")
            {
                Checkpoint = other.gameObject.transform.position;
            }
        }

        if (tag == Tags.Ramp.ToString())
        {
            Physics.gravity = BASE_GRAVITY;
            canJump = true;
            zVel = 15;
            yVel = 2;
        }
        if (tag == Tags.SuperGriavity.ToString())
        {
            //StartCoroutine(GreatGravityWhen(other.transform.position.z));
            canJump = true;
            zVel = 15;
            yVel = -10;
        }

        if (tag == Tags.HalfPipe.ToString())
        {
            canJump = true;
            zVel = 35;
            yVel = 0;
            //Physics.gravity = GREAT_GRAVITY;
        }

        if (tag == Tags.Wall.ToString())
        {
            DestroyAppropriatePlayer();
        }

        if (tag == Tags.Perk.ToString())
        {
            string name = other.gameObject.name;
            Destroy(other.gameObject);


            if (name == Perks.TacoIncreaseSpeed.ToString())
                SpeedUp(3f, 3f);
            else if (name == Perks.BottlePoint.ToString())
                AddPointToAppropriatePlayer();
            else if (name == Perks.BlockShooting.ToString())
                StopShooting(5f);
            else if (name == Perks.ReverseControls.ToString())
                ReverseMove(3f);
            else if (name == Perks.GhostPerk.ToString())
                GhostOn(3f);
        }

        if (tag == Tags.Cactus.ToString())
        {
            DestroyAppropriatePlayer();
        }

        if (tag == Objects.Bullet.ToString())
        {
            SlowOn(1f);
        }

    }

    private IEnumerator GreatGravityWhen(float positionZ)
    {
        yield return new WaitUntil(() => GetComponent<Transform>().position.z > positionZ);
        Physics.gravity = GREAT_GRAVITY;
    }

    #endregion


}