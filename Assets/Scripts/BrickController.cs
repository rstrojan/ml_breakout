using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject levelController;
    public Brick brick;

    private void Awake() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        levelController = GameObject.Find("Level Controller");
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ball")){
            float hitPower = other.gameObject.GetComponent<BallController>().hitPower;
            HitBrick(hitPower);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Projectile")){
            float hitPower = other.gameObject.GetComponent<ProjectileController>().hitPower;
            other.gameObject.SetActive(false);                            // deactivate projectile
            HitBrick(hitPower);
        }
    }

    public void HitBrick(float hitPower){
        // Brick thisBrick = brick.GetComponent<BrickController>().brick;
        if(brick.IsDestroyed(hitPower)){                        // check if thisBrick is destructable and hits left is 0
            gameManager.UpdateScore(brick.scoreValue);          // update the score
            if(brick.hasPowerUp){
                Instantiate(brick.powerup, transform.position, brick.powerup.transform.rotation);  // create powerup
            }
            levelController.GetComponent<LevelController>().destructableBrickCount--;   // decrease brick count
            Destroy(gameObject);                          // destroy brick
        }
        
    }

    
}


