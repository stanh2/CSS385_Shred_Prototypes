using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    //public int mCycles;
    public float offsetX;
    public GameObject mHero;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 camPos = this.transform.position;
        Vector2 heroPos = mHero.transform.position;
        transform.position = new Vector3(heroPos.x+offsetX,0,-10);
       

    }

}
