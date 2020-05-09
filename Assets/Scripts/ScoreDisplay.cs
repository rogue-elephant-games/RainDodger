using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        GetScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        GetScoreText();
    }

    private void GetScoreText()
    {
        if(gameSession == null)
            gameSession = FindObjectOfType<GameSession>();
        if(scoreText == null)
            scoreText = GameObject.FindWithTag("ScoreText")?.GetComponent<TextMeshProUGUI>();
        
        if(scoreText != null)
            scoreText.text = gameSession.GetScore().ToString();
    }
}
