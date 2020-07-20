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

    private bool isPaused;
    private float pausedTimeScale;
    private string highScoreName;
    private int highScore;
    private int score;
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
        LoadScore(); // make sure we have high score data
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
        
        if (highScore < score)
        {
            PlayerPrefs.SetString("HighScoreName", newScoreName.text);
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            newHighScoreObject.gameObject.SetActive(false);
        }

        // call loadScore again to update vars
        LoadScore();
    }

    public void ClearScore()
    {
        PlayerPrefs.DeleteAll();
        LoadScore();
    }

    public void LoadScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreName = PlayerPrefs.GetString("HighScoreName", "A55");
        highScoreText.text = "High Score: " + highScoreName + " - " + highScore;

    }

    public void TestSave()
    {
        SaveSerial saver = new SaveSerial();
        int testint = 1;
        string testname = "abc";
        saver.SaveGame(testname, testint);

    }

    public void TestLoad()
    {
        SaveSerial loader = new SaveSerial();
        loader.LoadGame();
        foreach (HighScoreEntry a in loader.HighScoreList)
        {
            Debug.Log(a.name + " - " + a.score);

        }
    }

}
