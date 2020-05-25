using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
public class MainMenuWindow : MonoBehaviour
{
    // Start is called before the first frame update
    private int test;
    private int test2;
    private void Awake()
    {
        transform.Find("PLAY").GetComponent<Button_UI>().ClickFunc = () => {
           // AdMobScript.GetInstance().RequestInterstitial();
            Loader.Load(Loader.Scene.GameScene); };        
        transform.Find("RESTHIGHSCORE").GetComponent<Button_UI>().ClickFunc = () => {
            //AdMobScript.GetInstance().RequestInterstitial();
            Score.ResetGetHighscore(); 
            setVisibilityHighScore(); };
        transform.Find("QUIT").GetComponent<Button_UI>().ClickFunc = () => { Application.Quit(); };

        transform.Find("EnableSound").GetComponent<Button_UI>().ClickFunc = () => { EnableSound(); };
        transform.Find("DisableSound").GetComponent<Button_UI>().ClickFunc = () => { DisableSound(); };
        transform.Find("PrivacyPolicy").GetComponent<Button_UI>().ClickFunc = () => { OpenURL(); };
    }

    // Update is called once per frame
    void Start()
    {
        
    }

    private void Update()
    {
        setVisibilityHighScore();
    }

    private void setVisibilityHighScore()
    {
        int score = Score.GetHighscore();
        if (score == 0)
        {
            GameObject RESTHIGHSCORE;
            RESTHIGHSCORE = GameObject.Find("RESTHIGHSCORE");
            //RESTHIGHSCORE.SetActive(false);
        }
    }

    private void EnableSound()
    {

        PlayerPrefs.SetInt("EnableSound", 0);
        PlayerPrefs.Save();
        StaticValue.bEnableSound = false;

    }
    private void DisableSound()
    {
        PlayerPrefs.SetInt("EnableSound", 1);
        PlayerPrefs.Save();
        StaticValue.bEnableSound = true;

    }

    private void OpenURL()
    {
        Debug.Log("open");
        Application.OpenURL(StaticValue.PRivacyPolicyURL.ToString());
    }
}
