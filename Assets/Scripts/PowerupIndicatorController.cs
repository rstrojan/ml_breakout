using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupIndicatorController : MonoBehaviour
{
    public GameObject player;   // assinged in PowerupController

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

}
