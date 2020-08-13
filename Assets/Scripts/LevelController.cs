using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Helper;

public class LevelController : MonoBehaviour
{
    [Header("For Agents and Training")]
    public bool isAgent;
    public bool isTraining;
    public int numSimulations;
    public GameObject agentPrefab;
    public GameObject levelControllerPrefab;

    [Header("Specific to Controller")]
    public int playerId;
    public static bool isTwoPlayer;
    [SerializeField] GameObject levelController2P;
    [SerializeField] TextMeshProUGUI scoreText2P;
    [SerializeField] GameObject mainCamera;
    public GameManager gameManager; 
    public GameObject ground;
    private float groundWidth;

    [Header("Default Scene Values")]
    public GameObject playerPrefab;
    public GameObject player;
    private Vector3 startPosition;
    public GameObject ballPrefab;
    private GameObject ball;
    public GameObject[] brickPrefabs;
    public float[] chanceForBrickType;
    public float chanceForPowerUp;
    
    private float maxBrickLength;
    private float brickWidth;
    private int brickZMin = 5;
    private int brickZMax = 10;
    public Material[] brickTextures;

    [Header("Values set during play")]
    public int destructableBrickCount = 0;
    public int ballCount = 0;
    [SerializeField] int powerupCount = 0;            // I don't think this is necessary but good for testing
    [SerializeField] int[] brickPicks = {0, 0, 0, 0};   // for testing ratio of chosen bricks

    [Header("Scene Randomization Variables")]
    public int levelProgression = 1;
    public static int brickZPosStart = 10;  // make static
    public static int brickColumnCount = 1;  // make static
    public static int maxBrickTypes;  // make static
    public static int brickRows = 4;  // make static
    public static int rowSkip = 0;  // make static
    public static int rowSkipMax = 2;
    public static bool doAddRandomBricks = false;
    public static int groundMatChoice;
    public static int wallMatChoice;
    [SerializeField] Material[] groundMaterials;
    [SerializeField] Material[] wallMaterials;

    // objects for bg music
    public AudioClip[] music;
    public AudioSource audioSource;

    private void Awake() {
        if(!isTraining){
            //get the audiosource from the camera
            audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
            levelProgression = GameManager.levelTracker;
            isTwoPlayer = GameManager.isTwoPlayer;
            isAgent = GameManager.playerOneIsAI;
            levelController2P.GetComponent<LevelController>().isAgent = GameManager.playerTwoIsAI;
        }
        groundWidth = ground.GetComponent<MeshRenderer>().bounds.size.x;      // get the width of the ground
    }

