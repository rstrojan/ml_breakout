﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
    public float speed;
    public float duration;
    public bool isActive = false;
    public GameObject powerupIndicator;

    public virtual void StartEffect(){
        Debug.Log("running generic start");
    }

    public virtual void EndEffect(){
        Debug.Log("running generic end");
    }
}
