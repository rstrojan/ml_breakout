﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Helper;

public class LevelController : MonoBehaviour
{
    public int playerId;
    public static bool isTwoPlayer;
    [SerializeField] GameObject levelController2P;
    [SerializeField] TextMeshProUGUI scoreText2P;
    [SerializeField] GameObject mainCamera;
    public GameManager gameManager; 

    private MeshRenderer ground;
    private float groundWidth;

    public GameObject playerPrefab;
    public GameObject ballPrefab;
    public GameObject[] brickPrefabs;
    public float[] chanceForBrickType;
    private int[] brickPicks = {0, 0, 0};
    private GameObject player;
    
    private float brickLength;
    private float maxBrickLength;
    private float brickWidth;
    private int brickRows = 4;
    private int brickZPosStart = 5;
    public int destructableBrickCount = 0;
    public float chanceForPowerUp;

    public int ballCount;
    public int powerupCount = 0;            // I don't think this is necessary but good for testing

    private void Awake() {
        isTwoPlayer = GameManager.isTwoPlayer;
        ground = this.transform.GetChild(1).gameObject.FindComponentChildWithTag<MeshRenderer>("Ground");
        groundWidth = ground.bounds.size.x;                                 // get the width of the ground
    }

    void Start(){
        // isTwoPlayer = gameManager.isTwoPlayer;
        SetPlayers();
        player = Instantiate(playerPrefab, playerPrefab.transform.position + transform.position, playerPrefab.transform.rotation); // set player in scene
        player.GetComponent<PlayerController>().playerId = playerId;        // pass player ID to player object
        player.GetComponent<PlayerController>().isTwoPlayer = isTwoPlayer;  // pass 2 player status to player object
        GameObject ball = Instantiate(ballPrefab, ballPrefab.transform.position + transform.position, ballPrefab.transform.rotation);  // set ball in scene
        ball.GetComponent<BallController>().playerId = playerId;            // pass player ID to ball object
        ball.GetComponent<BallController>().levelController = this.gameObject;  // pass correct level controller to ball object
        ballCount++;
        GetMaxBrickLength();                                                // get the length of the largest brick available
        SetBricks();                                                        // set the bricks in the scene
    }

    // Update is called once per frame
    void Update()
    {
        if(ballCount <= 0){
            gameManager.GameOver();     // if all balls destroyed, end game
        }
        if(destructableBrickCount <= 0){
            gameManager.LevelComplete();    // if all destroyable bricks are destroyed, complete level
        }
    }

    // set rows of bricks for beginning of level
    protected virtual void SetBricks(){
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            float xPos = 0f - groundWidth/2f + Random.Range(0, maxBrickLength);                           // get center of brick next to left wall as current position
            while(xPos + maxBrickLength < groundWidth/2f){              // while there is still space for the largest brick
                GameObject brickChoice = ChooseBrick();
                brickLength = brickChoice.GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
                GameObject newBrick = Instantiate(brickChoice, new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + i) + transform.position, brickChoice.transform.rotation);  // create brick at current position
                newBrick.GetComponent<BrickController>().playerId = playerId;   // pass player ID to brick object
                newBrick.GetComponent<BrickController>().levelController = this.gameObject; // pass correct level controller to brick object
                newBrick.GetComponent<BrickController>().gameManager = gameManager; // pass this level's instance of GM to brick object for scoring
                newBrick.GetComponent<BrickController>().player = player;       // pass player object to brick object
                PowerUpStatus(newBrick);                                        // determine if this brick has a power up
                if(newBrick.GetComponent<BrickController>().brick.isDestructable){
                    destructableBrickCount++;                                   // count bricks to be destroyed to complete level
                }
                xPos += brickLength;                                    // increment current position by length of this brick
            }
        }

        for(int i = 0; i < brickPicks.Length; i++)
        {
            Debug.Log("brickPicks[" + i + "]: " + brickPicks[i]);
        }
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

    // randomly choose prick based on percentage of total set in inspector
    protected GameObject ChooseBrick(){
        float randNum = Random.Range(0f, 1f);           // get random number
        float minRange = 0;                             // set minimum range to zero
        for(int i = 0; i < brickPrefabs.Length; i++){   // for each brick prefab option
            // if random number is greater than min range and less than max range for this type
            if((randNum >= minRange) && (randNum <= (minRange + chanceForBrickType[i]))){
                brickPicks[i]++;
                return brickPrefabs[i]; // return chosen brick
            }
            else{
                minRange += chanceForBrickType[i];      // or shift min range to look at next prefab option
            }
        }
        return brickPrefabs[0];                         // default to first prefab
    }

    // set status of powerup
    protected void PowerUpStatus(GameObject newBrick){
        if(Random.Range(0f, 1f) <= chanceForPowerUp){
            newBrick.GetComponent<BrickController>().brick.hasPowerUp = true;
            powerupCount++;
        }
    }

    private void SetPlayers(){
        if(playerId == 1 && isTwoPlayer){
            SetTwoPlayer();
        }
    }

    // set up 2 player environment
    private void SetTwoPlayer(){
        levelController2P.gameObject.SetActive(true);  // set LevelController2P to active
        mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);   // set main Camera to half screen
        mainCamera.transform.position = new Vector3(0, 55, 0);
        scoreText2P.gameObject.SetActive(true);
    }

    // count balls in play for this player
    // private int CountBalls(){
    //     int count = 0;
    //     GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
    //     foreach (var ball in balls){
    //         if(ball.GetComponent<BallController>().playerId == playerId){
    //             count++;
    //         }
    //     }
    //     return count;
    // }
}
