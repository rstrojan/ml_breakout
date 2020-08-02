using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PaddleAgent : Agent
{
    GameObject myBall;
    Rigidbody rBody;
    private int playerID;
    public GameObject levelController;

    // called after object is instantiated, before any action
    void Start()
    {
        rBody = gameObject.GetComponent<Rigidbody>();
        playerID = gameObject.GetComponent<PlayerController>().playerId;

        Debug.Log("Paddle playerID: " + playerID);
    }

    // each time a new training episode begins, instantiate
    // a new GameManager, call StartGame method
    // 
    public override void OnEpisodeBegin()
    {
        Debug.Log("In OnEpisodeBegin");
        levelController.GetComponent<LevelController>().SetScene();
    }

    // Observations:
    // - distance from paddle to ball (done)
    // - direction of ball relative to center of paddle (done)
    // - distance from each wall to edge of paddle
    public override void CollectObservations(VectorSensor sensor)
    {
        // find all balls
        GameObject[] balls;
        balls = GameObject.FindGameObjectsWithTag("Ball");

        // 1. get correct ball
        foreach (var ball in balls)
        {
            if (ball.GetComponent<BallController>().playerId == playerID)
            {
                myBall = ball;
                break;
            }
        }

        // 2. get my own position, ball position (potentially use this)
        Vector3 myPosition = this.transform.localPosition;
        Vector3 ballPosition = myBall.transform.localPosition;

        // 3. take difference, pass to sensor
        sensor.AddObservation(myPosition - ballPosition);

        // take direction relative to center of paddle
        // Vector3 ballDirection = myBall.GetComponent<Rigidbody>().velocity;
        float angle = Vector3.Angle(myBall.transform.forward, this.transform.forward);
        sensor.AddObservation(angle);

        // distance from each wall to edge of paddle
        // float leftDist = Vector3.Distance(myPosition.x - 2.5f, 
        //                                   GameObject.Find("Wall_Left").transform.localPosition.x);
        float leftDist = Vector3.Distance(myPosition, 
                                          GameObject.Find("Wall_Left").transform.localPosition);
        sensor.AddObservation(leftDist);

        // This didn't work
        // float rightDist = Vector3.Distance(myPosition.x + 2.5f, 
        //                                    GameObject.Find("Wall_Right").transform.localPosition.x);
        float rightDist = Vector3.Distance(myPosition,
                                           GameObject.Find("Wall_Right").transform.localPosition);
        sensor.AddObservation(rightDist);
    }

    // Rewards:
    // - tiny reward for the ball moving (done)
    // - tiny reward for deflecting ball with paddle
    // - large reward for breaking bricks
    // - large penalty for losing ball
    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 1 (only changing x position)
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        rBody.AddForce(controlSignal * speed);

        // tiny reward for ball moving
        if (myBall.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            SetReward(0.1f);
        }
    }

    // Function called by the BallController when it detects that it has
    // collided with the paddle
    public void HitBall()
    {
        AddReward(0.2f);
    }

    // Function called by BrickController when the brick is broken
    public void BrokeBrick()
    {
        AddReward(0.75f);
    }

    public void LostBall()
    {
        AddReward(-1f);
        levelController.GetComponent<LevelController>().ClearScene();
        EndEpisode();
    }
}