    void Start(){
        RunSceneGenerator();    // generate level variation variables based on progression
        SetPlayers();           // set one or two player
        SetEnvironment();        // set the environment variables
        InitPlayer();           // initialize player
        if(!isTraining){
            InitBall();             // initialize primary ball
            SetBricks();            // set the bricks in the scene
        }
        else{
            MakeSceneCopies();  // copy scene for training
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ballCount <= 0){
            if(isTraining){
                player.GetComponent<AgentController>().LostAllBalls();
            }
            else{
                if(!isTwoPlayer){
                    gameManager.GameOver();     // if all balls destroyed, end game
                }
                else{
                    ResetPlayer();
                }
            }
        }
        if(destructableBrickCount <= 0){
            if(isTraining){
                player.GetComponent<AgentController>().DestroyedAllBricks();
            }
            else{                
                gameManager.LevelComplete();    // if all destroyable bricks are destroyed, complete level
            }
        }
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(music[Random.Range(0, music.Length)]);
        }
    }

    private void ResetPlayer(){
        gameManager.UpdateScore(-50, playerId);
        InitBall();
        StartCoroutine(WaitForNewBall());
    }

    IEnumerator WaitForNewBall(){
        yield return new WaitForSeconds(3.0f);
        StartPlay();
    }

    private void InitPlayer(){
        Debug.Log("ID: " + playerId);
        if(isAgent){
            player = Instantiate(agentPrefab, agentPrefab.transform.position + transform.position, agentPrefab.transform.rotation, this.transform);
        }
        else{
            player = Instantiate(playerPrefab, playerPrefab.transform.position + transform.position, playerPrefab.transform.rotation, this.transform); // set player in scene
        }       
        startPosition = player.transform.position; // save for agent training resets
        player.SetActive(true);
    }

    private void InitBall(){
        ball = Instantiate(ballPrefab, ballPrefab.transform.position + transform.position, ballPrefab.transform.rotation, this.transform);  // set ball in scene
        ballCount = 1;
        if(isTraining){
            ball.SetActive(true);
        }
    }

    public void StartPlay(){
        ball.SetActive(true);
    }

    private void SetPlayers(){
        if(playerId == 1 && isTwoPlayer){
            SetTwoPlayer();
        }
    }

    // set up 2 player environment
    private void SetTwoPlayer(){
        mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);   // set main Camera to half screen
        levelController2P.gameObject.SetActive(true);  // set LevelController2P to active
        scoreText2P.gameObject.SetActive(true);
    }

    // set ground and wall materials for current scene
    private void SetEnvironment(){
        if(playerId != 1){
            return;
        }
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls){
            wall.GetComponent<Renderer>().material = wallMaterials[wallMatChoice];
        }
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        foreach(var ground in grounds){
            ground.GetComponent<Renderer>().material = groundMaterials[groundMatChoice];
        }
    }
    
    // set rows of bricks for beginning of level
    protected virtual void SetBricks(){
        GetMaxBrickLength();            // get the length of the largest brick available
        switch (brickColumnCount){
            case 2:
                TwoColumnBrickLayout();
                break;
            case 3:
                ThreeColumnBrickLayout();
                break;
            default:
                OneColumnBrickLayout();
                break;
        }        
        if(doAddRandomBricks){
            AddRandomBricks();
        }
    }

    // get longest brick for brick setting
    protected void GetMaxBrickLength(){
        maxBrickLength = 0;
        for(int i = 0; i < maxBrickTypes; i++){
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
        for(int i = 0; i < maxBrickTypes; i++){   // for each brick prefab option
            // if random number is greater than min range and less than max range for this type
            if((randNum >= minRange) && (randNum <= (minRange + chanceForBrickType[i]))){
                brickPicks[i]++;
                return brickPrefabs[i]; // return chosen brick
            }
            else{
                minRange += chanceForBrickType[i];      // or shift min range to look at next prefab option
            }
        }
        brickPicks[0]++;
        return brickPrefabs[0];                         // default to first prefab
    }

    // set status of powerup
    protected void PowerUpStatus(GameObject brick){
        if(Random.Range(0f, 1f) <= chanceForPowerUp){
            brick.GetComponent<BrickController>().brick.hasPowerUp = true;
            powerupCount++;
        }
    }

    // Brick layouts block
    private void InitBrick(GameObject brick){
        BrickController bc = brick.GetComponent<BrickController>();
        PowerUpStatus(brick);         // determine if this brick has a power up
        if(bc.brick.isDestructable){
            destructableBrickCount++;   // count bricks to be destroyed to complete level
        }
        brick.SetActive(true);
    }

    private void MakeRow(float xPos, float xMax, int iter){
        while(xPos + maxBrickLength < xMax){              // while there is still space for the largest brick
            GameObject brickChoice = ChooseBrick();
            float brickLength = brickChoice.GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
            GameObject newBrick = Instantiate(brickChoice, new Vector3(xPos + (brickLength/2f), 2, brickZPosStart + iter + rowSkip) + transform.position, brickChoice.transform.rotation, this.transform);  // create brick at current position
            InitBrick(newBrick);
            newBrick.GetComponent<Renderer>().material = brickTextures[Random.Range(0, 5)];

            xPos += brickLength;                                    // increment current position by length of this brick
        }
    }

    private void OneColumnBrickLayout(){
        float xPos;
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            xPos = 0f - groundWidth/2f + Random.Range(0, maxBrickLength); // get center of brick next to left wall as current position
            MakeRow(xPos, groundWidth/2f, i);
            if(rowSkip != 0){
                rowSkip += Random.Range(0, rowSkipMax);
            }
        }
        
    }

    private void TwoColumnBrickLayout(){
        float xPos;
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            // first column
            xPos = 0f - groundWidth/2f + Random.Range(0, maxBrickLength); // get center of brick next to left wall as current position
            MakeRow(xPos, 0, i);
            // second column
            xPos = 0f + Random.Range(0, maxBrickLength); // get center of brick next to left wall as current position
            MakeRow(xPos, groundWidth/2f, i);
            if(rowSkip != 0){
                rowSkip += Random.Range(0, rowSkipMax);
            }
        }
    }

    private void ThreeColumnBrickLayout(){
        float xPos;
        for(int i = 0; i < brickRows; i++){                             // for each row of bricks
            // first column
            xPos = 0f - groundWidth/2f + Random.Range(0, maxBrickLength); // get center of brick next to left wall as current position
            MakeRow(xPos, 0f - groundWidth/6f, i);
            // second column
            xPos = 0f - groundWidth/6f + Random.Range(0, maxBrickLength); // get center of brick next to left wall as current position
            MakeRow(xPos, 0 + groundWidth/6f, i);
            // third column
            xPos = 0f + groundWidth/6f + Random.Range(0, maxBrickLength); // get center of brick next to left wall as current position
            MakeRow(xPos, 0 + groundWidth/2f, i);
            if(rowSkip != 0){
                rowSkip += Random.Range(0, rowSkipMax);
            }
        }
    }

    // mix up the ratio of brick types
    private void BrickMix(){
        float chanceSum = 0;
        for(int i = 0; i < maxBrickTypes; i++){
            chanceForBrickType[i] = Random.Range(0f, 1f);
            chanceSum += chanceForBrickType[i];
        }
        for(int i = 0; i < maxBrickTypes; i++){
            chanceForBrickType[i] /= chanceSum;
        }
    }

    // adds pairs of random bricks between the player and the primary rows
    private void AddRandomBricks(){
        float minZ = playerPrefab.transform.position.z + 1; // row above the player
        float maxZ = brickZPosStart;                        // row where primary rows start
        float minX = 0f;                                    // center of floor
        float maxX = groundWidth / 2f - maxBrickLength;     // edge of floor
        int numRandomBricks = Random.Range(0, (int)(maxZ - minZ));  // number of pairs to generate
        int randBrickZStartPos = (int)Random.Range(minZ, maxZ - numRandomBricks);   // first row to populate
        for(int i = 0; i < numRandomBricks; i++){          
            float xPos = Random.Range(minX, maxX);          // x position
            GameObject brickChoice = ChooseBrick();         // get a brick type
            float brickLength = brickChoice.GetComponent<MeshRenderer>().bounds.size.x;   // get the length of this brick
            GameObject newBrick = Instantiate(brickChoice, new Vector3(xPos - (brickLength / 2f), 2, (float)randBrickZStartPos) + transform.position, brickChoice.transform.rotation, this.transform);  // create brick at current position
            InitBrick(newBrick);
            newBrick = Instantiate(brickChoice, new Vector3(-xPos + (brickLength / 2f), 2, (float)randBrickZStartPos) + transform.position, brickChoice.transform.rotation, this.transform);  // create brick at current position
            InitBrick(newBrick);
            randBrickZStartPos++;
            if((maxZ - randBrickZStartPos) > (numRandomBricks - i + 1)){    // maybe skip a row?
                int skipRowChance = Random.Range(0, 1);
                if(skipRowChance > 0.5){
                    randBrickZStartPos++;
                }
            }
        }
    }

    // sets class variables to randomize scene loosely based on game level progression
    private void RunSceneGenerator(){
        if(playerId != 1){
            return;
        }
        switch(levelProgression){
            case 1:                             // level 1
                maxBrickTypes = 2;
                break;
            case 2:                             // level 2
                maxBrickTypes = 3;              // more types of bricks
                brickColumnCount = 2;           // change layout
                brickRows = 5;                  // more rows
                groundMatChoice = 2;            // change floor material
                wallMatChoice = 2;              // change wall material
                break;
            case 3:                             // level 3
                maxBrickTypes = brickPrefabs.Length;    // all brick types
                brickColumnCount = 3;           // change layout
                brickRows = 5;                  // same rows
                groundMatChoice = 3;            // change floor material
                wallMatChoice = 3;              // change wall material
                break;
            case 4:                             // level 4
                maxBrickTypes = brickPrefabs.Length;    // all brick types
                brickColumnCount = Random.Range(1, 4);  // random layout
                brickRows = Random.Range(4, 8);         // random number of rows
                chanceForPowerUp = Random.Range(0.2f, 0.5f);    // change odds for a powerup
                brickZPosStart = Random.Range(brickZMin, brickZMax);    // move bottom of primary rows around
                rowSkip = Random.Range(0, 2);           // maybe skip a row of bricks?
                if(rowSkip == 1){
                    brickZPosStart--;           // add space just in case there isn't enough room
                }
                groundMatChoice = Random.Range(0, groundMaterials.Length);  // random floor material
                wallMatChoice = Random.Range(0, wallMaterials.Length);      // random wall material
                break;
            default:                            // level 5 or higher - go crazy
                maxBrickTypes = brickPrefabs.Length;
                brickColumnCount = Random.Range(1, 4);
                brickRows = Random.Range(4, 8);
                chanceForPowerUp = Random.Range(0.2f, 0.5f);
                brickZPosStart = Random.Range(brickZMin, brickZMax);
                rowSkip = Random.Range(0, 2);
                if(rowSkip == 1){
                    brickZPosStart--;
                }
                groundMatChoice = Random.Range(0, groundMaterials.Length);
                wallMatChoice = Random.Range(0, wallMaterials.Length);
                BrickMix();                     // change odds for bricks types
                doAddRandomBricks = true;       // add random bricks
                break;
        }
    }

    // Agent specific block
    // function provided for Agents to reset the scene
    public void SetScene(){
        InitBall();
        SetBricks();                                                        // set the bricks in the scene
        player.transform.position = startPosition;
    }

    // function provided for Agents to clear the scene
    public void ClearScene(){
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        if(bricks.Length != 0){
            foreach(var brick in bricks){
                if(brick.GetComponent<BrickController>().playerId == playerId){
                    Destroy(brick);
                }
            }
        }
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        if(balls.Length != 0){
            foreach(var ball in balls){
                if(ball.GetComponent<BallController>().playerId == playerId){
                    Destroy(ball);
                }                
            }
        }
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        if(powerups.Length != 0){
            foreach(var powerup in powerups){
                if(powerup.GetComponent<PowerupController>().playerId == playerId){
                    Destroy(powerup);
                }
            }
        }
        GameObject[] powerupIndicators = GameObject.FindGameObjectsWithTag("PowerupIndicator");
        if(powerupIndicators.Length != 0){
            foreach(var indicator in powerupIndicators){
                if(indicator.GetComponent<PowerupIndicatorController>().playerId == playerId){
                    Destroy(indicator);
                }
            }
        }
    }

    // make copies of scene for training agents
    private void MakeSceneCopies(){
        if(playerId != 1){
            return;
        }
        Vector3 nextPosition = transform.position + new Vector3(200f, 0f, 0f);
        for(int i = 0; i < numSimulations; i++){
            GameObject newLevelController = Instantiate(levelControllerPrefab, nextPosition, levelControllerPrefab.transform.rotation);
            LevelController levCon = newLevelController.GetComponent<LevelController>();
            levCon.playerId = playerId + 1 + i;
            levCon.isAgent = true;
            levCon.isTraining = true;
            levCon.gameManager = gameManager;
            levCon.chanceForPowerUp = chanceForPowerUp;
            levCon.levelProgression = levelProgression;
            newLevelController.SetActive(true);
            nextPosition += new Vector3(200f, 0f, 0f);
        }
    }

}
