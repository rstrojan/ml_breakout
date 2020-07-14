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

    public void StartEffect(float duration){
        StartCoroutine(IndicatorCountDownRoutine(duration));
    }

    IEnumerator IndicatorCountDownRoutine(float duration){
        Debug.Log("indicator duration: " + duration);
        yield return new WaitForSeconds(duration);
        Debug.Log("end indicator");
        EndEffect();
    }

    public void EndEffect(){
        Destroy(gameObject);
    }
}
