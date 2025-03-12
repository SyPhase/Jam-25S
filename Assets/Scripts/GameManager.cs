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
    [SerializeField] TMP_Text livesCount;

    int lives = 3;
    int level = 1;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

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
    void LevelFailed()
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
        currentShip.ActivateShip(false);

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

    public void TryLevelSucceeded()
    {
        level++;

        if (lives < 3 && level % 3 == 0)
        {
            lives++;
            UpdateLivesUI();
        }

        // Reset level time when new level starts
        timeRemaining = _levelTime;

        // TODO : Don't load past final level
        // Load next Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Adds points to game score
    /// </summary>
    /// <param name="points">Points to add to score</param>
    public void AddPoints(int points)
    {
        score += points * level;
        UpdateScoreUI();
    }

    /// <summary>
    /// Removes one life
    /// </summary>
    public void RemoveLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            LevelFailed();
        }
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
    /// Updates lives UI
    /// </summary>
    void UpdateLivesUI()
    {
        if (lives == 3)
        {
            livesCount.text = "";
        }
        else if (lives == 2)
        {
            livesCount.text = "X";
            StartCoroutine(FlashLives(1));
        }
        else if (lives == 1)
        {
            livesCount.text = "X X";
            StartCoroutine(FlashLives(2));
        }
        else
        {
            livesCount.text = "X X X";
            StartCoroutine(FlashLives(3));
        }
    }

    /// <summary>
    /// Flashes lives a few times
    /// </summary>
    /// <param name="urgency">Higher number flashes faster but more times</param>
    /// <returns></returns>
    IEnumerator FlashLives(int urgency)
    {
        for (int i = 0; i < urgency * 3; i++)
        {
            yield return new WaitForSeconds(0.4f / urgency);
            livesCount.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f / urgency);
            livesCount.gameObject.SetActive(true);
        }
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