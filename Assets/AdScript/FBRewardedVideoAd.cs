using AudienceNetwork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBRewardedVideoAd : MonoBehaviour
{
    public string PlacementsID;
    private RewardedVideoAd rewardedVideoAd;
    private bool isLoaded;
    AdManagerUnity _adManagerUnity;

    private void Start()
    {
        _adManagerUnity = GetComponent<AdManagerUnity>();
    }

    public void LoadRewardedVideo()
    {
        // Create the rewarded video unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        this.rewardedVideoAd = new RewardedVideoAd(PlacementsID);

        this.rewardedVideoAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.rewardedVideoAd.RewardedVideoAdDidLoad = (delegate () {
            Debug.Log("RewardedVideo ad loaded.");
            this.isLoaded = true;
        });
        this.rewardedVideoAd.RewardedVideoAdDidFailWithError = (delegate (string error) {
            Debug.Log("RewardedVideo ad failed to load with error: " + error);
        });
        this.rewardedVideoAd.RewardedVideoAdWillLogImpression = (delegate () {
            Debug.Log("RewardedVideo ad logged impression.");
        });
        this.rewardedVideoAd.RewardedVideoAdDidClick = (delegate () {
            Debug.Log("RewardedVideo ad clicked.");
        });

        this.rewardedVideoAd.RewardedVideoAdDidClose = (delegate () {
            Debug.Log("Rewarded video ad did close.");
            if (this.rewardedVideoAd != null)
            {
                ReloadAd();
            }
        });


        // For S2S validation you need to register the following two callback
        this.rewardedVideoAd.RewardedVideoAdDidSucceed = (delegate () {
            Debug.Log("Rewarded video ad validated by server");
            AdManager.instance.OnrewardedAdShownFully();
        });
        this.rewardedVideoAd.RewardedVideoAdDidFail = (delegate () {
            Debug.Log("Rewarded video ad not validated, or no response from server");
        });






        //        this.rewardedVideoAd.rewardedVideoAdDidClose = (delegate () {
        //            Debug.Log("Rewarded video ad did close.");
        //            this.didClose = true;
        //            if (this.rewardedVideoAd != null)
        //            {
        //                this.rewardedVideoAd.Dispose();
        //            }
        //        });

        //#if UNITY_ANDROID
        //        /*
        //         * Only relevant to Android.
        //         * This callback will only be triggered if the Rewarded Video activity
        //         * has been destroyed without being properly closed. This can happen if
        //         * an app with launchMode:singleTask (such as a Unity game) goes to
        //         * background and is then relaunched by tapping the icon.
        //         */
        //        this.rewardedVideoAd.rewardedVideoAdActivityDestroyed = (delegate () {
        //            if (!this.didClose)
        //            {
        //                Debug.Log("Rewarded video activity destroyed without being closed first.");
        //                Debug.Log("Game should resume. User should not get a reward.");
        //            }
        //        });
        //#endif


        // Initiate the request to load the ad.
        this.rewardedVideoAd.LoadAd();
    }

    private void ReloadAd()
    {
        this.rewardedVideoAd.Dispose();
        LoadRewardedVideo();
    }


    /// <summary>
    /// Show Rewarded ad..
    /// </summary>
    public void ShowRewardedVideo()
    {
        if (this.isLoaded)
        {
            this.rewardedVideoAd.Show();
            this.isLoaded = false;
        }
        else
        {
            _adManagerUnity.ShowRewardedAd();
            Debug.Log("Ad not loaded. Click load to request an ad.");
        }
    }
}
