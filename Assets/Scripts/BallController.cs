using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // private GameManager gameManager;
    // private GameObject levelController;
    private Rigidbody ballRb;
    public Vector3 startVelocity;
    private Vector3 lastUpdateVelocity;
    private float freezeCheck = 0;
    public float hitPower;

    private float xRange;
    
    // Start is called before the first frame update
    void Awake(){
        // gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        // levelController = GameObject.Find("Level Controller");
        xRange = GetFloorRange();
        ballRb = gameObject.GetComponent<Rigidbody>();
        SetBallVelocity(startVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        CheckXBoundary();
        ballRb.velocity = ballRb.velocity.normalized * startVelocity.magnitude; // maintain constant speed
        if(ballRb.velocity.magnitude < startVelocity.magnitude){
            freezeCheck += Time.deltaTime;
            if(freezeCheck >= 0.5f){
                ballRb.velocity = startVelocity;
                freezeCheck = 0;
            }
        }
        else{
            freezeCheck = 0;
        }
        lastUpdateVelocity = ballRb.velocity;           // always grab current velocity

    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint hitAngle = other.contacts[0];      // get point of contact with other object
        ReflectBounce(hitAngle.normal);                 // change forward direction
        // if(other.gameObject.CompareTag("Brick"))
        // {
        // //     Brick brick = other.gameObject.GetComponent<BrickController>().brick;
        // //     if(brick.IsDestroyed(hitPower)){                        // check if brick is destructable and hits left is 0
        // //         gameManager.UpdateScore(brick.scoreValue);  // update the score
        // //         if(brick.hasPowerUp){
        // //             Instantiate(brick.powerup, other.gameObject.transform.position, brick.powerup.transform.rotation);  // create powerup
        // //         }
        // //         Destroy(other.gameObject);                      // destroy brick
        // //         levelController.GetComponent<LevelController>().destructableBrickCount--;
        // //     }
        // // }
        if(other.gameObject.CompareTag("Bottom Sensor")){
            Destroy(gameObject);
        }
    }

    // change forward direction of ball based on collision
    private void ReflectBounce(Vector3 barrierNormal){
        Vector3 newAngle = Vector3.Reflect(lastUpdateVelocity.normalized, barrierNormal); // get angle of reflection
        SetBallVelocity(newAngle * startVelocity.magnitude);
    }

    // check for ball going through walls
    private void CheckXBoundary(){
        if(transform.position.x > xRange){
            transform.position = new Vector3(xRange * 0.9f, transform.position.y, transform.position.z);
        }
        if(transform.position.x < -xRange){
            transform.position = new Vector3(-xRange * 0.9f, transform.position.y, transform.position.z);
        }
    }

    // return size of current level floor
    private float GetFloorRange(){
        return (GameObject.Find("Ground").GetComponent<MeshRenderer>().bounds.size.x / 2f);
    }

    // set new ball velocity
    public void SetBallVelocity(Vector3 newVelocity){
        ballRb.velocity = newVelocity;
    }
}
