using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastUp : MonoBehaviour {

    public Vector2 point;
    private playerBehavior parent;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<playerBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 10f, parent.groundLayer);

        Debug.DrawRay(transform.position, -Vector3.up * 10f, Color.green);
        if (hit)
        {
            point = hit.point;
        }            
	}
}
