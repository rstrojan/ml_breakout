using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject uiController;
    public static bool isPaused;
    public static bool isPlaying;
    public static bool isGameOver;
    public static bool isTwoPlayer;

    public float pausedTimeScale;
    public string highScoreName;
    public int highScore;
    public string highScoreListText;
    public int scorePlayerOne;
    public int scorePlayerTwo;

    //these objects are for testing purposes.
    public InputField TEST_newScoreName;
    public InputField TEST_newScoreVal;


    // Start is called before the first frame update
    void Start()
    {
        // init scores in case theres no save data
        scorePlayerOne = 0;
        scorePlayerTwo = 0;
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

        if (!isPaused && !isGameOver && (SceneManager.GetActiveScene().name != "MainMenu"))
        {
            isPlaying = true;
        }
        else
        {
            isPlaying = false;
        }
    }

    public void UpdateScore(int scoreToAdd, int playerId)
    {
        if (playerId == 1)
        {
            scorePlayerOne += scoreToAdd;
        }
        else
        {
            scorePlayerTwo += scoreToAdd;
        }
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
            }
            else
            {
                isPaused = false; //flip the bool
                Time.timeScale = pausedTimeScale; //set the timescale to what it was before            
            }
            
        }
        GameOver();
    }

       
    // MAKE ME DO STUFF!
    public void LevelComplete(){
        Debug.Log("Level Complete!");
    }

    // Set isGameOver to true.
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over");
    }

    // load mainmenu scene, make sure timescale is set to 1, isGameover to false
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        isGameOver = false;
    }

    // load level1 scene, make sure timescale is set to 1, isGameover to false
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level1");
        Time.timeScale = 1;
        isGameOver = false;
    }

    // restart current scene, make sure timescale is set to 1, isGameover to false
    public void Restart()
    {
        // get current scene and reload it.
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        isGameOver = false;
    }

    public void SaveScore(string scoreName)
    {
        
        SaveSerial saver = new SaveSerial();
        int scoreInt = scorePlayerOne;

        //THIS IF BLOCK IS FOR TESTING SCORE SAVING 
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            scoreInt = int.Parse(TEST_newScoreVal.text);
            scoreName = TEST_newScoreName.text;
        }

        saver.SaveGame(scoreName, scoreInt);

        // call loadScore again to update vars
        LoadScore();
    }

    // delete the game file and clear all current vars
    public void ClearScore()
    {
        PlayerPrefs.DeleteAll();

        SaveSerial clearer = new SaveSerial();
        clearer.ClearData();
        LoadScore(); // update info on screen
        
    }

    //open game file and load all vars
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
            highScoreListText = scoreList;
        }
        highScore = loader.HighScoreList[0].score;
        highScoreName = loader.HighScoreList[0].name;

        Debug.Log(scoreList);
    }

    //exit game to desktop
    public void ExitGame()
    {
        Application.Quit();
    }
    
    //set isTwoPlayer to true
    public void SetTwoPlayer(){
        isTwoPlayer = true;
        StartGame();
    }
}
