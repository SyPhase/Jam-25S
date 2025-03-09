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
    [SerializeField] TMP_Text countdownText;

    const string _highscore = "highscore";
    const float _levelTime = 30f;
    float timeRemaining = 0f;

    bool isLevelStarted = false;
    Ship currentShip;

    // Singleton Setup
    public static GameManager Instance;

    void Awake()
    {
        // Singleton Setup
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        //Debug.Log("Instance is set!");

        // Singleton Persists through Scene changes
        DontDestroyOnLoad(gameObject);

        // Update UI
        UpdateScoreUI();
    }

    void Update()
    {
        // if level is NOT started
        if (!isLevelStarted) { return; }

        // if ran out of time
        if (timeRemaining < 0f) { LevelFailed(); }

        // Subtract time passed
        timeRemaining -= Time.deltaTime;

        // Update Timer UI
        timeCount.text = ((int)timeRemaining).ToString();
    }

    /// <summary>
    /// Tries to start the level, activates spaceship either way
    /// </summary>
    /// <param name="sender">ship object's Ship component</param>
    public void TryStartLevel(Ship sender)
    {
        StartCoroutine(DoTryStartLevel(sender));
    }

    /// <summary>
    /// Tries to start the level, activates spaceship either way
    /// </summary>
    /// <param name="sender">ship object's Ship component</param>
    IEnumerator DoTryStartLevel(Ship sender)
    {
        //Debug.Log("Trying to start level...");

        currentShip = sender;

        // If level is NOT started, start it
        if (!isLevelStarted)
        {
            // Level countdown
            countdownText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            countdownText.text = "2...";
            yield return new WaitForSeconds(1);
            countdownText.text = "1...";
            yield return new WaitForSeconds(1);
            countdownText.text = "Park!";
            yield return new WaitForSeconds(1);
            countdownText.text = "3...";
            countdownText.gameObject.SetActive(false);

            // Set timeRemaining
            timeRemaining = _levelTime;

            // Start Level
            isLevelStarted = true;

            // Activate Spaceship
            currentShip.ActivateShip(true);
        }
        else // if level is started, activate ship controls
        {
            currentShip.ActivateShip(true);
        }
    }

    /// <summary>
    /// Does Game Over logic and brings us back to Main Menu
    /// </summary>
    public void LevelFailed()
    {
        StartCoroutine(DoLevelFailed());
    }

    /// <summary>
    /// Does Game Over logic and brings us back to Main Menu
    /// </summary>
    IEnumerator DoLevelFailed()
    {
        // Stop level
        isLevelStarted = false;

        // Try to set new highscore (only works if higher)
        bool isNewHighscore = SetHighscore(score);

        // Activate Game Over UI
        gameOver.SetActive(true);
        if (isNewHighscore)
        {
            newHighscore.SetActive(true);
        }

        // Wait a few seconds
        yield return new WaitForSeconds(10);

        // Deactivate Game Over UI
        newHighscore.SetActive(false);
        gameOver.SetActive(false);

        // Return to Menu
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void LevelSucceeded()
    {

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

    /// <summary>
    /// Waits for number of seconds inputted
    /// </summary>
    /// <param name="seconds">seconds to wait</param>
    /// <returns></returns>
    IEnumerator WaitSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}