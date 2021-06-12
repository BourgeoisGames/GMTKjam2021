using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void game_over()
    {
        Debug.Log("YOU SUCK ASS, START OVER YOU DINGUSWEED");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
