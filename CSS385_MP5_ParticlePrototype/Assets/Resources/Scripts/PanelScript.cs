using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelScript : MonoBehaviour
{
    public Button StartSS;
    public Button StartPhysic;
    public Button StartJump;
    public Button StartParticle;
    public Button ExitButton;

    // Use this for initialization
    void Start()
    {
        StartSS.onClick.AddListener(Start2D);
        StartPhysic.onClick.AddListener(StartCredit);
        StartJump.onClick.AddListener(StartTricksAndLanding);
        StartParticle.onClick.AddListener(StartSnow);
        ExitButton.onClick.AddListener(EndGame);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Start2D()
    {
        Application.LoadLevel("2DSS");
    }
    void StartCredit()
    {
        Application.LoadLevel("ShredPrototype_Physics");
    }
    void StartTricksAndLanding()
    {
        Application.LoadLevel("ShredPrototypeTricksAndLanding");
    }
    void StartSnow()
    {
        Application.LoadLevel("ShredPrototype_SnowParticles");
    }
    void EndGame()
    {
        Application.Quit();
    }
}

