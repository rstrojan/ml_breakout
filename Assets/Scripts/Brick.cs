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

    public SFXController sfx;

    public bool IsDestroyed(float hitPower){
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();

        if (!isDestructable){
            sfx.PlayBrickMetal();
            return false;
        }
        else{
            hitCount -= hitPower;
            if(hitCount <= 0){
                sfx.PlayBrickBreak();
                return true;
            }
            else{
                sfx.PlayBrickHit();
                return false;
            }
        }
    }
}
