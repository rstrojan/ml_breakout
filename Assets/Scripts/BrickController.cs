using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public int playerId;
    public GameManager gameManager;     // assigned in Level Controller
    public LevelController levelController;  // assigned in Level Controller
    public GameObject player;           // assigned in Level Controller
    public Brick brick;

    public SFXController sfx;

    private void Awake() {
        levelController = transform.parent.gameObject.GetComponent<LevelController>();
        playerId = levelController.playerId;
        player = levelController.player;
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();
    }

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
                GameObject powerup = Instantiate(brick.powerup, transform.position, brick.powerup.transform.rotation, transform.parent);  // create powerup
                InitPowerup(powerup);
            }
            levelController.destructableBrickCount--;   // decrease brick count
            if(player.GetComponent<PlayerController>().isAgent){
                player.GetComponent<AgentController>().DestroyedBrick();
            }
            Destroy(gameObject);                          // destroy brick
            sfx.PlayBrickBreak();
        }
        else
        {
            sfx.PlayBrickHit();
        }
    }

    private void InitPowerup(GameObject powerup){
        powerup.SetActive(true);
    }

}


