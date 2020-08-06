using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBallPwrController : PowerupController
{
    [SerializeField] GameObject ballPrefab;
    private GameObject currentBall;
    private Vector3 currentBallPosition;
    private Vector3 currentBallVelocity;
    public int ballCount;
    [SerializeField] int ballsToMake;
    [SerializeField] float angleVariation;


    public override void StartEffect(){
        ballCount = levelController.GetComponent<LevelController>().ballCount;
        FindBall();
        currentBallPosition = currentBall.transform.position;
        currentBallVelocity = currentBall.GetComponent<Rigidbody>().velocity;
        for(int i = 0; i < (ballsToMake - ballCount); i++){
            // Vector3 newBallVelocity = new Vector3((currentBallVelocity.x + angleVariation * (i + 1)), currentBallVelocity.y, currentBallVelocity.z);
            Vector3 newBallVelocity = Quaternion.AngleAxis(angleVariation * (i + 1), new Vector3(0, 1, 0)) * currentBallVelocity;
            GameObject newBall = Instantiate(ballPrefab, currentBallPosition, ballPrefab.transform.rotation);
            newBall.GetComponent<Rigidbody>().velocity = newBallVelocity;
            newBall.GetComponent<BallController>().playerId = playerId;
            newBall.GetComponent<BallController>().player = player;
            newBall.GetComponent<BallController>().levelController = levelController;
        }
        levelController.GetComponent<LevelController>().ballCount += (ballsToMake - ballCount);
    }

    public override void EndEffect(){
        return;
    }

    private void FindBall(){
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (var ball in balls){
            if(ball.GetComponent<BallController>().playerId == playerId){
                currentBall = ball;
                break;
            }
        }
    }
}
