using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{
    private Rigidbody rBody;
    public LevelController levelController;
    private PlayerController playerController;
    private float speed;
    public bool fire;
    public bool useVectorObs;
    private int horizontalInput;
    public GameObject ball;

    public override void Initialize() {
        levelController = transform.parent.gameObject.GetComponent<LevelController>();
        playerController = gameObject.GetComponent<PlayerController>();
    }

    // resets the game for a new round of training
    public override void OnEpisodeBegin()
    {
        levelController.ClearScene();
        levelController.SetScene();
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    void Start(){
        speed = playerController.speed;
    }

    void Update(){
        speed = playerController.speed;        
        // transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Debug.Log("action0: " + vectorAction[0]);
        var moveChoice = Mathf.FloorToInt(vectorAction[0]);
        Move(moveChoice);
        // var fireChoice = Mathf.FloorToInt(vectorAction[1]);
        // DoFire(fireChoice);        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // x position of paddle
        sensor.AddObservation(this.transform.localPosition.x);

        // x any y position of ball
        if (ball != null)
        {
            sensor.AddObservation(ball.transform.localPosition.x);
            sensor.AddObservation(ball.transform.localPosition.y);
        }
        else
        {
            sensor.AddObservation(0.0f);
            sensor.AddObservation(0.0f);
        }
    }

    private void Move(int moveChoice){

        switch(moveChoice){
            case 1:
                horizontalInput = -1;
                break;
            case 2:
                horizontalInput = 1;
                break;
            default:
                horizontalInput = 0;
                break;
        }
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 2;
        }
    }

    private void DoFire(int fireChoice){
        switch(fireChoice){
            case 1:
                fire = true;
                break;
            default:
                fire = false;
                break;
        }
    }

    public void LostBall(){
        // small negative reward
        // Debug.Log("Lost Ball");
        // AddReward(-0.1f);
    }

    public void LostAllBalls(){
        // big negative reward
        // Debug.Log("Lost all balls");
        AddReward(-1.0f);
        EndEpisode();
    }

    public void GotPowerUp(){
        // small positive reward
        // Debug.Log("Got Powerup");
        // AddReward(0.2f);
    }

    public void DestroyedBrick(){
        // medium positive reward
        // Debug.Log("Destroyed Brick");
        AddReward(1.0f);
    }

    public void DestroyedAllBricks(){
        // huge positive reward
        // Debug.Log("Destroyed all bricks");
        // AddReward(1.0f);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ball")){
            // small positive reward
            // Debug.Log("Hit Ball");
            AddReward(1.0f);
        }
    }
}
