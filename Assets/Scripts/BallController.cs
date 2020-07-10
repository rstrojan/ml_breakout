using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameManager gameManager;
    public float speed = 10.0f;
    private Rigidbody ballRb;

    [SerializeField] Vector3 startVelocity;
    private Vector3 lastUpdateVelocity;

    int hitCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ballRb = gameObject.GetComponent<Rigidbody>();
        ballRb.velocity = startVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        // maintain constant speed
        lastUpdateVelocity = ballRb.velocity;
    }

    private void OnCollisionEnter(Collision other) {
        
        Debug.Log("hitCount: " + hitCount);
        hitCount++;
        ContactPoint hitAngle = other.contacts[0];          // get point of contact with other object
        ReflectBounce(hitAngle.normal);                 // change forward direction
        if (other.gameObject.CompareTag("Brick"))
        {
            // update the score
            gameManager.UpdateScore(other.gameObject.GetComponent<BrickController>().scoreValue);
            Destroy(other.gameObject);                      // destroy brick
        }
        if(other.gameObject.CompareTag("Bottom Sensor")){
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    // change forward direction of ball based on collision
    private void ReflectBounce(Vector3 barrierNormal){
        // Debug.Log("Velocity: " + ballRb.velocity);
        Vector3 newAngle = Vector3.Reflect(lastUpdateVelocity.normalized, barrierNormal); // get angle of reflection
        // Debug.Log("newAngle: " + newAngle);
        ballRb.velocity = newAngle * speed;
    }
}
