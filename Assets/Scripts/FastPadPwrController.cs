using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// powerup to speed up player paddle
public class FastPadPwrController : PowerupController
{
    private float playerStartSpeed;
    [SerializeField] float speedMult = 2f;

    public override void StartEffect(){
        playerStartSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed;    // copy current player speed
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed *= speedMult;          // change player speed
    }

    public override void EndEffect(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed = playerStartSpeed;    // put player speed back
    }
}
