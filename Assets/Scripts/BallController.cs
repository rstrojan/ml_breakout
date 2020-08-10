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
    public float hitPower;
    private float halfSize;
    public float minXPosition;
    public float maxXPosition;
    private float floorZRange;
    public bool bottomSensorBounce;

    [Header("Bad Behavior Checks")]
    private Vector3 lastUpdateVelocity;
    private bool isColliding;
    private float freezeTimer = 0;
    private float zeroXTimer = 0;
    private float zeroZTimer = 0;
    
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
        GetFloorXRange();
        GetFloorZRange();
    }

    // Update is called once per frame
    void Update()
    {
        CheckXBoundary();
        ballRb.velocity = ballRb.velocity.normalized * startVelocity.magnitude; // maintain constant speed
        FreezeChecks();
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

    // return x size of current level floor
    private void GetFloorXRange(){
        float xRange = GameObject.Find("Ground").GetComponent<MeshRenderer>().bounds.size.x / 2f;
        minXPosition = transform.parent.gameObject.transform.position.x - xRange + halfSize;
        maxXPosition = transform.parent.gameObject.transform.position.x + xRange - halfSize;
    }

    // return z size of current level floor
    private void GetFloorZRange(){
        floorZRange = GameObject.Find("Ground").GetComponent<MeshRenderer>().bounds.size.z;
    }

    // set new ball velocity
    public void SetBallVelocity(Vector3 newVelocity){
        ballRb.velocity = newVelocity;
    }

    
    private void FreezeChecks(){
        // check if ball is stuck somewhere
        if(ballRb.velocity.magnitude < startVelocity.magnitude){
            freezeTimer += Time.deltaTime;
            if(freezeTimer >= 0.8f){
                float x = Random.Range(1f, startVelocity.magnitude);
                float z = Random.Range(1f, startVelocity.magnitude);
                ballRb.velocity = new Vector3(x, 0, z);
            }
        }
        else{
            freezeTimer = 0;
        }

        // check if ball is being boring
        if(ballRb.velocity.x == 0){
            zeroXTimer += Time.deltaTime;
            if (zeroXTimer >= ((maxXPosition - minXPosition) * 2)){
                ballRb.velocity = new Vector3(0.1f, 0, ballRb.velocity.z); 
            }
        }
        else{
            zeroXTimer = 0;
        }

        if(ballRb.velocity.z == 0){
            zeroZTimer += Time.deltaTime;
            if(zeroZTimer >= (floorZRange * 2)){
                ballRb.velocity = new Vector3(ballRb.velocity.x, 0, 0.1f);
            }
        }

    }

}
