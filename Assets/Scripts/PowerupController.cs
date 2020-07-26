using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public int playerId;
    public float speed;
    public float duration;
    public bool isActive = false;
    private float lowerBound;
    private bool canMove = true;
    public GameObject powerupIndicator;
    private GameObject activeIndicator;
    public GameObject player;           // assigned in BrickController
    public GameObject levelController;  // assigned in BrickController
    
    protected void Awake() {
        lowerBound = GameObject.Find("Bottom Sensor").transform.position.z; // get limit of motion
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));    // drift down
            if(transform.position.z < lowerBound){                  // destroy if past play field
                Destroy(gameObject);
            }
        }
    }

    public void ActivateEffect(){
        // this block elimnates previous powerups and indicators to switch to new one
        GameObject[] powerupsList = GameObject.FindGameObjectsWithTag("Powerup");           // get active powerups
        if(powerupsList != null){                                                           // none then skip
            for(int i = 0; i < powerupsList.Length; i++){                                   // iterate through list
                if(powerupsList[i].GetComponent<PowerupController>().isActive && powerupsList[i].GetComponent<PowerupController>().playerId == playerId){             // if active,
                    powerupsList[i].GetComponent<PowerupController>().EndPowerup();         // get rid of it
                }
            }
        }

        player.GetComponent<PlayerController>().hasPowerup = true;                          // player has powerup
        isActive = true;                                                                    // powerup is active
        activeIndicator = Instantiate(powerupIndicator, player.transform.position, powerupIndicator.transform.rotation);  // get indicator
        activeIndicator.GetComponent<PowerupIndicatorController>().player = player;
        StartEffect();                                                                      // start powerup effect
        StartCoroutine(PowerupCountDownRoutine());                                          // start timer for powerup
        GetComponent<MeshRenderer>().enabled = false;                                       // make powerup object invisible
        GetComponent<CapsuleCollider>().enabled = false;                                    // remove triggering
        canMove = false;                                                                    // stopp motion - last lines are to keep object while active
    }

    // limit powerup activity
    IEnumerator PowerupCountDownRoutine(){
        yield return new WaitForSeconds(duration);
        EndPowerup();
    }

    // stop effect of powerup
    public void EndPowerup(){
        player.GetComponent<PlayerController>().hasPowerup = false;
        EndEffect();
        Destroy(activeIndicator);
        Destroy(gameObject);       
    }

    public virtual void StartEffect(){
        Debug.Log("running generic start");
    }

    public virtual void EndEffect(){
        Debug.Log("running generic end");
    }

    private void OnDestroy() {
        Debug.Log("destroying!");    
    }
}
