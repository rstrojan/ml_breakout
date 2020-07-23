using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highScoreObject;
    public TextMeshProUGUI highScoreListText;
    public TextMeshProUGUI newHighScoreObject;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject pauseMenuObject;
    public TextMeshProUGUI pauseText;
    public Button restartLevelButton;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button gameStartButton;
    public InputField newScoreName;
    public InputField newScoreVal;

    private bool isPaused;
    private float pausedTimeScale;
    private string highScoreName;
    private int highScore;
    private int score;
    public bool isTwoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // init scores in case theres no save data
        score = 0;
        // get high score
        LoadScore();

        //set pause
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        // when hit escape toggle pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void TogglePause()
    {
        //if not in the main menu, the proceed with togglepause
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            //check the bool
            if (isPaused == false)
            {
                isPaused = true; //flip the bool
                pausedTimeScale = Time.timeScale; //capture current timescale
                Time.timeScale = 0; //set timescale to 0
                pauseMenuObject.gameObject.SetActive(true); //show pause menu

            }
            else
            {
                isPaused = false; //flip the bool
                Time.timeScale = pausedTimeScale; //set the timescale to what it was before
            }
            
        }
    }


    // manage gameover state
    public void GameOver()
    {
        LoadScore(); // make sure we have high score dat
        Debug.Log("score loaded");
        highScoreObject.gameObject.SetActive(true);
        if (highScore < score)
        {
            newHighScoreObject.text = "New High Score: " + score;
            highScoreText.text = "Old High Score: " + highScoreName + " - " + highScore;
            newHighScoreObject.gameObject.SetActive(true);
        }
        mainMenuButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    // MAKE ME DO STUFF!
    public void LevelComplete(){
        Debug.Log("Level Complete!");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level1");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        // get current scene and reload it.
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void SaveScore()
    {
        
        SaveSerial saver = new SaveSerial();
        int scoreInt = score;
        string scoreName = newScoreName.text;

        //THIS IF BLOCK IS FOR TESTING SCORE SAVING 
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            scoreInt = int.Parse(newScoreVal.text);
            scoreName = newScoreName.text;
        }

        saver.SaveGame(scoreName, scoreInt);

        if (newHighScoreObject)
        {
            newHighScoreObject.gameObject.SetActive(false);
        }

        // call loadScore again to update vars
        LoadScore();
    }

    public void ClearScore()
    {
        PlayerPrefs.DeleteAll();

        SaveSerial clearer = new SaveSerial();
        clearer.ClearData();
        LoadScore(); // update info on screen
        
    }

    public void LoadScore()
    {
        SaveSerial loader = new SaveSerial();
        loader.LoadGame();
        string scoreList = "";
        foreach (HighScoreEntry a in loader.HighScoreList)
        {
            scoreList += a.name + " - " + a.score + '\n';
        }

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            highScoreListText.text = scoreList;
        }
        highScore = loader.HighScoreList[0].score;
        highScoreName = loader.HighScoreList[0].name;
        highScoreText.text = "High Score: " + highScoreName + " - " + highScore;


        Debug.Log(scoreList);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
