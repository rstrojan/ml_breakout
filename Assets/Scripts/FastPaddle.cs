using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPaddle : Powerup
{
    private float playerStartSpeed;
    [SerializeField] float speedMult = 1.5f;

    public override void StartEffect(){
        playerStartSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed *= speedMult;
    }

    public override void EndEffect(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed *= playerStartSpeed;
    }
}
