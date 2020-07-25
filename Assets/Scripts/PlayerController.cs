using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerId;
    public bool isTwoPlayer;
    private string horizontalAxis;

    private Rigidbody playerRb;
    public float speed;
    public bool isSticky;
    public bool hasPowerup;

    private float horizontalInput;                       // input from player
    private float xRange;                       // limit left right movement
    private Vector3 startPos;                   // to save start position
    private bool isColliding;

    private void Awake() {
        startPos = transform.position;
        xRange = GetFloorRange();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playerId == 1){
            if(isTwoPlayer){
                horizontalAxis = "Player1_Horizontal";
                Debug.Log("pid: " + playerId + ", axis: " + horizontalAxis);
            }
            else{
                horizontalAxis = "Horizontal";
                Debug.Log("pid: " + playerId + ", axis: " + horizontalAxis);
            }
            
        }
        if((playerId == 2)){
            horizontalAxis = "Player2_Horizontal";
            Debug.Log("pid: " + playerId + ", axis: " + horizontalAxis);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
        MovePlayer();
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
        }
    }

}
