using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalBehavior : MonoBehaviour
{

    public Transform spawnLocation;
    public GameObject mSnowboarder;
    public GameObject mSnowboarderClone;
    private AudioSource crashSound;
    private int score = 0;
    public GameObject UIGame;
    public GameObject UIDie;
    public Camera camera;
    // UI text variables
    public Text landingText;
    public Text speedMulText;
    public Text trickText;
    public Text scoreText;
    public Image timerImage;
    private CameraScript cs;
    private playerBehavior pb;
    // Use this for initialization
    void Start()
    {
        //mSnowboarder = Instantiate(mSnowboarderClone, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        crashSound = GetComponent<AudioSource>();
        timerImage.fillAmount = 0;
        UIDie.SetActive(false);
        cs = (CameraScript)GetComponentInParent(typeof(CameraScript));
        pb = (playerBehavior)mSnowboarder.GetComponent(typeof(playerBehavior));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            //DestroyObject(mSnowboarder);
            PlayerDie();
            //mSnowboarder = Instantiate(mSnowboarderClone, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
        //Debug.Log(speedMulText.text);
        //Debug.Log(trickText.text);
        
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLandingText(string newText)
    {
        landingText.text = newText;
    }

    public void UpdateSpeedMulText(string newText)
    {
        speedMulText.text = newText;
    }

    public void UpdateTrickText(string newText)
    {
        trickText.text = newText;
    }

    public void UpdateScore(int points)
    {
        score += points;
    }

    public void updateTimer(float initTime, float deltaTime)
    {
        timerImage.fillAmount = deltaTime / initTime;
    }
    public void PlayerDie()
    {
        UIDie.SetActive(true);
        UIGame.SetActive(false);
        cs.StopCam();
    }
    public void RetryLevel()
    {
        UIDie.SetActive(false);
        UIGame.SetActive(true);
        pb.Retry();
        cs.StartCam();
    }
    public void DestroyMe()
    {
        //DestroyObject(mSnowboarder);
        crashSound.Play();
        speedMulText.text = "";
        trickText.text = "Trick: reset";
    }
}
