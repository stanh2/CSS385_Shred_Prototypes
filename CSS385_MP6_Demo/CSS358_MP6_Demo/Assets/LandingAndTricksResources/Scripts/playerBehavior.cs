using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerBehavior : MonoBehaviour {

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
    private float initRotation;
    private GameObject gm;
    private GlobalBehavior gb;
    private raycastUp rayCastLeft, rayCastRight;

    // ground checker variables     
    public LayerMask groundLayer;
    public Transform groundChecker;
    private bool isOnGround = false;
    private Rigidbody2D mRB;
    private float groundCheckerRadius = 0.5f;

    // head checker variables
    public Transform headChecker;
    private float headCheckerRadius = 0.8f;

    #endregion

    // Use this for initialization
    void Start () {
        mRB = GetComponent<Rigidbody2D>();

        gm = GameObject.Find("Game Manager");

        gb = gm.GetComponent<GlobalBehavior>();

        rayCastLeft = GameObject.Find("ray_cast_left").GetComponent<raycastUp>();
        rayCastRight = GameObject.Find("ray_cast_right").GetComponent<raycastUp>();

        speedMultiplier = 1;

        gb.UpdateLandingText("Landing: In Air");

        gb.UpdateSpeedMulText("Speed multiplier: " + speedMultiplier.ToString());

        gb.UpdateTrickText("Trick: ");
    }
	
    void Update()
    {
        if (isOnGround && Input.GetAxis("Jump") > 0)
        {
            initRotation = mRB.rotation;
            isOnGround = false;
            jumped = true;
            mRB.AddForce(new Vector2(0, jumpHeight));
            gm.GetComponent<GlobalBehavior>().UpdateLandingText("Landing: In Air");            
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        // checks if the player is contacting the ground
        isOnGround = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);

        // checks if player's head has hit the ground
        if (Physics2D.OverlapCircle(headChecker.position, headCheckerRadius, groundLayer))
            gb.DestroyMe();

        // prevents character from move faster than the max speed;
        if(mRB.velocity.magnitude >= maxSpeed)
        {
            mRB.velocity = mRB.velocity.normalized * maxSpeed;
        }

        // player can only rotate when in the air
        if (!isOnGround && !isAboveRail)
        {
            mRB.MoveRotation(mRB.rotation - Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime);
            // mRB.AddTorque(Input.GetAxis("Horizontal") * -rotationSpeed);  different rotation method feels different
        }

        if (isAboveRail)
        {
            gb.UpdateScore(2);
            if (Input.GetAxis("Vertical") < 0)
                mRB.AddForce(transform.right * grindSpeed);
        }        

        // apply speed boost for x amount of seconds
        if (boost)
        {
            speedBoostTime -= Time.deltaTime;
            if(speedBoostTime > 0)
            {
                ApplySpeedBoost();                
            }
            else
            {
                boost = false;
                speedMultiplier = 1;
                speedBoostTime = 2f; // reset timer
            }
        }

        // update text
        gb.UpdateSpeedMulText("Speed multiplier: " + speedMultiplier.ToString());

        if (jumped)
        {
            CheckForTricks();                      
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GroundCollider"))
        {
            var angle = Vector2.Angle(this.transform.right, rayCastRight.point - rayCastLeft.point);
            transform.rotation = mRB.velocity.x > 0
                ? Quaternion.Euler(new Vector3(0, 0, -angle))
                : Quaternion.Euler(new Vector3(0, 0, angle));
            mRB.AddForce(transform.right * AddedSpeed * speedMultiplier);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        #region ground trigger
        if (other.gameObject.CompareTag("GroundCollider") && jumped)
        {
            float angle = Vector2.Angle(this.transform.right, rayCastRight.point - rayCastLeft.point);
            Debug.Log(angle);
            if (angle <= 15f)
            {
                int reward = 20;
                jumped = false;
                speedMultiplier = 2f;
                boost = true;
                gb.UpdateLandingText("Landing: Perfect! +" + reward.ToString());
                gb.UpdateScore(reward);
            }                
            else if (angle <= 30f)
            {
                int reward = 15;
                speedMultiplier = 1.8f;
                boost = true;
                jumped = false;
                gb.UpdateLandingText("Landing: Great! +" + reward.ToString());
                gb.UpdateScore(reward);
            }                
            else if (angle <= 45f)
            {
                int reward = 10;
                speedMultiplier = 1.6f;
                boost = true;
                jumped = false;
                gb.UpdateLandingText("Landing: Good! +" + reward.ToString());
                gb.UpdateScore(reward);

            }                
            else if (angle <= 60f)
            {
                int reward = 5;
                speedMultiplier = 1.2f;
                boost = true;
                jumped = false;
                gb.UpdateLandingText("Landing: Alright +" + reward.ToString());
                gb.UpdateScore(reward);
            }                
            else if(angle <= 90f)
            {
                int reward = 2;
                speedMultiplier = 0.5f;
                boost = true;
                jumped = false;
                gb.UpdateLandingText("Landing: Poor +" + reward.ToString());
                gb.UpdateScore(reward);
            }     
            else
            {
                int reward = 0;
                speedMultiplier = 0f;
                boost = true;
                jumped = false;
                gb.UpdateLandingText("Landing: CRASH! +" + reward.ToString());
                gb.DestroyMe();
            }
            transform.rotation = mRB.velocity.x > 0
                ? Quaternion.Euler(new Vector3(0, 0, -angle))
                : Quaternion.Euler(new Vector3(0, 0, angle));
            mRB.AddForce(transform.right * AddedSpeed * speedMultiplier);
        }

        #endregion

        #region rail trigger
        if (other.gameObject.CompareTag("GrindObject"))
        {
            isAboveRail = true;
            if (Input.GetAxis("Vertical") >= 0)
                gb.DestroyMe();
        }
        else
            isAboveRail = false;
        #endregion
    }

    void ApplySpeedBoost()
    {
        // apply speed code here
        mRB.AddForce(transform.right * 5 * speedMultiplier);
    }

    void CheckForTricks()
    {
        float trickRotation = mRB.rotation;
        float deltaAngle = 0;
        deltaAngle += initRotation - trickRotation;
       
        // front flip detection
        if(deltaAngle > 330f)
        {
            int reward = 50;
            gb.UpdateTrickText("Trick: Frontflip");
            if (isOnGround)
            {
                gb.UpdateScore(50);
                gb.UpdateTrickText("Trick: Frontflip +" + reward.ToString());
            }                
        }
        if (deltaAngle < -330f)
        {
            int reward = 50;
            gb.UpdateTrickText("Trick: Backflip");
            if (isOnGround)
            {
                gb.UpdateScore(50);
                gb.UpdateTrickText("Trick: Backflip +" + reward.ToString());
            }
                
        }
        //Debug.Log(deltaAngle);
    }
}
