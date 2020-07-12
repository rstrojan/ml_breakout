using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;               // input from player
    private float xRange;                       // limit left right movement
    private Vector3 startPos;                   // to save start position

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        xRange = GetFloorRange();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ConstrainPlayer();        
    }

    // get player input to move left or right
    private void MovePlayer(){
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * player.speed);
    }

    // Limit player motion from left to right
    private void ConstrainPlayer(){
        if(transform.position.x < -xRange){
            transform.position = new Vector3(-xRange, startPos.y, startPos.z);
        }
        if(transform.position.x > xRange){
            transform.position = new Vector3(xRange, startPos.y, startPos.z);
        }
    }

    // return size of current level floor minus player size
    private float GetFloorRange(){
        return (GameObject.Find("Ground").GetComponent<MeshRenderer>().bounds.size.x / 2f) - (gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2f);
    }

    // catch a power up
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Powerup")){
            player.hasPowerup = true;
            GameObject powerupIndicator = other.GetComponent<PowerupController>().powerup.powerupIndicator;
            GameObject powInd = Instantiate(powerupIndicator, transform.position, powerupIndicator.transform.rotation);
            StartCoroutine(PowerupCountDownRoutine(powInd));
            Destroy(other.gameObject);
        }
    }

    // limit powerup activity
    IEnumerator PowerupCountDownRoutine(GameObject powerupIndicator){
        yield return new WaitForSeconds(5);
        player.hasPowerup = false;
        Destroy(powerupIndicator);
    }
}
