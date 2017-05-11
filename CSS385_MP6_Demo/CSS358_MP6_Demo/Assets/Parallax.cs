using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    private Transform camT;
    private Transform[] layers;
    private int leftIndex;
    private int rightIndex;
    private float bgSize;
    private float viewZone;
    public Rigidbody2D MheroRB;
    public GameObject Snowboarder;
    public float Speed;
    private float lastCamX,lastCamY;
    public void Start()
    {
        camT = Camera.main.transform;//Camera
        layers = new Transform[transform.childCount];//save the background
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);
        MheroRB = Snowboarder.GetComponent<Rigidbody2D>();
        leftIndex = 0;
        rightIndex = transform.childCount-1;
        bgSize = layers[1].transform.position.x - layers[0].transform.position.x;
        lastCamX = camT.transform.position.x;
        lastCamY = camT.transform.position.y;
    }

    private void shiftlLeft()
    {
        int lastRight = rightIndex;
        layers[rightIndex].position =new Vector3(0,layers[0].transform.position.y,layers[0].transform.position.z)+ Vector3.right * (layers[leftIndex].position.x - bgSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
        
    }
    private void shiftRight()
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = new Vector3(0, layers[0].transform.position.y, layers[0].transform.position.z) + Vector3.right * (layers[rightIndex].position.x + bgSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex=0 ;

    }
    private void FixedUpdate()
    {
        if (camT.transform.position.x < layers[leftIndex].transform.position.x+5)
        {
            shiftlLeft();
        }
        else if (camT.transform.position.x > layers[rightIndex].transform.position.x - 5)
        {
            shiftRight();
        }
       
            transform.position += Vector3.right * ((camT.transform.position.x - lastCamX) * Speed);
            lastCamX = camT.transform.position.x;
        
        
            transform.position += Vector3.up * ((camT.transform.position.y - lastCamY) * Speed);
            lastCamY = camT.transform.position.y;
        
    }
}
