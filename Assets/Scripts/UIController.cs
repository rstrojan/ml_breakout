using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This object controls all of the menus and UIs.
// Most menu elements are children to a single menuObject
// that can easily be turned on/off depending on the conditions
// set by the GM.

public class UIController : MonoBehaviour
{
    public GameManager gameManager;

    //display strings for player scores
    public TextMeshProUGUI scoreTextPlayerOne;
    public TextMeshProUGUI scoreTextPlayerTwo;

    //objects for count down
    public bool isCountingDown;
    public float startTime;
    public float timer;
    public TextMeshProUGUI timerText;
    public GameObject timerTextObject;
    public float savedTimeScale;

    //main menu objects
    public GameObject mainMenuObject;
    public Button soloPlayButton;
    public Button duoPlayButton;
    public Button quitButton;
    public Button playerOneAIButton;
    public Button playerTwoAIButton;
    public TextMeshProUGUI mainMenuTitle;
    public TextMeshProUGUI highestScoreTitle;
    public GameObject highScoreListObject;
    public TextMeshProUGUI highScoreListTitle;
    public TextMeshProUGUI highScoreList;

    //game over menu objects
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highScoreObject;
    public TextMeshProUGUI highScoreListText;
    public TextMeshProUGUI newHighScoreObject;
    public InputField newScoreName;
    public InputField newScoreVal;

    //pause menu objects
    public GameObject pauseMenuObject;
    public TextMeshProUGUI pauseText;
    public Button restartLevelButton;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button gameStartButton;


    // Start is called before the first frame update
    void Start()
    {
        //Turns on score UI overlay while game is being played
        if (SceneManager.GetActiveScene().name != "MainMenu" && !GameManager.isGameOver)
        {
            //turn off main menu if not in main menu;
            mainMenuObject.gameObject.SetActive(false);

            //set up timer
            startTime = Time.time;
            isCountingDown = true;
            timer = 4.0f;
            timerText.gameObject.SetActive(true);
            timerText.alpha = 255;


            //check for players
            if (!GameManager.isTwoPlayer)
            {
                scoreTextPlayerOne.gameObject.SetActive(true);
            }
            else
            {
                scoreTextPlayerOne.gameObject.SetActive(true);
                scoreTextPlayerTwo.gameObject.SetActive(true);
            }
        }
        else
        {
            scoreTextPlayerOne.gameObject.SetActive(false);
            scoreTextPlayerTwo.gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        //inspiration for this timer found here: https://forum.unity.com/threads/start-timer-on-new-scene.487709/
        //countdown timer
        if (isCountingDown)
        {
            //get diff between curr time and startime
            float timediff = (Time.time - startTime);
            Debug.Log("counting down " + (Time.time - startTime));
            //while timer is greather than or equal to 0
            if (timer - timediff >= 0.0)
            {
                if (timer - timediff > 3.0f)
                {
                    timerText.text = "3";
                }
                else if (timer - timediff > 2.0f)
                {
                    timerText.text = "2";
                }
                else if (timer - timediff > 1.0f)
                {
                    timerText.text = "1";
                }
                else if (timer - timediff > 0.0f)
                {
                    timerText.text = "GO!";
                    timerText.alpha = 255 * timer; // fade alpha based on time
                }
            }
            else
            {
                isCountingDown = false;
                timerText.gameObject.SetActive(false);
            }

        }

    }
    // Update is called once per frame
    void Update()
    {


        // if game is paused, turn on pause menu
        if (GameManager.isPaused == true)
        {
            pauseMenuObject.gameObject.SetActive(true);
        }
        else
        {
            pauseMenuObject.gameObject.SetActive(false);
        }

        //update the scores
        if(GameManager.isPlaying == true)
        {
            UpdateScore();
        }

        // if game is over, call game over
        if (GameManager.isGameOver == true)
        {
            GameOver();
        }

        // toggle main menu 
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuObject.gameObject.SetActive(true);
            gameManager.LoadScore();
            highestScoreTitle.text = "#1 Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
            highScoreList.text = gameManager.highScoreListText;
            if (GameManager.playerOneIsAI)
            {
                playerOneAIButton.GetComponentInChildren<Text>().text = "P1 is AI";
            }
            else
            {
                playerOneAIButton.GetComponentInChildren<Text>().text = "P1 is Human";
            }
            if (GameManager.playerTwoIsAI)
            {
                playerTwoAIButton.GetComponentInChildren<Text>().text = "P2 is AI";
            }
            else
            {
                playerTwoAIButton.GetComponentInChildren<Text>().text = "P2 is Human";
            }
        }
        else
        {
            mainMenuObject.gameObject.SetActive(false);
        }

    }

    // manage gameover state
    public void GameOver()
    {
        gameManager.LoadScore(); // make sure we have high score dat
        highScoreObject.gameObject.SetActive(true);
        if (gameManager.highScore < GameManager.scorePlayerOne)
        {
            newHighScoreObject.text = "New High Score: " + GameManager.scorePlayerOne;
            highScoreText.text = "Old High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
            newHighScoreObject.gameObject.SetActive(true);
        }
        mainMenuButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    //update the in game score UI overlay.
    public void UpdateScore()
    { 
        scoreTextPlayerOne.text = "Score: " + GameManager.scorePlayerOne;
        scoreTextPlayerTwo.text = "Score: " + GameManager.scorePlayerTwo;
    }

    //button function for saving the score in gameover menu
    public void SaveButton()
    {
        gameManager.SaveScore(newScoreName.text);

        //if the newHighScoreObject is on, turn it off
        if (newHighScoreObject)
        {
            newHighScoreObject.gameObject.SetActive(false);
        }

        highScoreText.text = "High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;


    }



}
