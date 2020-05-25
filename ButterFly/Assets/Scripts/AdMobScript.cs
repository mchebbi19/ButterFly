using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
public class AdMobScript : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private static AdMobScript instance;
    private bool bInmodeTest = true;
    public static AdMobScript GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
#if UNITY_ANDROID

        string appId = "";
        if (bInmodeTest)
        {
            appId = "ca-app-pub-3940256099942544~3347511713"; //Test
        }
        else
        {
            appId = "ca-app-pub-4437363774481806~8702689475";
        }


#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif

#if UNITY_ANDROID
        Debug.Log("Test debogeur");
            #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

       this.RequestBanner();


        

      
        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        //this.RequestRewardBasedVideo();

        this.rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoOnAdLoaded;

        // Called when an ad request failed to load.
        this.rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;

        // Called when the user should be rewarded for watching a video.
        this.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        this.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
    }

    public void RequestBanner()
    {
#if UNITY_ANDROID

        string adUnitId;
        if (bInmodeTest)
        {
            adUnitId = "ca-app-pub-3940256099942544/6300978111";  //test
        }
        else
        {
            adUnitId = "ca-app-pub-4437363774481806/7198036114";
        }

#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);


        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += HandleBannerView_OnAdLoaded;        
                
        
    }

    private void HandleBannerView_OnAdLoaded(object sender, EventArgs args)
    {
        this.bannerView.Show();    

    }

    public void BannerViewDistroy()
    {
        this.bannerView.Destroy();

    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId;
        if (bInmodeTest)
        {
            adUnitId = "ca-app-pub-3940256099942544/1033173712"; //test
        }
        else
        {
            adUnitId = "ca-app-pub-4437363774481806/8541007563";
        }

#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        this.interstitial.OnAdLoaded += Handle_Interstitial_OnAdLoaded;

        this.interstitial.OnAdFailedToLoad += Handle_Interstitial_OnFailedToLoad;

    }

    private void Handle_Interstitial_OnAdLoaded(object sender, EventArgs args)
    {

        this.interstitial.Show();
       
    }

    private void Handle_Interstitial_OnFailedToLoad(object sender, EventArgs args)
    {
       
    }

    public void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID

        string adUnitId;
        if (bInmodeTest)
        {
            adUnitId = "ca-app-pub-3940256099942544/5224354917"; //Test
        }
        else
        {
            adUnitId = "ca-app-pub-4437363774481806/3360455773";
        }

#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);

        
    }

    public void ShowRewardAd()
    {
        Debug.Log("Rewarded0 Showed");
        if (rewardBasedVideo.IsLoaded())
        {
            Debug.Log("Rewarded1 Showed");
            rewardBasedVideo.Show();
        }
        else
        {
            Debug.Log("Rewarded ad not loaded");
        }
    }

    //public void Handle_rewardBasedVideo_OnAdLoaded(object sender, EventArgs args)
    //{
    //    this.rewardBasedVideo.Show();
    //}

    public void HandleRewardBasedVideoOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded video ad loaded successfuly" );
    }
    

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Failed to load rewards video ad :" + args.Message);
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("You have been rewarded with  :" + amount.ToString() + " " + type);
        AddAwards.instance.RewardPanel.SetActive(true);
        AddAwards.instance.WatchVideoRewards.SetActive(false);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded  video has closed");
        RequestRewardBasedVideo();
    }
}
