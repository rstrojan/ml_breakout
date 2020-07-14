using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// powerup to speed up player paddle
public class FastPadPwrController : PowerupController
{
    private float playerStartSpeed;
    [SerializeField] float speedMult = 2f;

    public override void StartEffect(){
        Debug.Log("start speed");
        playerStartSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed;    // copy current player speed
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed *= speedMult;          // change player speed
    }

    public override void EndEffect(){
        Debug.Log("end speed");
        Debug.Log("playerStartSpeed: " + playerStartSpeed);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed = playerStartSpeed;    // put player speed back
        Debug.Log("reset speed: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed);
    }
}
