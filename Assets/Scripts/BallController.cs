using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10.0f;
    public float boundaryZ = 15;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z >= boundaryZ){
            changeLookForward(new Vector3(0, 0, -1));
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision other) {
        
        ContactPoint hitAngle = other.contacts[0];       // get point of contact with other object
        Debug.Log("hitAngle: " + hitAngle.normal);
        changeLookForward(hitAngle.normal);
        Debug.Log("forward: " + transform.forward);
    }

    private void changeLookForward(Vector3 barrier){
        Vector3 newAngle = Vector3.Reflect(transform.forward.normalized, barrier.normalized); // get angle of reflection
        transform.forward = new Vector3(newAngle.x, 0, newAngle.z); // assign foward to angle of relection
    }
}
