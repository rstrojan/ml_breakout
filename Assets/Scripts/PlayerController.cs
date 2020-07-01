using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 20.0f;
    private float xRange = 20.0f;
    private Vector3 startPos;

    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        startPos = transform.position;
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
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
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
}
