using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject uiController;

    public SFXController sfx;

    //Game state bools
    public static bool isPaused;
    public static bool isPlaying;
    public static bool isNextLevel; //tracks whether player has moved passed level 1
    public static bool isLevelComplete; //tracks whether a level has been completed
    public static bool isGameOver;
    public static bool isTwoPlayer;
    public static bool playerOneIsAI;
    public static bool playerTwoIsAI;

    //for pause menu
    public float pausedTimeScale;

    //High score data
    public string highScoreName;
    public int highScore;
    public int lowScore;
    public string highScoreListText;

    // static objects used for carrying over data to next level
    public static int scorePlayerOne;
    public static int scorePlayerTwo;
    public static int levelTracker;

    //these objects are for testing purposes.
    public InputField TEST_newScoreName;
    public InputField TEST_newScoreVal;


    // Start is called before the first frame update
    void Start()
    {
        //get sfx controller
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();

        //for testing
        Debug.Log("Now Playing Level " + levelTracker);
        // get high score
        LoadScore();

        if(!isNextLevel) //if a new game, 0 out static vars
        {
            // init scores in case theres no save data
            scorePlayerOne = 0;
            scorePlayerTwo = 0;
            levelTracker = 1;
        }

        //certain states need to be reset at start of level
        isPaused = false;
        isLevelComplete = false;
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
                sfx.PlayPause(0); //0 = Pause sound
                isPaused = true; //flip the bool
                pausedTimeScale = Time.timeScale; //capture current timescale
                Time.timeScale = 0; //set timescale to 0
            }
            else
            {
                sfx.PlayPause(1); //1 = unpause sound
                isPaused = false; //flip the bool
                Time.timeScale = pausedTimeScale; //set the timescale to what it was before            
            }
            
        }
    }

       
    // Set isLevelComplete
    public void LevelComplete(){
        isLevelComplete = true;
        Time.timeScale = 0;
        // Debug.Log("Level Complete!");
    }

    // Go to isNextLevel
    public void NextLevel()
    {
        sfx.PlayButtonClick();
        isGameOver = false;
        levelTracker++;
        isNextLevel = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    // Set isGameOver to true.
    public void GameOver()
    {
        isGameOver = true;
        isLevelComplete = false;
        Time.timeScale = 0;
        // Debug.Log("Game Over");
    }

    //toggle whether p1 is AI or not
    public void PlayerOneIsAI()
    {
        sfx.PlayButtonClick();
        playerOneIsAI = !playerOneIsAI;
    }

    //toggle whether p2 is AI or not
    public void PlayerTwoIsAI()
    {
        sfx.PlayButtonClick();
        playerTwoIsAI = !playerTwoIsAI;
    }

    // load mainmenu scene, make sure timescale is set to 1, isGameover to false
    public void ToMainMenu()
    {
        sfx.PlayButtonClick();
        Time.timeScale = 1;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");

    }

    // start game from main menu
    public void StartGame()
    {
        sfx.PlayButtonClick();

        //load the scene
        SceneManager.LoadSceneAsync("Level1");
        Time.timeScale = 1;
        //set game states for this type of load
        isNextLevel = false;
        isGameOver = false;
        isLevelComplete = false;
    }

    // restart current scene, make sure timescale is set to 1, isGameover to false
    public void Restart()
    {
        sfx.PlayButtonClick();

        // get current scene and reload it.
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        isGameOver = false;
    }

    public void SaveScore(string scoreName)
    {
        sfx.PlayButtonClick();

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
        sfx.PlayButtonClick();

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
        lowScore = loader.HighScoreList[loader.HighScoreList.Count-1].score;

        // Debug.Log(scoreList);
    }

    //exit game to desktop
    public void ExitGame()
    {
        sfx.PlayButtonClick();

        Application.Quit();
    }
    
    //set isTwoPlayer to true, and starts game
    public void SetTwoPlayer(){
        isTwoPlayer = true;
        StartGame();
    }
}
