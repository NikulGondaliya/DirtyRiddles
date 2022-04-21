using AudienceNetwork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FBInterstitialAd : MonoBehaviour
{
    public string PlacementsID;
    private InterstitialAd interstitialAd;
    private bool isLoaded;
    private AdManagerUnity _adManagerUnity;

    private void Start()
    {
        _adManagerUnity.GetComponent<AdManagerUnity>();
        AudienceNetworkAds.Initialize();
    }

    public void LoadInterstitial()
    {
        this.interstitialAd = new InterstitialAd(PlacementsID);
        this.interstitialAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.interstitialAd.InterstitialAdDidLoad = (delegate () {
            Debug.Log("Interstitial ad loaded.");
            this.isLoaded = true;
        });
        interstitialAd.InterstitialAdDidFailWithError = (delegate (string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
        });
        interstitialAd.InterstitialAdWillLogImpression = (delegate () {
            Debug.Log("Interstitial ad logged impression.");
        });
        interstitialAd.InterstitialAdDidClick = (delegate () {
            Debug.Log("Interstitial ad clicked.");
        });

        this.interstitialAd.interstitialAdDidClose = (delegate () {
            Debug.Log("Interstitial ad did close.");
            if (this.interstitialAd != null)
            {
               
            }
        });


        //this.interstitialAd.interstitialAdDidClose = (delegate () {
        //    Debug.Log("Interstitial ad did close.");
        //    this.didClose = true;
        //    if (this.interstitialAd != null)
        //    {
        //        this.interstitialAd.Dispose();
        //    }
        //});



//#if UNITY_ANDROID
//        /*
//         * Only relevant to Android.
//         * This callback will only be triggered if the Interstitial activity has
//         * been destroyed without being properly closed. This can happen if an
//         * app with launchMode:singleTask (such as a Unity game) goes to
//         * background and is then relaunched by tapping the icon.
//         */
//        this.interstitialAd.interstitialAdActivityDestroyed = (delegate () {
//            if (!this.didClose)
//            {
//                Debug.Log("Interstitial activity destroyed without being closed first.");
//                Debug.Log("Game should resume.");
//            }
//        });
//#endif




        // Initiate the request to load the ad.
        this.interstitialAd.LoadAd();
    }


    private void ReloadAd()
    {
        this.interstitialAd.Dispose();
        LoadInterstitial();
    }



    // Show button
    public void ShowInterstitial()
    {
        if (this.isLoaded)
        {
            this.interstitialAd.Show();
            this.isLoaded = false;

        }
        else
        {
            _adManagerUnity.ShowAd();
            Debug.Log("Interstitial Ad not loaded!");
        }
    }
}