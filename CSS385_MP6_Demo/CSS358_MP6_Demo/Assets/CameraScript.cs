using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    private enum CamState
    {
        Following,
        Stop
    };
    //public int mCycles;
    public float offsetX;
    public GameObject mHero;
    private CamState camerastate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (camerastate == CamState.Following)
        {
            Vector2 camPos = this.transform.position;
            Vector2 heroPos = mHero.transform.position;
            transform.position = new Vector3(heroPos.x + offsetX, heroPos.y, -10);
        }
       

    }
    public void StartCam()
    {
        camerastate = CamState.Following;
    }
    public void StopCam()
    {
        camerastate = CamState.Stop;
    }
}
