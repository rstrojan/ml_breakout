using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupIndicatorController : MonoBehaviour
{
    public int playerId;
    public GameObject player;   // assinged in PowerupController

    private void Awake() {
        playerId = transform.parent.gameObject.GetComponent<LevelController>().playerId;
        player = transform.parent.gameObject.GetComponent<LevelController>().player;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position = player.transform.position;
    }

}
