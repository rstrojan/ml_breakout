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
    public TextMeshProUGUI timerText;
    public GameObject timerTextObject;
    int timer = 3;
    float savedTimesScale;
    float startime;

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

    //next level menu objects
    public GameObject levelCompleteMenuObject;
    public Button endRunButton;
    public Button nextLevelButton;

    //game over menu objects
    public GameObject gameOverMenuObject;
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

            //Setting up for countdown
            savedTimesScale = Time.timeScale; //grab current timescale
            timerText.gameObject.SetActive(true); //turn on the ui
            isCountingDown = true; //make sure it starts
            startime = Time.unscaledTime; //grab current time
            Time.timeScale = 0;



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




    // Update is called once per frame
    void Update()
    {
        //level complete block
        if (GameManager.isLevelComplete)
        {
            levelCompleteMenuObject.gameObject.SetActive(true);
        }
        else
        {
            levelCompleteMenuObject.gameObject.SetActive(false);
        }


        //countdown block
        if (isCountingDown)
        {
            if ((Time.unscaledTime - startime) < 1.0f)
            {
                timerText.text = "3";
            }
            else if ((Time.unscaledTime - startime) < 2.0f)
            {
                timerText.text = "2";
            }
            else if ((Time.unscaledTime - startime) < 3.0f)
            {
                timerText.text = "1";
            }
            else if ((Time.unscaledTime - startime) < 4.0f)
            {
                timerText.text = "GO!";
                Time.timeScale = savedTimesScale;
            }
            else if ((Time.unscaledTime - startime) < 5.0f)
            {
                isCountingDown = false;
                timerText.gameObject.SetActive(false);
            }
        }


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
            gameOverMenuObject.gameObject.SetActive(true);
            GameOver();
        }
        else
        {
            gameOverMenuObject.gameObject.SetActive(false);
        }

        // toggle main menu 
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuObject.gameObject.SetActive(true);
            gameOverMenuObject.gameObject.SetActive(false); // make sure game over turns off
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
        Time.timeScale = 0;
        gameManager.LoadScore(); // make sure we have high score dat
        highScoreObject.gameObject.SetActive(true);
        if (gameManager.highScore < GameManager.scorePlayerOne)
        {
            newHighScoreObject.text = "New High Score: " + GameManager.scorePlayerOne;
            highScoreText.text = "Old High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
            newHighScoreObject.gameObject.SetActive(true);
        }
        else
        {
            highScoreText.text = "High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
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
        Debug.Log(newScoreName.text);
        gameManager.SaveScore(newScoreName.text);

        //if the newHighScoreObject is on, turn it off
        if (newHighScoreObject)
        {
            newHighScoreObject.gameObject.SetActive(false);
        }

        highScoreText.text = "High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;


    }



}
