using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float topBoundary;

    [SerializeField] float speed;
    public float hitPower;

    // Start is called before the first frame update
    void Start()
    {
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
    
}
