using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PhysicsHero : MonoBehaviour
{

    public float MoveForce;
    public Rigidbody2D Physics;
    public float Multiplyer = 2.2f;
//    public Vector3 velocity;
// Use this for initialization
    void Start ()
	{
	    Physics = GetComponent<Rigidbody2D>();
        Physics.AddForce(transform.right * 500);
//        velocity = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update ()
	{
//	    transform.position += velocity;
	}
    private Vector3 _previousCollision;

    void OnCollisionStay2D(Collision2D other)
    {
        var currentPosition = other.gameObject.GetComponent<EdgeCollider2D>().bounds.ClosestPoint(transform.position);

        var displacement = currentPosition - _previousCollision;

        transform.rotation = Physics.velocity.x > 0 ? Quaternion.FromToRotation(Vector3.right, displacement.normalized) :
            Quaternion.FromToRotation(Vector3.right, -displacement.normalized);
        _previousCollision = currentPosition;
//        Debug.Log(Physics.gravityScale);
        Physics.AddForce(transform.right * (Physics.gravityScale * Multiplyer));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        _previousCollision = other.gameObject.GetComponent<EdgeCollider2D>().bounds.ClosestPoint(transform.position);
//        Debug.Log(other.gameObject.GetComponent<EdgeCollider2D>().bounds.ClosestPoint(transform.position));
//          Debug.Log(other.gameObject.GetComponent<EdgeCollider2D>().bounds.ClosestPoint(transform.position));
        //find out which pair of points i am closest to, find the angle of the curve.
    }

//    void 
}
