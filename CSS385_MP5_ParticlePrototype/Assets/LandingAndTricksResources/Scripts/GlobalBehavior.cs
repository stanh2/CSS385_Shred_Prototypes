using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalBehavior : MonoBehaviour
{

    public Transform spawnLocation;
    private GameObject mSnowboarder;
    public GameObject mSnowboarderClone;
    private AudioSource crashSound;
    private int score = 0;

    // UI text variables
    public Text landingText;
    public Text speedMulText;
    public Text trickText;
    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        mSnowboarder = Instantiate(mSnowboarderClone, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        crashSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            DestroyObject(mSnowboarder);
            mSnowboarder = Instantiate(mSnowboarderClone, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
        //Debug.Log(speedMulText.text);
        //Debug.Log(trickText.text);
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLandingText(string text)
    {
        landingText.text = text;
    }

    public void UpdateSpeedMulText(string text)
    {
        speedMulText.text = text;        
    }

    public void UpdateTrickText(string text)
    {
        trickText.text = text;
    }

    public void UpdateScore(int points)
    {
        score += points;
    }

    public void DestryMe()
    {
        DestroyObject(mSnowboarder);
        crashSound.Play();
        speedMulText.text = "Speed multiplier: ";
        trickText.text = "Trick: ";
    }
}
