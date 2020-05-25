using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Security;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using Facebook.MiniJSON;
using System;

public class FBscript : MonoBehaviour
{

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUsername;
    public GameObject DialogProfilPic;
    public Text scoreDebug;
    // Start is called before the first frame update
    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    // Update is called once per frame
    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB is logged in");
        }
        else
        {
            Debug.Log("FB is not logged in");
        }
        DealWithFBMenus(FB.IsLoggedIn);
    }

    void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBlogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
    
        permissions.Add("user_about_me");
        permissions.Add("user_friends");
        permissions.Add("manage_pages");
        permissions.Add("user_photos");
        permissions.Add("user_posts");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
        
    }

    void AuthCallBack(IResult result)
    {
        if(result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("FB is logged in");
            }
            else
            {
                Debug.Log("FB is not logged in");
            }

            DealWithFBMenus(FB.IsLoggedIn);
        }
        
    }

    void DealWithFBMenus(bool isloggedIn)
    {
        if (isloggedIn)
        {
            DialogLoggedIn.SetActive(true);
            DialogLoggedOut.SetActive(false);
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&widh=128", HttpMethod.GET, DisplayProfilePic);

        }
        else
        {
            DialogLoggedIn.SetActive(false);
            DialogLoggedOut.SetActive(true);
        }
    }

    void DisplayUsername(IResult result)
    {
        Text UserName = DialogUsername.GetComponent<Text>();
        if (result.Error==null) 
        {
            UserName.text = "Hi there, " + result.ResultDictionary["first_name"];
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
       
        if (result.Texture != null)
        {
             Image ProfilePic = DialogProfilPic.GetComponent<Image>();
            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 80, 130), new Vector2());
        }
        
    }


    public void QueryScore() {

        FB.API("me/friends", HttpMethod.GET, FriendCallBack);       
    }

 

   


    void FriendCallBack(IGraphResult result)
    {
        IDictionary<string, object> data = result.ResultDictionary;
        List<object> friends = (List<object>)data["data"];
        foreach (object obj in friends)
        {
            Dictionary<string, object> entry = (Dictionary<string, object>)obj;
            // Dictionary<string, object> user = (Dictionary<string, object>)entry["user"];
            //Debug.Log(entry["user"].ToString());
            //Debug.Log(entry["score"].ToString());
            Debug.Log(entry["name"].ToString());
            Debug.Log(entry["id"].ToString());
        }

        //foreach (object obj in friends)
        //{
        //    var entry = (Dictionary<string, object>)obj;
        //    var user = (Dictionary<string, object>)entry["user"];

        //    Debug.Log(user["name"].ToString() + " , " + entry["score"].ToString());
        //    scoreDebug.text = user["name"].ToString() + " " + entry["score"].ToString();
        //    Debug.Log("test");
        //    //scoreDebug.text = result.RawResult;
        //}

    }



    private string getDataValueForKey(Dictionary<string, object> dict, string key)
    {
        object objectForKey;
        if (dict.TryGetValue(key, out objectForKey))
        {
            return (string)objectForKey;
        }
        else
        {
            return "";
        }
    }

    public void SetScore()
    {
        var scoreData = new Dictionary<string, string>();
        scoreData["score"] = UnityEngine.Random.Range(0, 250).ToString();
        FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult result) {
            Debug.Log(" Score submitted successfuly " + result.RawResult);
        },scoreData);


    }
}
