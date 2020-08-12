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

    //audio objects for countdown
    public SFXController sfx;
    public bool threeWasPlayed;
    public bool twoWasPlayed;
    public bool oneWasPlayed;
    public bool goWasPlayed;

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
    public bool hasBeenSaved;

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
        //get the SFX controller object
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();

        //bool to track if score has been saved
        hasBeenSaved = false;

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
            threeWasPlayed = twoWasPlayed = oneWasPlayed = goWasPlayed = false;


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
                if(!threeWasPlayed)
                {
                    sfx.PlayCountDown(3);
                    threeWasPlayed = true;
                }

            }
            else if ((Time.unscaledTime - startime) < 2.0f)
            {
                timerText.text = "2";
                if (!twoWasPlayed)
                {
                    sfx.PlayCountDown(2);
                    twoWasPlayed = true;
                }
            }
            else if ((Time.unscaledTime - startime) < 3.0f)
            {
                timerText.text = "1";
                if (!oneWasPlayed)
                {
                    sfx.PlayCountDown(1);
                    oneWasPlayed = true;
                }

            }
            else if ((Time.unscaledTime - startime) < 4.0f)
            {
                timerText.text = "GO!";
                if (!goWasPlayed)
                {
                    sfx.PlayCountDown(0);
                    goWasPlayed = true;
                }
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
        Time.timeScale = 0; //pauses the game
        gameManager.LoadScore(); // make sure we have high score data
        highScoreObject.gameObject.SetActive(true);
        if (!GameManager.isTwoPlayer)
        {
            //if the score is in the top 10 and has not already been saved
            if (gameManager.lowScore < GameManager.scorePlayerOne && !hasBeenSaved)
            {
                newHighScoreObject.text = "New High Score: " + GameManager.scorePlayerOne;
                highScoreText.text = "Old High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
                newHighScoreObject.gameObject.SetActive(true);
            }
            else
            {
                newHighScoreObject.gameObject.SetActive(false);
                highScoreText.text = "High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
            }
        }
        else
        {
            //grab the controllers so we can check ball counts
            LevelController p1 = GameObject.Find("Level Controller").GetComponent<LevelController>();
            LevelController p2 = GameObject.Find("Level Controller 2P").GetComponent<LevelController>();
            //first check ball counts as that is higher priority than score
            if (p1.ballCount == 0)
            {
                highScoreText.text = "Player Two Wins!";
            }
            else if (p2.ballCount == 0)
            {
                highScoreText.text = "Player One Wins!";
            }
            //then check scores for winner
            else if (GameManager.scorePlayerOne > GameManager.scorePlayerTwo)
            {
                highScoreText.text = "Player One Wins!";
            }
            else if (GameManager.scorePlayerOne < GameManager.scorePlayerTwo)
            {
                highScoreText.text = "Player Two Wins!";
            }
            else
            {
                highScoreText.text = "Wow, I can't believe you actually tied.";
            }
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
        hasBeenSaved = true; //mark that score has been saved
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
