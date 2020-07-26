using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// powerup to speed up player paddle
public class FastPadPwrController : PowerupController
{
    private float playerStartSpeed;
    [SerializeField] float speedMult = 2f;

    public override void StartEffect(){
        playerStartSpeed = player.GetComponent<PlayerController>().speed;    // copy current player speed
        player.GetComponent<PlayerController>().speed *= speedMult;          // change player speed
    }

    public override void EndEffect(){
        player.GetComponent<PlayerController>().speed = playerStartSpeed;    // put player speed back
    }
}
