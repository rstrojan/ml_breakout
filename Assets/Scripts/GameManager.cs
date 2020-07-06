using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button mainMenuButton;
    public Button gameStartButton;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        // set score to 0
        score = 0;
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
        mainMenuButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("BreakoutScene");
    }
}
