using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helper;

public class LevelController : MonoBehaviour
{
    private GameManager gameManager;

    MeshRenderer ground;
    private float groundWidth;

    public GameObject playerPrefab;
    public GameObject ballPrefab;
    public GameObject[] brickPrefabs;
    
    private float brickLength;
    private float maxBrickLength;
    private float brickWidth;
    private int brickRows = 4;
    private int brickZPosStart = 5;
    public int destructableBrickCount = 0;
    public float chanceForPowerUp = 0.05f;

    public int previousBallCount;
    public int currentBallCount;

    public int powerupCount = 0;            // I don't think this is necessary but good for testing

    


    // Start is called before the first frame update
    void Awake(){
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Debug.Log("is 2P: " + gameManager.isTwoPlayer);
        if((gameObject.name == "Level Controller") && gameManager.isTwoPlayer){
            Debug.Log("true 2P");
            SetTwoPlayer();
        }
        ground = this.transform.GetChild(1).gameObject.FindComponentChildWithTag<MeshRenderer>("Ground");
        groundWidth = ground.bounds.size.x;                                 // get the width of the ground
        GetMaxBrickLength();                                                // get the length of the largest brick available
        SetBricks();                                                        // set the bricks in the scene
        Instantiate(playerPrefab, playerPrefab.transform.position + transform.position, playerPrefab.transform.rotation); // set player in scene
        Instantiate(ballPrefab, ballPrefab.transform.position + transform.position, ballPrefab.transform.rotation);  // set ball in scene
    }

    // Update is called once per frame
    void Update()
    {
        currentBallCount = 0;
        currentBallCount += GameObject.FindGameObjectsWithTag("Ball").Length; // count balls in play
        if(currentBallCount <= 0){
            gameManager.GameOver();     // if all balls destroyed, end game
        }
        if(destructableBrickCount <= 0){
            gameManager.LevelComplete();
        }
    }

    // set rows of bricks for beginning of level
    protected virtual void SetBricks(){
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            float xPos = 0f - groundWidth/2f;                           // get center of brick next to left wall as current position
            while(xPos + maxBrickLength < groundWidth/2f){              // while there is still space for the largest brick
                GameObject brickChoice = brickPrefabs[Random.Range(0, brickPrefabs.Length)];   // pick a brick type
                brickLength = brickChoice.GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
                GameObject newBrick = Instantiate(brickChoice, new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + i) + transform.position, brickChoice.transform.rotation);  // create brick at current position
                PowerUpStatus(newBrick);
                if(newBrick.GetComponent<BrickController>().brick.isDestructable){
                    destructableBrickCount++;
                }
                xPos += brickLength;                                    // increment current position by length of this brick
            }
        }
        Debug.Log("Destructable Bricks: " + destructableBrickCount);    // 4TESTING
        Debug.Log("Num Bricks with Powerups: " + powerupCount);         // 4TESTING
    }

    // get longest brick for brick setting
    protected void GetMaxBrickLength(){
        maxBrickLength = 0;
        for(int i = 0; i < brickPrefabs.Length; i++){
            float length = brickPrefabs[i].GetComponent<MeshRenderer>().bounds.size.x;
            if(maxBrickLength < length){
                maxBrickLength = length;
            }
        }
    }

    // set status of powerup
    protected void PowerUpStatus(GameObject newBrick){
        if(Random.Range(0f, 1f) <= chanceForPowerUp){
            newBrick.GetComponent<BrickController>().brick.hasPowerUp = true;
            powerupCount++;
        }
    }


    private void SetTwoPlayer(){
        Debug.Log("setting 2 players");
        this.transform.GetChild(2).gameObject.SetActive(true);
        this.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
    }
}
