using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PaddleAgent : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // each time a new training episode begins, instantiate
    // a new GameManager, call StartGame method
    public override void OnEpisodeBegin()
    {
        GameManager manager = new GameManager();
        manager.StartGame();
    }

    // Observations:
    // - distance from paddle to ball
    // - direction of ball relative to center of paddle
    // - distance from each wall to edge of paddle
    public override void CollectObservations(VectorSensor sensor)
    {
        


    }

    // Rewards:
    // - tiny reward for the ball moving
    // - tiny reward for deflecting ball with paddle
    // - large reward for breaking bricks
    // - large penalty for losing ball
    public override void OnActionReceived(float[] vectorAction)
    {



        
    }

}
