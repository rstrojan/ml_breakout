using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    MeshRenderer ground;
    private float groundWidth;

    public GameObject[] brickPrefabs;
    // public GameObject brickPrefabs2;
    // public GameObject brickPrefabs3;
    private float brickLength;
    private float maxBrickLength;
    private float brickWidth;
    private int brickRows = 4;
    private int brickZPosStart = 5;


    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<MeshRenderer>();
        groundWidth = ground.bounds.size.x;
        brickWidth = brickPrefabs[0].GetComponent<MeshRenderer>().bounds.size.z;
        GetMaxBrickLength();
        SetBricks();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // set rows of bricks for beginning of level
    private void SetBricks(){
        for(int i = 0; i < brickRows; i++){
            float xPos = 0f - groundWidth/2f;
            Debug.Log("starting xPos: " + xPos);
            while(xPos + maxBrickLength < groundWidth/2f){
                int thisBrick = Random.Range(0, brickPrefabs.Length);
                brickLength = brickPrefabs[thisBrick].GetComponent<MeshRenderer>().bounds.size.x;
                Instantiate(brickPrefabs[thisBrick], new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + i), brickPrefabs[thisBrick].transform.rotation);
                xPos += brickLength;
                Debug.Log("brickLength: " + brickLength + "\nxPos: " + xPos);
            }
        }
    }

    // get longest brick for brick setting
    private void GetMaxBrickLength(){
        maxBrickLength = 0;
        for(int i = 0; i < brickPrefabs.Length; i++){
            float length = brickPrefabs[i].GetComponent<MeshRenderer>().bounds.size.x;
            if(maxBrickLength < length){
                maxBrickLength = length;
            }
        }
    }
}
