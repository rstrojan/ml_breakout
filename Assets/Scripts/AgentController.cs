using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LostBall(){
        // small negative reward
        Debug.Log("Lost Ball");
    }

    public void LostAllBalls(){
        // big negative reward
        Debug.Log("Lost all balls");
    }

    public void GotPowerUp(){
        // small positive reward
        Debug.Log("Got Powerup");
    }

    public void DestroyedBrick(){
        // medium positive reward
        Debug.Log("Destroyed Brick");
    }

    public void DestroyedAllBricks(){
        // huge positive reward
        Debug.Log("Destroyed all bricks");
    }
}
