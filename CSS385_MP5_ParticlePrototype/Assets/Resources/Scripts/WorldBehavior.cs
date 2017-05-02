using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehavior : MonoBehaviour {
    GameObject hero = null;
    public Transform snowBurst;

    // Use this for initialization
    void Start() {
        //hero = (GameObject)Instantiate(Resources.Load("Prefabs/Penguin"));
        snowBurst.GetComponent<ParticleSystem>().enableEmission = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        snowBurst.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(timeSnowBurst());
    }

    
    IEnumerator timeSnowBurst()
    {
        yield return new WaitForSeconds(0.8f);
        snowBurst.GetComponent<ParticleSystem>().enableEmission = false;
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}
