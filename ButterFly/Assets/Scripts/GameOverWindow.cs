using CodeMonkey.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highScoreText;
    private Button_UI button_UI;

    // Start is called before the first frame update
    private void Awake()
    {

        scoreText = transform.Find("GameOverScoreText").GetComponent<Text>();
        highScoreText = transform.Find("highscoreText").GetComponent<Text>();
        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {

            Loader.Load(Loader.Scene.GameScene);
        };
        transform.Find("MainMenuBtn").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.Menu); };


    }



    // Update is called once per frame
    private void Start()
    {
        Bird.GetInstance().Ondied += Bird_OnDied;
        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        Debug.Log("GameHandler.Bird_OnDied1");
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
        if (Level.GetInstance().GetPipePassedCount() > Score.GetHighscore())
        {
            highScoreText.text = "NEW HIGHSCORE : " + Level.GetInstance().GetPipePassedCount();
        }
        else
        {
            highScoreText.text = "HIGHSCORE : " + Score.GetHighscore();
        }

        int number = Random.Range(1, 4);
        if (number == 1)
        {
            Show();
            AdmobManager.GetInstance().ShowInterstitialAd();
        }
        else
        {
            Show();
        }
       
        

      

    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);

    }


    private float step = 20f;
    private float currentTime = 0;
    IEnumerator TimerRoutine()
    {

        yield return new WaitForSeconds(step);
        gameObject.SetActive(true);
      
    }


}
