﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleGunController : MonoBehaviour
{
    private float projectileOffsetX;
    private float projectileOffsetZ;
    public KeyCode fireKey;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        FindFireKey();
        projectileOffsetX = gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2f; // get size of paddle gun object
        projectileOffsetX *= 0.9f;      // bring offset in from edge
        projectileOffsetZ = ObjectPooler.SharedInstance.GetPooledObject1().GetComponent<MeshRenderer>().bounds.size.z / 2f; // set to edge of projectile
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(fireKey)){      
            GameObject pooledProjectile1 = ObjectPooler.SharedInstance.GetPooledObject1();  // get first pooled object
            GameObject pooledProjectile2 = ObjectPooler.SharedInstance.GetPooledObject2();  // get second pooled object
            if(pooledProjectile1 != null){
                pooledProjectile1.SetActive(true); // activate it
                pooledProjectile1.transform.position = new Vector3(transform.position.x + projectileOffsetX, transform.position.y, transform.position.z + projectileOffsetZ); // position it at player
            }
            if(pooledProjectile2 != null){
                pooledProjectile2.SetActive(true); // activate it
                pooledProjectile2.transform.position = new Vector3(transform.position.x - projectileOffsetX, transform.position.y, transform.position.z + projectileOffsetZ); // position it at player
            }
        }
    }

    private void FindFireKey(){
        bool isTwoPlayer = GameManager.isTwoPlayer;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++){
            if(players[i].transform.position.x == transform.position.x){
                player = players[i];
                break;
            }
        }
        if(player.GetComponent<PlayerController>().playerId == 1){
            if(isTwoPlayer){
                fireKey = KeyCode.LeftControl;
            }
            else{
                fireKey = KeyCode.Space;
            }
        }
        else{
            fireKey = KeyCode.RightAlt;
        }
    }

}
