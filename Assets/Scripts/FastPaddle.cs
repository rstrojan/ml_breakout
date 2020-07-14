using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FastPaddle : Powerup
{
    private float playerStartSpeed;
    [SerializeField] float speedMult = 1.5f;

    public override void StartEffect(){
        Debug.Log("starteffect flip fleep");
        playerStartSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed *= speedMult;
    }

    public override void EndEffect(){
        Debug.Log("endeffect beep boop");
        Debug.Log("playerStartSpeed: " + playerStartSpeed);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.speed = playerStartSpeed;
    }
}
