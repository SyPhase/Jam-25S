using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Setup
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); }
        else { instance = this; }

        // Persists through Scene changes
        DontDestroyOnLoad(gameObject);
    }

    // Variables
    [SerializeField] int score = 0;
    const string _highscore = "highscore";

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
        }
    }
}
