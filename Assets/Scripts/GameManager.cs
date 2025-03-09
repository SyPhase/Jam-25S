using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Variables
    [SerializeField] int score = 0;
    [SerializeField] TMP_Text timeCount;
    [SerializeField] TMP_Text scoreCount;
    [SerializeField] TMP_Text highscoreCount;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject newHighscore;

    const string _highscore = "highscore";
    const float _levelTime = 20f;
    float timeRemaining = 0f;

    // Singleton Setup
    public static GameManager Instance;

    private void Awake()
    {
        // Singleton Setup
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        // Singleton Persists through Scene changes
        DontDestroyOnLoad(gameObject);

        // Update UI
        UpdateScoreUI();
    }

    /// <summary>
    /// Does Game Over logic and brings us back to Main Menu
    /// </summary>
    public void LevelFailed()
    {
        // Try to set new highscore (only works if higher)
        bool isNewHighscore = SetHighscore(score);

        // Activate Game Over UI
        gameOver.SetActive(true);
        if (isNewHighscore)
        {
            newHighscore.SetActive(true);
        }

        // Wait five seconds
        StartCoroutine(WaitSeconds(5));

        // Deactivate Game Over UI
        newHighscore.SetActive(false);
        gameOver.SetActive(false);

        // Return to Menu
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Waits for number of seconds inputted
    /// </summary>
    /// <param name="seconds">seconds to wait</param>
    /// <returns></returns>
    IEnumerator WaitSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// Adds points to game score
    /// </summary>
    /// <param name="points">Points to add to score</param>
    public void AddPoints(int points)
    {
        score += points;
    }

    /// <summary>
    /// Gets the highscore from PlayerPrefs
    /// </summary>
    /// <returns>the highscore</returns>
    public int GetHighscore()
    {
        // Gets highscore or zero if no highscore saved
        return PlayerPrefs.GetInt(_highscore, 0);
    }

    /// <summary>
    /// Sets new highscore if new highscore is greater than old highscore.
    /// </summary>
    /// <param name="newHighscore">new highscore number to set highscore to</param>
    /// <returns>true if new highscore is set, else false</returns>
    public bool SetHighscore(int newHighscore)
    {
        int oldHighscore = GetHighscore();

        if (newHighscore > oldHighscore)
        {
            // Saves new highscore
            PlayerPrefs.SetInt(_highscore, newHighscore);

            // Update UI
            UpdateScoreUI();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates both the scores UI
    /// </summary>
    void UpdateScoreUI()
    {
        highscoreCount.text = GetHighscore().ToString();
        scoreCount.text = score.ToString();
    }
}