using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;

public class AdmobManager : MonoBehaviour
{

    public bool AdmobTestAdsMode = false;


    [SerializeField] private string appId = "ca-app-pub-4437363774481806~8702689475";
    [SerializeField] private string bannerID = "ca-app-pub-4437363774481806/7198036114";
    [SerializeField] private string regularAdID = "ca-app-pub-4437363774481806/8541007563";
    [SerializeField] private string rewardedAdID = "ca-app-pub-4437363774481806/3360455773";
  
    public GameAssets gameAsset;
    public GameObject RewardedAppButton;

    public TextMeshProUGUI txtDbg;
    private string strDbg;
    

    private InterstitialAd interstitial;
    private BannerView bannerView;
    private RewardedAd rewardedAd;

    private static AdmobManager instance;

    public static AdmobManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;

        if (AdmobTestAdsMode == true)
        {
            appId = "ca-app-pub-3940256099942544~3347511713";
            bannerID = "ca-app-pub-3940256099942544/6300978111";
            regularAdID = "ca-app-pub-3940256099942544/1033173712";
            rewardedAdID = "ca-app-pub-3940256099942544/5224354917";
        }





        strDbg = "DATA";
        MobileAds.Initialize(initStatus => { });

        RequestBanner();

    

        this.rewardedAd = new RewardedAd(rewardedAdID);
        this.rewardedAd.OnUserEarnedReward += RewardedAd_HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += RewardedAd_HandleRewardedAdClosed;
        AdRequest request1 = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request1);

        Debug.Log("show Interstitial 9 ");
        this.interstitial = new InterstitialAd(regularAdID);
        this.interstitial.OnAdClosed += this.HandleAdClosed;
        this.interstitial.OnAdLoaded += this.HandleLoaded;
        AdRequest request2 = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request2);

    }

    private void RequestBanner()
    {

        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
       // bannerView.OnAdLoaded += HandleBannerView_OnAdLoaded;
    }

    //private void HandleBannerView_OnAdLoaded(object sender, EventArgs args)
    //{
    //    bannerView.Show();

    //}

    public void HandleAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        this.interstitial = new InterstitialAd(regularAdID);
        this.interstitial.OnAdClosed += this.HandleAdClosed;
        this.interstitial.OnAdLoaded += this.HandleLoaded;
        AdRequest request2 = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request2);
    }

    public void HandleLoaded(object sender, EventArgs args)
    {
       // Debug.Log("HandleLoaded event received");
        Debug.Log("show Interstitial 10 ");
    }




    public void ShowInterstitialAd()
    {
        Debug.Log("show Interstitial 2 ");
        bool lCanPlayInterstatialAd = GameAssets.GetInstance().adsCanPlayInterstatialAd();


        //Debug.Log("show Interstitial 2 is loaded " + interstitial.IsLoaded());

        //Debug.Log("show Interstitial 2 is null " + (this.interstitial==null));

        if (lCanPlayInterstatialAd)
        {
            Debug.Log("show Interstitial 3 ");
            if (this.interstitial != null && interstitial.IsLoaded())
            {
                Debug.Log("show Interstitial 4");
                interstitial.Show();
            }


            //interstitial.Show();
            

        }
    }

    


    public void RequestRewardedAd()
    {

        /// Added for debug only ----------
        //gameManager."(false);
        //return;
        //---------------------------------

        strDbg += "-" + "R";
        if (this.rewardedAd.IsLoaded())
        {
            strDbg += "-" + "S";
            rewardedAd.Show();
        }
    }

    public void RewardedAd_HandleUserEarnedReward(object sender, Reward args)
    {
        strDbg += "-" + "E";
        //gameManager.adsRequestRetry(false);
        string type = args.Type;
        int MunitionToWin = (int) args.Amount;
        Debug.Log("You have been rewarded with  :" + MunitionToWin.ToString() + " " + type);
       
    }

    public void RewardedAd_HandleRewardedAdClosed(object sender, EventArgs args)
    {
        strDbg += "-" + "C";

        this.rewardedAd = new RewardedAd(rewardedAdID);
        this.rewardedAd.OnUserEarnedReward += RewardedAd_HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += RewardedAd_HandleRewardedAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }
}
