using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCollider : MonoBehaviour {

    // Use this for initialization
    
    private playerBehavior pb;
	void Start () {
     
        pb = (playerBehavior)GetComponentInParent(typeof(playerBehavior));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GroundCollider"))
        {
            pb.CrashPlayer();
            Debug.Log("Succes");
        }
        
    }
}
