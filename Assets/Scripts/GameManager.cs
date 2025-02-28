using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variables
    [SerializeField] int score = 0;
    [SerializeField] TMP_Text timeCount;
    [SerializeField] TMP_Text scoreCount;
    [SerializeField] TMP_Text highscoreCount;

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

    public int GetHighscore()
    {
        // Gets highscore or zero if no highscore saved
        return PlayerPrefs.GetInt(_highscore, 0);
    }

    /// <summary>
    /// Sets new highscore if new highscore is greater than old highscore.
    /// </summary>
    /// <param name="newHighscore">score number to set highscore to</param>
    public void SetHighscore(int newHighscore)
    {
        int oldHighscore = GetHighscore();

        if (newHighscore > oldHighscore)
        {
            // Saves new highscore
            PlayerPrefs.SetInt(_highscore, newHighscore);

            // Update UI
            UpdateScoreUI();
        }
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