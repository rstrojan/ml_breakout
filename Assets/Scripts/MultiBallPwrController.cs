using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBallPwrController : PowerupController
{
    [SerializeField] GameObject ballPrefab;
    private GameObject currentBall;
    private Vector3 currentBallPosition;
    private Vector3 currentBallVelocity;
    [SerializeField] int totalBallsToMake;
    [SerializeField] float angleVariation;
    private int ballsToMake = 0;
    private int ballsMade = 0;
    public static bool doBlock = false;

    public override void StartEffect()
    {
        if(doBlock){    // if 2 multiballs are gotten at the same time, it causes race conditions breaking the ball count
            return;
        }
        else{
            doBlock = true;
        }
        ballsToMake = totalBallsToMake - levelController.ballCount; // calculate number of balls to make
        FindBall();         // find a ball in play to disperse from
        currentBallPosition = currentBall.transform.position;   // get position of that ball to spawn from
        currentBallVelocity = currentBall.GetComponent<Rigidbody>().velocity;   // get velocity of that ball
        if(ballsToMake > 0){                    // if there are not already max balls in play
            StartCoroutine(MakeBallRoutine());  // make a ball
        }
    }

    public override void EndEffect(){
        doBlock = false;
    }

    private void FindBall(){
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(var ball in balls){
            if(ball.GetComponent<BallController>().playerId == playerId){
                currentBall = ball;
                break;
            }
        }
    }

    IEnumerator MakeBallRoutine(){
        yield return new WaitForSeconds(0.1f);  // wait for ball spawned from to get out of the way
        Vector3 newBallVelocity = Quaternion.AngleAxis(angleVariation * (ballsMade + 1), new Vector3(0, 1, 0)) * currentBallVelocity; // spread new balls out
        newBallVelocity.z = Mathf.Abs(newBallVelocity.z); // new balls go up
        GameObject newBall = Instantiate(ballPrefab, currentBallPosition, ballPrefab.transform.rotation, transform.parent);
        newBall.SetActive(true);
        newBall.GetComponent<Rigidbody>().velocity = newBallVelocity;
        levelController.ballCount++;
        ballsToMake--;
        ballsMade++;
        if(ballsToMake > 0){    // if there are more balls to make, make another one
            StartCoroutine(MakeBallRoutine());
        }
    }
    
}
