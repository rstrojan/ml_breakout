using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public Powerup powerup;
    private float lowerBound;
    private bool canMove = true;
    
    private void Awake() {
        lowerBound = GameObject.Find("Bottom Sensor").transform.position.z; // get limit of motion
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            transform.Translate(new Vector3(0, 0, -powerup.speed * Time.deltaTime));    // drift down
            if(transform.position.z < lowerBound){                  // destroy if past play field
                Destroy(gameObject);
            }
        }
    }

    public void ActivateEffect(){
        GameObject[] powerupsList = GameObject.FindGameObjectsWithTag("Powerup");
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("PowerupIndicator");
        if(powerupsList != null){
            for(int i = 0; i < powerupsList.Length; i++){
                if(powerupsList[i].GetComponent<PowerupController>().powerup.isActive){
                    powerupsList[i].GetComponent<PowerupController>().powerup.EndEffect();
                    Destroy(powerupsList[i]);
                }
            }
        }
        if(indicators != null){
            for(int i = 0; i < indicators.Length; i++){
                indicators[i].GetComponent<PowerupIndicatorController>().EndEffect();
            }
        }
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Player player = playerObj.GetComponent<PlayerController>().player;
        player.hasPowerup = true;
        powerup.StartEffect();
        powerup.isActive = true;
        GameObject activeIndicator = Instantiate(powerup.powerupIndicator, playerObj.transform.position, powerup.powerupIndicator.transform.rotation);
        activeIndicator.GetComponent<PowerupIndicatorController>().StartEffect(powerup.duration);
        canMove = false;
        StartCoroutine(PowerupCountDownRoutine(player));
        GetComponent<MeshRenderer>().enabled = false;
    }

    // limit powerup activity
    IEnumerator PowerupCountDownRoutine(Player player){
        yield return new WaitForSeconds(5);
        EndPowerup(player);
    }

    // stop effect of powerup
    public void EndPowerup(Player player){
        Debug.Log("end powerup");
        player.hasPowerup = false;
        powerup.EndEffect();
        // powerup.activeIndicator.GetComponent<PowerupIndicatorController>().EndEffect();
        Destroy(gameObject);        
    }
}
