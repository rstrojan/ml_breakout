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
    public float chanceForPowerUp = 0.05f;
    public int powerupCount = 0;

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
                GameObject brickChoice = brickPrefabs[Random.Range(0, brickPrefabs.Length)];   // pick a brick type
                brickLength = brickChoice.GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
                GameObject newBrick = Instantiate(brickChoice, new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + i), brickChoice.transform.rotation);  // create brick at current position
                PowerUpStatus(newBrick);
                if(newBrick.GetComponent<BrickController>().brick.isDestructable){
                    destructableBrickCount++;
                }
                xPos += brickLength;                                    // increment current position by length of this brick
            }
        }
        Debug.Log("Destructable Bricks: " + destructableBrickCount);
        Debug.Log("Num Bricks with Powerups: " + powerupCount);
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

    // set status of powerup
    private void PowerUpStatus(GameObject newBrick){
        if(Random.Range(0f, 1f) <= chanceForPowerUp){
            newBrick.GetComponent<BrickController>().brick.hasPowerUp = true;
            powerupCount++;
        }
    }
}
