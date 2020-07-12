using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public Powerup powerup;
    [SerializeField] float lowerBound;
    
    private void Awake() {
        lowerBound = GameObject.Find("Bottom Sensor").transform.position.z; // get limit of motion
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, -powerup.speed * Time.deltaTime));    // drift down
        if(transform.position.z < lowerBound){                  // destroy if past play field
            Destroy(gameObject);
        }
    }
}
