using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brick
{
    public float hitCount;
    public bool isDestructable;
    public int scoreValue;
    public bool hasPowerUp;
    public GameObject powerup;

    public bool IsDestroyed(float hitPower){
        if(!isDestructable){
            return false;
        }
        else{
            hitCount -= hitPower;
            if(hitCount <= 0){
                return true;
            }
            else{
                return false;
            }
        }
    }
}
