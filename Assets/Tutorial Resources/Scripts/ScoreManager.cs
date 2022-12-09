using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Core.Managers.Audio;

public class ScoreManager : MonoBehaviour
{
    public static int score;        // The player's score.


    public Text text;                      // Reference to the Text component.

    private int scoreRequirement = 100;
    private int prevScore = 0;
    void Awake()
    {
        // Set up the reference.
        text = GetComponent<Text>();

        // Reset the score.
        score = 0;
    }


    void Update()
    {
        // Set the displayed text to be the word "Score" followed by the score value.
        text.text = "Score: " + score;
        if (score <= prevScore + scoreRequirement)
        {
            return;
        }
        AudioManager.Instance.Stage = AudioManager.Instance.Stage + 1;
        prevScore = score;
        scoreRequirement *= 2;
    }
}
