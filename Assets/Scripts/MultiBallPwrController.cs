using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBallPwrController : PowerupController
{
    [SerializeField] GameObject ballPrefab;
    private GameObject currentBall;
    private Vector3 currentBallPosition;
    private Vector3 currentBallVelocity;
    private int ballCount;
    [SerializeField] int ballsToMake;
    [SerializeField] float angleVariation;

    public override void StartEffect(){
        ballCount = GameObject.Find("Level Controller").GetComponent<LevelController>().currentBallCount;
        currentBall = GameObject.FindGameObjectWithTag("Ball");
        currentBallPosition = currentBall.transform.position;
        currentBallVelocity = currentBall.GetComponent<Rigidbody>().velocity;
        for(int i = 0; i < (ballsToMake - ballCount); i++){
            Vector3 newBallVelocity = new Vector3((currentBallVelocity.x + angleVariation * (i + 1)), currentBallVelocity.y, currentBallVelocity.z);
            GameObject newBall = Instantiate(ballPrefab, currentBallPosition, ballPrefab.transform.rotation);
            newBall.GetComponent<Rigidbody>().velocity = newBallVelocity;
        }
        GameObject.Find("Level Controller").GetComponent<LevelController>().currentBallCount += (ballsToMake - ballCount);
    }

    public override void EndEffect(){
        return;
    }
}
