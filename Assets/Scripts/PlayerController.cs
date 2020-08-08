using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LevelController levelController;
    public int playerId;
    public bool isAgent;
    public bool isTwoPlayer;
    private string horizontalAxis;

    private Rigidbody playerRb;
    public float speed;
    public bool hasPowerup;

    private float horizontalInput;                       // input from player
    private float xRange;                       // limit left right movement
    private Vector3 startPos;                   // to save start position
    private bool isColliding;

    private void Awake() {
        levelController = transform.parent.gameObject.GetComponent<LevelController>();
        playerId = levelController.playerId;
        isAgent = levelController.isAgent;
        isTwoPlayer = LevelController.isTwoPlayer;
        startPos = transform.position;
        xRange = GetFloorRange();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("running player Start()\nplayerId: " + playerId);
        if(playerId == 1 && !isAgent){
            if(isTwoPlayer){
                horizontalAxis = "Player1_Horizontal";
            }
            else{
                horizontalAxis = "Horizontal";
            }
            
        }
        if((playerId == 2 && !isAgent)){
            horizontalAxis = "Player2_Horizontal";
        }
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
        if(!isAgent){
            MovePlayer();
        }
        ConstrainPlayer();        
    }

    // get player input to move left or right
    private void MovePlayer(){
        horizontalInput = Input.GetAxisRaw(horizontalAxis);
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
    }

    // Limit player motion from left to right
    private void ConstrainPlayer(){
        if(transform.position.x <= -xRange + startPos.x){
            transform.position = new Vector3(-xRange + startPos.x, startPos.y, startPos.z);
        }
        if(transform.position.x >= xRange + startPos.x){
            transform.position = new Vector3(xRange + startPos.x, startPos.y, startPos.z);
        }
    }

    // return size of current level floor minus player size
    private float GetFloorRange(){
        return (GameObject.Find("Ground").GetComponent<MeshRenderer>().bounds.size.x / 2f) - (gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2f);
    }

    // catch a power up
    private void OnTriggerEnter(Collider other) {
        if(isColliding){
            return;
        }
        isColliding = true;
        if(other.CompareTag("Powerup")){
            other.GetComponent<PowerupController>().ActivateEffect();
            if(isAgent){
                gameObject.GetComponent<AgentController>().GotPowerUp();
            }
        }
    }

}
