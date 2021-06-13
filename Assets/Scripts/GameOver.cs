using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Is the game over?
    public static bool isOver;

    // Audio source and clip for death sound
    public AudioSource deathSource;
    public AudioClip deathClip;

    // Screen to display on the UI
    public GameObject gameOverScreen;
    // Text to update with time remaining
    public Text gameOverTimerText;
    // Time before restarting the game
    public float restartTime = 3.0f;

    // Timer for the restart
    private float restartTimer;
    // Are we restarting?
    private bool isRestarting;

    private void Start()
    {
        isRestarting = false;
        restartTimer = 0.0f;

        // Just to be sure ...
        isOver = false;
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (isRestarting)
        {
            // Add the REAL delta time
            restartTimer += Time.unscaledDeltaTime;

            // Update the timer on the screen with the rounded-up restart time
            int remainingTime = Mathf.CeilToInt(restartTime - restartTimer);
            gameOverTimerText.text = remainingTime.ToString();

            if (restartTimer >= restartTime)
            {
                // Yeahhhhh ... this is, like, the one thing that won't get reset otherwise
                Time.timeScale = 1.0f;

                // Restart!
                isOver = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }

    public void game_over()
    {
        if (isRestarting)
            return;

        isOver = true;

        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.Stop();
        }

        deathSource.PlayOneShot(deathClip);

        isRestarting = true;
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);
    }
}
