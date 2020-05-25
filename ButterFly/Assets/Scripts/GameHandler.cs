using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

 

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("GameHandler.Start");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Score.Start();
        

      

        /* debug on screen */

        //int count = 0;
        //FunctionPeriodic.Create(() => {
        //    CMDebug.TextPopup("Ding! " + count , new Vector3(0.0f, 0.0f, 0.0f));
        //    count++;
        //}, .300f);


        //GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));
        // gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHeadSprite;

    }

   
}
