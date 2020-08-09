using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public LevelController levelController;
    public int playerId;
    public GameObject player;
    private Rigidbody ballRb;
    public Vector3 startVelocity;
    private Vector3 lastUpdateVelocity;
    private float freezeCheck = 0;
    public float hitPower;
    private float halfSize;
    public float minXPosition;
    public float maxXPosition;
    private bool isColliding;

    public bool bottomSensorBounce;
    
    // Start is called before the first frame update
    void Awake(){
        levelController = transform.parent.gameObject.GetComponent<LevelController>();
        playerId = levelController.playerId;
        player = levelController.player;
        ballRb = gameObject.GetComponent<Rigidbody>();
        halfSize = gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2f;
        SetBallVelocity(startVelocity);
    }

    private void Start() {
        GetFloorRange();
    }

    // Update is called once per frame
    void Update()
    {
        CheckXBoundary();
        ballRb.velocity = ballRb.velocity.normalized * startVelocity.magnitude; // maintain constant speed
        if(ballRb.velocity.magnitude < startVelocity.magnitude){
            freezeCheck += Time.deltaTime;
            if(freezeCheck >= 0.8f){
                ballRb.velocity = startVelocity;
                freezeCheck = 0;
            }
        }
        else{
            freezeCheck = 0;
        }
        lastUpdateVelocity = ballRb.velocity;           // always grab current velocity
        isColliding = false;
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint hitAngle = other.contacts[0];      // get point of contact with other object
        ReflectBounce(hitAngle.normal);                 // change forward direction
        if(other.gameObject.CompareTag("Bottom Sensor")){
            if(isColliding || bottomSensorBounce){
                return;
            }
            else{
                isColliding = true;     // prevent multiple registers of the collision as it passes through the sensor
            }
            levelController.ballCount--;
            if(player.GetComponent<PlayerController>().isAgent){
                player.GetComponent<AgentController>().LostBall();
            } 
            Destroy(gameObject);
        }
    }

    // change forward direction of ball based on collision
    private void ReflectBounce(Vector3 barrierNormal){
        // Vector3 newAngle = Vector3.Reflect(lastUpdateVelocity.normalized, barrierNormal); // get angle of reflection
        Vector3 newAngle = Vector3.Reflect(lastUpdateVelocity, barrierNormal);
        // SetBallVelocity(newAngle * startVelocity.magnitude);
        SetBallVelocity(newAngle);
    }

    // check for ball going through walls
    private void CheckXBoundary(){
        if(transform.position.x > maxXPosition){
            transform.position = new Vector3(maxXPosition, transform.position.y, transform.position.z);
        }
        if(transform.position.x < minXPosition){
            transform.position = new Vector3(minXPosition, transform.position.y, transform.position.z);
        }
    }

    // return size of current level floor
    private void GetFloorRange(){
        float xRange = GameObject.Find("Ground").GetComponent<MeshRenderer>().bounds.size.x / 2f;
        minXPosition = transform.parent.gameObject.transform.position.x - xRange + halfSize;
        maxXPosition = transform.parent.gameObject.transform.position.x + xRange - halfSize;
    }

    // set new ball velocity
    public void SetBallVelocity(Vector3 newVelocity){
        ballRb.velocity = newVelocity;
    }
}
