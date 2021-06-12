using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    // The score text
    public Text scoreText;

    private float score;

    private void Start()
    {
        score = 0;
    }

    private void Update()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void add_score(float addedScore)
    {
        score += addedScore;
    }
}
