using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerBehavior : MonoBehaviour
{
    enum State
    {
        Live, Die,Crash
    };
    #region global variables
    // player variables
    public const float maxSpeed = 100f;
    public const float jumpHeight = 1000f;
    public const float rotationSpeed = 200f;
    public const float grindSpeed = 50;
    public float speedBoostTime = 2f; // the duration the speed boost is applied for
    private float speedMultiplier;
    private float AddedSpeed = 90.2f;
    private bool jumped = true;
    private bool boost = false;
    private bool isAboveRail = false;
    private bool trickComplete = false;
    private float initRotation;
    private Vector3 initPos;
    private int tricksInARow = 1;
    private float stateTimer = 0;
    private GameObject gm;
    private GlobalBehavior gb;
    private raycastUp rayCastLeft, rayCastRight;
    private State HeroState;
    // ground checker variables     
    public LayerMask groundLayer;
    public Transform groundChecker;
    private bool isOnGround = false;
    private Rigidbody2D mRB;
    private float groundCheckerRadius = 1f;

    // head checker variables
    public Transform headChecker;
    private float headCheckerRadius = 0.8f;

    #endregion

    // Use this for initialization
    void Start()
    {
        initPos = transform.position;
        mRB = GetComponent<Rigidbody2D>();
        HeroState = State.Live;
        gm = GameObject.Find("Game Manager");

        gb = gm.GetComponent<GlobalBehavior>();

        rayCastLeft = GameObject.Find("ray_cast_left").GetComponent<raycastUp>();
        rayCastRight = GameObject.Find("ray_cast_right").GetComponent<raycastUp>();

        speedMultiplier = 1;

        gb.UpdateLandingText("Landing: In Air");

        gb.UpdateTrickText("Trick: ");
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (HeroState == State.Live)
        {
            // checks if the player is contacting the ground
            isOnGround = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);
            if (isOnGround && Input.GetAxis("Jump") > 0)
            {
                initRotation = mRB.rotation;
                isOnGround = false;
                jumped = true;
                mRB.AddForce(new Vector2(0, jumpHeight));
                gm.GetComponent<GlobalBehavior>().UpdateLandingText("Landing: In Air");
            }
            // checks if player's head has hit the ground
            if (Physics2D.OverlapCircle(headChecker.position, headCheckerRadius, groundLayer))
                //LocalDestroy();

            // prevents character from move faster than the max speed;
            if (mRB.velocity.magnitude >= maxSpeed)
            {
                mRB.velocity = mRB.velocity.normalized * maxSpeed;
            }

            // player can only rotate when in the air
            if (!isOnGround && !isAboveRail)
            {
                mRB.MoveRotation(mRB.rotation - Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime);
                // mRB.AddTorque(Input.GetAxis("Horizontal") * -rotationSpeed);  different rotation method feels different
            }

            /*
            if (isAboveRail)
            {
                gb.UpdateScore(2);
                if (Input.GetAxis("Vertical") < 0)
                    mRB.AddForce(transform.right * grindSpeed);
            }
            */

            // apply speed boost for x amount of seconds
            if (boost && trickComplete)
            {
                speedBoostTime -= Time.deltaTime;
                if (speedBoostTime > 0f)
                {
                    mRB.AddForce(Vector3.Normalize(transform.right) * speedMultiplier);
                    gb.updateTimer(2f, speedBoostTime);
                }
                else
                {
                    boost = false;
                    trickComplete = false;
                    speedMultiplier = 1f;
                    speedBoostTime = 2f; // reset timer
                }
            }

            // update text
            if (speedMultiplier == 1f)
                gb.UpdateSpeedMulText("");
            else
                gb.UpdateSpeedMulText("Boost X " + speedMultiplier);

            if (jumped)
            {
                CheckForTricks();
            }
        }
        if (HeroState == State.Crash)//Player Crash
        {
            gb.UpdateLandingText("Landing: CRASH!");
            stateTimer += 1 * Time.smoothDeltaTime;

            if(stateTimer>1.5)
            {
                HeroState = State.Die;
            }
        }

        if (HeroState == State.Die)//Player Die
        {
            gb.PlayerDie();
            stateTimer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        #region ground trigger
       
            if (other.gameObject.CompareTag("GroundCollider") && jumped)
            {
                float angle = Vector2.Angle(this.transform.right, rayCastRight.point - rayCastLeft.point);

                if (angle <= 15f)
                {
                    int reward = 20;

                    jumped = false;

                    if (trickComplete)
                        BoostPlayer(2f);

                    gb.UpdateLandingText("Landing: Perfect! +" + reward);
                    gb.UpdateScore(reward);
                }
                else if (angle <= 30f)
                {
                    int reward = 15;

                    if (trickComplete)
                        BoostPlayer(1.8f);

                    jumped = false;

                    gb.UpdateLandingText("Landing: Great! +" + reward);
                    gb.UpdateScore(reward);
                }
                else if (angle <= 45f)
                {
                    int reward = 10;

                    if (trickComplete)
                        BoostPlayer(1.6f);

                    jumped = false;

                    gb.UpdateLandingText("Landing: Good! +" + reward);
                    gb.UpdateScore(reward);

                }
                else if (angle <= 60f)
                {
                    int reward = 5;

                    if (trickComplete)
                        BoostPlayer(1.2f);

                    jumped = false;

                    gb.UpdateLandingText("Landing: Alright +" + reward);
                    gb.UpdateScore(reward);
                }
                else if (angle <= 90f)
                {
                    int reward = 2;

                    if (trickComplete)
                        BoostPlayer(0.5f);

                    jumped = false;

                    gb.UpdateLandingText("Landing: Poor +" + reward);
                    gb.UpdateScore(reward);
                }
                else
                {
                    int reward = 0;
                    gb.UpdateLandingText("Landing: CRASH! +" + reward);
                    LocalDestroy();
                    HeroState = State.Crash;
                }

                Vector3 targetVector = (rayCastRight.point - rayCastLeft.point).normalized;
                Quaternion toRotation = Quaternion.LookRotation(targetVector, Vector3.up);
                float maxAngleDelta = 15f;

                // rotates the player to be parallel to the ground
                transform.rotation = mRB.velocity.x > 0 ? Quaternion.RotateTowards(transform.rotation, toRotation, maxAngleDelta)
                    : Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-targetVector, Vector3.up), maxAngleDelta);

                mRB.AddForce(Vector3.Normalize(transform.right) * AddedSpeed);
            }
            
        
        #endregion

        #region rail trigger
        /*
        if (other.gameObject.CompareTag("GrindObject"))
        {
            isAboveRail = true;
            if (Input.GetAxis("Vertical") >= 0)
                LocalDestroy();
        }
        else
            isAboveRail = false;
            */
        #endregion
    }

    void BoostPlayer(float boostMul)
    {
        speedMultiplier = boostMul;
        boost = true;
    }
    public string getState()
    {
        return HeroState.ToString();
    }
    public void CrashPlayer()
    {
        HeroState = State.Crash;
       
    }
    void CheckForTricks()
    {
        int reward = 50;
        float trickRotation = mRB.rotation;
        float deltaAngle = 0;
        bool oneFlipCompleted = false;

        deltaAngle += initRotation - trickRotation;

        if (oneFlipCompleted)
        {
            tricksInARow++;
            oneFlipCompleted = false;
        }

        // front flip detection
        if (deltaAngle > 330f)
        {
            oneFlipCompleted = true;
            gb.UpdateTrickText("Trick: Frontflip");
            if (isOnGround)
            {
                trickComplete = true;
                gb.UpdateScore(reward);
                gb.UpdateTrickText("Trick: Backflip +" + reward);
                tricksInARow = 1;
            }
        }

        // back flip detection
        if (deltaAngle < -330f)
        {
            oneFlipCompleted = true;
            gb.UpdateTrickText("Trick: Backflip");
            if (isOnGround)
            {
                trickComplete = true;
                gb.UpdateScore(reward);
                gb.UpdateTrickText("Trick: Backflip +" + reward);
                tricksInARow = 1;
            }
        }

        if (isOnGround && tricksInARow > 1)
        {
            trickComplete = true;
            gb.UpdateScore(reward * tricksInARow);
            gb.UpdateTrickText("Trick: Combo +" + reward + " X " + tricksInARow);
            tricksInARow = 1;
        }
    }
    public void Retry()
    {
        transform.position = initPos;
        mRB.velocity = new Vector2(0, 0);
        HeroState = State.Live;
        mRB.rotation = 0f;
    }
    void LocalDestroy()
    {
        speedMultiplier = 0f;
        boost = false;
        jumped = false;
        tricksInARow = 1;
        //gb.DestroyMe();
    }
}
