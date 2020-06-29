using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 15.0f;
    public float xRange = 15.0f;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Limit player motion from left to right
        if(transform.position.x < -xRange){
            transform.position = new Vector3(-xRange, startPos.y, startPos.z);
        }
        if(transform.position.x > xRange){
            transform.position = new Vector3(xRange, startPos.y, startPos.z);
        }

        // get player input to move left or right
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
        
    }
}
