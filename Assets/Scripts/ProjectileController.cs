using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject levelController;
    private float topBoundary;

    [SerializeField] float speed;
    [SerializeField] float hitPower;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        levelController = GameObject.Find("Level Controller");
        topBoundary = GameObject.Find("Top Sensor").transform.position.z;   // can't use collision becuase neither have Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z >= topBoundary){    // check top position
            gameObject.SetActive(false);            // deactivate if too far up
        }
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime)); // move up
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Brick")){            
            Brick brick = other.gameObject.GetComponent<BrickController>().brick;
            if(brick.IsDestroyed(hitPower)){                        // check if brick is destructable and hits left is 0
                gameManager.UpdateScore(brick.scoreValue);          // update the score
                if(brick.hasPowerUp){
                    Instantiate(brick.powerup, other.gameObject.transform.position, brick.powerup.transform.rotation);  // create powerup
                }
                Destroy(other.gameObject);                          // destroy brick
                levelController.GetComponent<LevelController>().destructableBrickCount--;   // decrease brick count
            }
            gameObject.SetActive(false);                            // deactivate projectile
        }
    }
}
