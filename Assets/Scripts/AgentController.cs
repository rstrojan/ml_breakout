using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{
    private Rigidbody rBody;
    public GameObject levelController;
    private float speed;
    
    // resets the game for a new round of training
    public override void OnEpisodeBegin()
    {
        levelController.GetComponent<LevelController>().ClearScene();
        levelController.GetComponent<LevelController>().SetScene();
    }

    void Start(){
        speed = gameObject.GetComponent<PlayerController>().speed;
    }

    void Update(){
        speed = gameObject.GetComponent<PlayerController>().speed;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log("action: " + vectorAction);
        int horizontalInput;
        var action = Mathf.FloorToInt(vectorAction[0]);

        switch(action){
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

    public void LostBall(){
        // small negative reward
        Debug.Log("Lost Ball");
        AddReward(-0.1f);
    }

    public void LostAllBalls(){
        // big negative reward
        Debug.Log("Lost all balls");
        AddReward(-1.0f);
        EndEpisode();
    }

    public void GotPowerUp(){
        // small positive reward
        Debug.Log("Got Powerup");
        AddReward(0.1f);
    }

    public void DestroyedBrick(){
        // medium positive reward
        Debug.Log("Destroyed Brick");
        AddReward(0.2f);
    }

    public void DestroyedAllBricks(){
        // huge positive reward
        Debug.Log("Destroyed all bricks");
        AddReward(1.0f);
    }
}
