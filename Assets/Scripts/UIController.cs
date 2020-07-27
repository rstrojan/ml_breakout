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

    //main menu objects
    public GameObject mainMenuObject;
    public Button soloPlayButton;
    public Button duoPlayButton;
    public Button quitButton;
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
        Debug.Log("score loaded");
        highScoreObject.gameObject.SetActive(true);
        if (gameManager.highScore < gameManager.scorePlayerOne)
        {
            newHighScoreObject.text = "New High Score: " + gameManager.scorePlayerOne;
            highScoreText.text = "Old High Score: " + gameManager.highScoreName + " - " + gameManager.highScore;
            newHighScoreObject.gameObject.SetActive(true);
        }
        mainMenuButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    //update the in game score UI overlay.
    public void UpdateScore()
    { 
        scoreTextPlayerOne.text = "Score: " + gameManager.scorePlayerOne;
        scoreTextPlayerTwo.text = "Score: " + gameManager.scorePlayerTwo;
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
