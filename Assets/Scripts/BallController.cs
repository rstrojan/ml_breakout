using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameManager gameManager;
    public Ball ball;
    private Vector3 lastUpdateVelocity;

    private float xRange;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        xRange = GetFloorRange();
        
        SetBallVelocity(ball.startVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        CheckXBoundary();
        // maintain constant speed
        // CheckSpeed();    - direction transition requires low magnitudes for duration. maybe count frames?
        lastUpdateVelocity = ball.ballRb.velocity;

    }

    private void OnCollisionEnter(Collision other) {
        
        ContactPoint hitAngle = other.contacts[0];          // get point of contact with other object
        ReflectBounce(hitAngle.normal);                 // change forward direction
        if (other.gameObject.CompareTag("Brick"))
        {
            // update the score
            Brick brick = other.gameObject.GetComponent<BrickController>().brick;
            if(brick.IsDestroyed()){
                gameManager.UpdateScore(brick.scoreValue);
                if(brick.hasPowerUp){
                    Instantiate(brick.powerup, other.gameObject.transform.position, brick.powerup.transform.rotation);
                }
                Destroy(other.gameObject);                      // destroy brick
            }
        }
        if(other.gameObject.CompareTag("Bottom Sensor")){
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    // change forward direction of ball based on collision
    private void ReflectBounce(Vector3 barrierNormal){
        Vector3 newAngle = Vector3.Reflect(lastUpdateVelocity.normalized, barrierNormal); // get angle of reflection
        SetBallVelocity(newAngle * ball.startVelocity.magnitude);
    }

    // check for slowed or stopped ball
    private void CheckSpeed(){
        if(ball.ballRb.velocity.magnitude < ball.startVelocity.magnitude){
            Debug.Log("Check speed vel: " + ball.ballRb.velocity);
            SetBallVelocity(lastUpdateVelocity.normalized * ball.startVelocity.magnitude); 
        }
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
        ball.ballRb.velocity = newVelocity;
    }
}
