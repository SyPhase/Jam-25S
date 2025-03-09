using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text highscoreCount;

    const string _highscore = "highscore";

    void Awake()
    {
        highscoreCount.text = PlayerPrefs.GetInt(_highscore, 0).ToString();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }
}