using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brick
{
    public int hitCount;
    public bool isDestructable;
    public int scoreValue;
    public bool hasPowerUp;
    public GameObject powerup;

    public bool IsDestroyed(){
        if(!isDestructable){
            return false;
        }
        else{
            hitCount--;
            if(hitCount == 0){
                return true;
            }
            else{
                return false;
            }
        }
    }
}
