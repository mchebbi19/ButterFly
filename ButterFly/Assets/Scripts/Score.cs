using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score 
{

    

    public static void Start()
    {
        Bird.GetInstance().Ondied += Bird_OnDied;    
    }

    private static void Bird_OnDied(object sender, System.EventArgs e)
    {
        TrySetNewHighscore(Level.GetInstance().GetPipePassedCount());
    }



    // Start is called before the first frame update
    public static int GetHighscore()
    {
        int hightscore = PlayerPrefs.GetInt("Highscore");
        return hightscore;
    }

    public static bool TrySetNewHighscore(int score)
    {
        int currentHighscore = GetHighscore();
       
        if (score > currentHighscore)
        {
            // New hight score
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
           

            return true;
        }
        else
        {
            return false;
        }
       
    }

    public static void ResetGetHighscore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.Save();
    }

}
