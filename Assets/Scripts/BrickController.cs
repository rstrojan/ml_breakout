using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public int playerId;
    public GameManager gameManager;     // assigned in Level Controller
    public GameObject levelController;  // assigned in Level Controller
    public GameObject player;           // assigned in Level Controller
    public Brick brick;

    // brick is hit by ball
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ball")){
            float hitPower = other.gameObject.GetComponent<BallController>().hitPower;
            HitBrick(hitPower);
        }
    }

    // brick is hit by projectile
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Projectile")){
            float hitPower = other.gameObject.GetComponent<ProjectileController>().hitPower;
            other.gameObject.SetActive(false);                   // deactivate projectile
            HitBrick(hitPower);
        }
    }

    // 
    public void HitBrick(float hitPower){
        if(brick.IsDestroyed(hitPower)){                 // check if thisBrick is destructable and hits left is 0
            gameManager.UpdateScore(brick.scoreValue, playerId);          // update the score
            if(brick.hasPowerUp){
                GameObject powerup = Instantiate(brick.powerup, transform.position, brick.powerup.transform.rotation);  // create powerup
                powerup.GetComponent<PowerupController>().playerId = playerId;      // pass player ID to powerup object
                powerup.GetComponent<PowerupController>().player = player;          // pass correct player to powerup object
                powerup.GetComponent<PowerupController>().levelController = levelController;    // pass correct level controller to powerup object
            }
            levelController.GetComponent<LevelController>().destructableBrickCount--;   // decrease brick count
            Destroy(gameObject);                          // destroy brick
        }
    }

}


