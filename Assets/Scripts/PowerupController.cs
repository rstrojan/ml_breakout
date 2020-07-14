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
        GameObject[] powerupsList = GameObject.FindGameObjectsWithTag("Powerup");
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("PowerupIndicator");
        if(powerupsList != null){
            for(int i = 0; i < powerupsList.Length; i++){
                if(powerupsList[i].GetComponent<PowerupController>().isActive){
                    powerupsList[i].GetComponent<PowerupController>().EndPowerup();
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
        isActive = true;
        GameObject activeIndicator = Instantiate(powerupIndicator, playerObj.transform.position, powerupIndicator.transform.rotation);
        StartEffect();
        StartCoroutine(PowerupCountDownRoutine());
        activeIndicator.GetComponent<PowerupIndicatorController>().StartEffect(duration);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().isTrigger = false;
        canMove = false;        
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
