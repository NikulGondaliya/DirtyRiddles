using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    private AdManagerUnity _adManagerUnity;


    public FBInterstitialAd _fBInterstitial;
    public FBRewardedVideoAd _fBRewarded;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    //static bool OneTime;
    public UIStore uistore;
    public static bool firstTime = true;

    public static AdManager instance;
    public string bannerID = "/6499/example/banner";
    public string interstitialID = "/6499/example/interstitial"; 
    public string rewardVideoID = "/6499/example/rewarded";
    public int RewardValue;
    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        instance = this;
    }

    void Start()
    {
        _adManagerUnity = GetComponent<AdManagerUnity>();
        _fBInterstitial = GetComponent<FBInterstitialAd>();
        _fBRewarded = GetComponent<FBRewardedVideoAd>();
        Debug.Log("Initialize the Mobile Ads SDK.");
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
                // SDK initialization is complete
                Debug.Log("SDK initialization is complete");
                //RequestBanner();
                RequestInterstitial();
            RequestRewardBasedVideo();
        });

    }
    //Call this to show banner ad
    public void ShowAdmobBanner()
    {
        //RequestBanner();
        //this.bannerView.Show();
    }

    //    //Call this to hide banner ad
    public void HideAdmobBanner()
    {
        //this.bannerView.Destroy();
        //this.bannerView.Hide();
    }

    //    //Call this to show interstitial ad
    public void ShowAdmobInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            //_adManagerUnity.ShowAd();
            _fBInterstitial.ShowInterstitial();
        }
    }
        int Val;
    //    //Call this to show reward video ad
    public void ShowAdmobRewardVideoForContinue()
    {
        Val = 0;
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            //When completed this function is called : HandleRewardBasedVideoRewarded
        }
        else
        {
            //_adManagerUnity.ShowRewardedAd();
            _fBRewarded.ShowRewardedVideo();
            Debug.Log("ADDED Not Loaded......................................");
        }
    }
    //public void ShowAdmobRewardVideoForSkin(int Value)
    //{
    //    Val = Value;
    //    if (rewardedAd.IsLoaded())
    //    {
    //        rewardedAd.Show();
    //        //When completed this function is called : HandleRewardBasedVideoRewarded
    //    }
    //    else
    //    {
    //        Debug.Log("ADDED Not Loaded..........................................");
    //        RequestRewardBasedVideo();
    //    }
    //}

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = interstitialID;
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
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        Debug.Log("Inter Requested............");
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestInterstitial();
        Debug.Log("HandleOn AdFailed To Load .. event recived...");
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //RequestInterstitial();
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
        MonoBehaviour.print("HandleAdClosed event received");
    }




    public void RequestRewardBasedVideo()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = rewardVideoID;
#elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    		adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        Debug.Log("Rewarded requested................");

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();

        //// Load the rewarded video ad with the request.
        //admanagerInstance.rewardBasedVideoAd.LoadAd(request, "ca-app-pub-3940256099942544/5224354917");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        Debug.Log("Faild to load " + e);
        RequestRewardBasedVideo();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        RequestRewardBasedVideo();
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        Debug.Log("Earned");
        OnrewardedAdShownFully();
    }




    public void OnrewardedAdShownFully()
    {
        uistore.OnVideoAdComplete();

    }


//    private void RequestBanner()
//    {
//        string adUnitId;
//#if UNITY_ANDROID
//        adUnitId = bannerID;
//#elif UNITY_IPHONE
//                 adUnitId = "ca-app-pub-3940256099942544/2934735716";
//#else
//                 adUnitId = "unexpected_platform";
//#endif

    //        // Create a 320x50 banner at the top of the screen.
    //        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);


    //        // Create an empty ad request.
    //        AdRequest request = new AdRequest.Builder().Build();

    //        // Load the banner with the request.
    //        this.bannerView.LoadAd(request);
    //    }






}
