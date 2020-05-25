using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
public class ScoreWindow : MonoBehaviour
{

    private Text scoreText;
    private Text highscoreText;

    // Start is called before the first frame update
    private void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<Text>();
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();

    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
    }
     private void Start()
    {
        int highscore = Score.GetHighscore();
        
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
        

    }

}
