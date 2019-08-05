using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.scoreText = this.GetComponent<Text>();

        UpdateText();
    }

    public void ScoreHit(int additionalPoints)
    {
        this.score += additionalPoints;
        UpdateText();
    }

    private void UpdateText()
    {
        this.scoreText.text = this.score.ToString();
    }

    private Text scoreText;
    private int score;
}
