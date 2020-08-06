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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // resets the game for a new round of training
    public override void OnEpisodeBegin()
    {
        levelController.GetComponent<LevelController>().ClearScene();
        levelController.GetComponent<LevelController>().SetScene();
    }

    public override void OnActionReceived(float[] vectorAction)
    {

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
