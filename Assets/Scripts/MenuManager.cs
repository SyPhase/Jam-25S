using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text highscoreCount;

    const string _highscore = "highscore";
    bool keyPressed = false;

    void Awake()
    {
        highscoreCount.text = PlayerPrefs.GetInt(_highscore, 0).ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.anyKeyDown)
        {
            keyPressed = true;
        }

        if (keyPressed && Time.time > 3.0f)
        {
            SceneManager.LoadScene(1);
        }
    }
}