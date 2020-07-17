using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupIndicatorController : MonoBehaviour
{
    private GameObject player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

}
