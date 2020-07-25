using System.Collections;
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
    [SerializeField] Camera mainCamera;
    public GameManager gameManager; 

    private MeshRenderer ground;
    private float groundWidth;

    public GameObject playerPrefab;
    public GameObject ballPrefab;
    public GameObject[] brickPrefabs;
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
        player.GetComponent<PlayerController>().playerId = playerId;
        Debug.Log("gm isTwoPlayer: " + isTwoPlayer);
        player.GetComponent<PlayerController>().isTwoPlayer = isTwoPlayer;
        GameObject ball = Instantiate(ballPrefab, ballPrefab.transform.position + transform.position, ballPrefab.transform.rotation);  // set ball in scene
        ball.GetComponent<BallController>().playerId = playerId;
        ball.GetComponent<BallController>().levelController = this.gameObject;
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
            gameManager.LevelComplete();
        }
    }

    // set rows of bricks for beginning of level
    protected virtual void SetBricks(){
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            float xPos = 0f - groundWidth/2f + Random.Range(0, maxBrickLength);                           // get center of brick next to left wall as current position
            while(xPos + maxBrickLength < groundWidth/2f){              // while there is still space for the largest brick
                GameObject brickChoice = brickPrefabs[Random.Range(0, brickPrefabs.Length)];   // pick a brick type
                brickLength = brickChoice.GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
                GameObject newBrick = Instantiate(brickChoice, new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + i) + transform.position, brickChoice.transform.rotation);  // create brick at current position
                newBrick.GetComponent<BrickController>().playerId = playerId;
                newBrick.GetComponent<BrickController>().levelController = this.gameObject;
                newBrick.GetComponent<BrickController>().gameManager = gameManager;
                newBrick.GetComponent<BrickController>().player = player;
                PowerUpStatus(newBrick);
                if(newBrick.GetComponent<BrickController>().brick.isDestructable){
                    destructableBrickCount++;
                }
                xPos += brickLength;                                    // increment current position by length of this brick
            }
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
        mainCamera.rect = new Rect(0, 0, 0.5f, 1);   // set main Camera to half screen
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
