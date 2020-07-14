using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPadPwrController : PowerupController
{
    private float playerStartSpeed;
    [SerializeField] float speedMult = 2f;

    public override void StartEffect(){
        Debug.Log("start speed");
        playerStartSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed *= speedMult;
    }

    public override void EndEffect(){
        Debug.Log("end speed");
        Debug.Log("playerStartSpeed: " + playerStartSpeed);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed = playerStartSpeed;
        Debug.Log("reset speed: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed);
    }
}
