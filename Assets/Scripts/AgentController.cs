﻿using System;
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
    public bool isTraining;

    public override void Initialize() {
        levelController = transform.parent.gameObject.GetComponent<LevelController>();
        playerController = gameObject.GetComponent<PlayerController>();
        isTraining = levelController.isTraining;
    }

    // resets the game for a new round of training
    public override void OnEpisodeBegin()
    {
        if(isTraining){
            levelController.ClearScene();
            levelController.SetScene();
        }
    }

    void Start(){
        speed = playerController.speed;
    }

    void Update(){
        speed = playerController.speed;        
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Debug.Log("action0: " + vectorAction[0] + "\naction1: " + vectorAction[1]);
        var moveChoice = Mathf.FloorToInt(vectorAction[0]);
        Move(moveChoice);
        var fireChoice = Mathf.FloorToInt(vectorAction[1]);
        DoFire(fireChoice);        
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
        AddReward(-0.1f);
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
        AddReward(0.3f);
    }

    public void DestroyedBrick(){
        // medium positive reward
        // Debug.Log("Destroyed Brick");
        AddReward(0.05f);
    }

    public void DestroyedAllBricks(){
        // huge positive reward
        // Debug.Log("Destroyed all bricks");
        AddReward(10.0f);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ball")){
            // small positive reward
            // Debug.Log("Hit Ball");
            AddReward(1.0f);
        }
    }
}
