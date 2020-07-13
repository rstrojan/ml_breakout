using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highScoreNameText;
    public TextMeshProUGUI highScoreObject;
    public TextMeshProUGUI newHighScoreObject;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button mainMenuButton;
    public Button gameStartButton;
    public InputField newScoreName;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // manage gameover state
    public void GameOver()
    {
        LoadScore(); // make sure we have high score data
        highScoreObject.gameObject.SetActive(true);
        highScoreText.text = "High Score: " + highScoreName + " - " + highScore;
        if (highScore < score)
        {
            newHighScoreObject.gameObject.SetActive(true);
        }
        mainMenuButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level1");
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

    public void LoadScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreName = PlayerPrefs.GetString("HighScoreName", "A55");

    }
}
