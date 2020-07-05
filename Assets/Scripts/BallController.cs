using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameManager gameManager;
    public float speed = 10.0f;
    public float boundaryZ = 15;
    private bool checkZ = true;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z >= boundaryZ && checkZ){
            changeLookForward(new Vector3(0, 0, -1));
            checkZ = false;
            StartCoroutine(CheckZWait());
        }
        if(transform.position.z < -15){
            Destroy(gameObject);
            Debug.Log("Game Over");
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    IEnumerator CheckZWait(){
        yield return new WaitForSeconds(0.5f);
        checkZ = true;
    }

    private void OnCollisionEnter(Collision other) {
        
        ContactPoint hitAngle = other.contacts[0];       // get point of contact with other object
        Debug.Log("hitAngle: " + hitAngle.normal);
        changeLookForward(hitAngle.normal);
        Debug.Log("forward: " + transform.forward);
        if(other.gameObject.CompareTag("Brick")){
            // update the score
            gameManager.UpdateScore(other.gameObject.GetComponent<BrickController>().scoreValue);
            Destroy(other.gameObject);
            
        }
    }

    private void changeLookForward(Vector3 barrier){
        Vector3 newAngle = Vector3.Reflect(transform.forward.normalized, barrier.normalized); // get angle of reflection
        transform.forward = new Vector3(newAngle.x, 0, newAngle.z); // assign foward to angle of relection
    }
}
