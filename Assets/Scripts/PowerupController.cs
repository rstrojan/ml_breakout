using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public float speed;
    public float duration;
    public bool isActive = false;
    public GameObject powerupIndicator;
    private float lowerBound;
    private bool canMove = true;
    
    private void Awake() {
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
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("PowerupIndicator");    // get active indicators
        if(powerupsList != null){                                                           // none then skip
            for(int i = 0; i < powerupsList.Length; i++){                                   // iterate through list
                if(powerupsList[i].GetComponent<PowerupController>().isActive){             // if active,
                    powerupsList[i].GetComponent<PowerupController>().EndPowerup();         // get rid of it
                }
            }
        }
        if(indicators != null){                                                             // none then skip
            for(int i = 0; i < indicators.Length; i++){                                     // iterate through list
                indicators[i].GetComponent<PowerupIndicatorController>().EndEffect();       // get rid of indicators
            }
        }
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");                  // get player
        Player player = playerObj.GetComponent<PlayerController>().player;
        player.hasPowerup = true;                                                           // player has powerup
        isActive = true;                                                                    // powerup is active
        GameObject activeIndicator = Instantiate(powerupIndicator, playerObj.transform.position, powerupIndicator.transform.rotation);  // get indicator
        StartEffect();                                                                      // start powerup effect
        StartCoroutine(PowerupCountDownRoutine());                                          // start timer for powerup
        activeIndicator.GetComponent<PowerupIndicatorController>().StartEffect(duration);   // start indicator
        GetComponent<MeshRenderer>().enabled = false;                                       // make powerup object invisible
        GetComponent<CapsuleCollider>().isTrigger = false;                                  // remove triggering
        canMove = false;                                                                    // stopp motion - last lines are to keep object while active
    }

    // limit powerup activity
    IEnumerator PowerupCountDownRoutine(){
        Debug.Log("powerup start " + duration + " seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log("powerup timer end");
        EndPowerup();
    }

    // stop effect of powerup
    public void EndPowerup(){
        Debug.Log("end powerup");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player.hasPowerup = false;
        EndEffect();
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
