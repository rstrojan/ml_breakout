using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    MeshRenderer ground;
    private float groundWidth;

    public GameObject[] brickPrefabs;
    private float brickLength;
    private float maxBrickLength;
    private float brickWidth;
    private int brickRows = 4;
    private int brickZPosStart = 5;
    public int destructableBrickCount = 0;

    public GameObject playerPrefab;
    public GameObject ballPrefab;


    // Start is called before the first frame update
    void Awake()
    {
        ground = GameObject.Find("Ground").GetComponent<MeshRenderer>();    // get the ground object
        groundWidth = ground.bounds.size.x;                                 // get the width of the ground
        GetMaxBrickLength();                                                // get the length of the largest brick available
        SetBricks();                                                        // set the bricks in the scene
        Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation); // set player in scene
        Instantiate(ballPrefab, ballPrefab.transform.position, ballPrefab.transform.rotation);  // set ball in scene
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // set rows of bricks for beginning of level
    private void SetBricks(){
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            float xPos = 0f - groundWidth/2f;                           // get center of brick next to left wall as current position
            while(xPos + maxBrickLength < groundWidth/2f){              // while there is still space for the largest brick
                int thisBrick = Random.Range(0, brickPrefabs.Length);   // pick a brick type
                brickLength = brickPrefabs[thisBrick].GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
                Instantiate(brickPrefabs[thisBrick], new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + i), brickPrefabs[thisBrick].transform.rotation);  // create brick at current position
                xPos += brickLength;
                if(brickPrefabs[thisBrick].GetComponent<BrickController>().brick.isDestructable){
                    destructableBrickCount++;
                }                                    // increment current position by length of this brick
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
