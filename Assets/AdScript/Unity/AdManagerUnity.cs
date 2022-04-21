using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManagerUnity : MonoBehaviour, IUnityAdsInitializationListener
{




    #region Unity ADS




    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;





    public float playAdTime;

    public InterstitialAdExample unityad;
    [SerializeField] RewardedAdUnity rewarded;



    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;

        unityad = GetComponent<InterstitialAdExample>();
        rewarded = GetComponent<RewardedAdUnity>();

        Advertisement.Initialize(_gameId, _testMode, this);
        Debug.Log("init Game Ad");

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        // Get the Ad Unit ID for the current platform:

        unityad.LoadAd();
        rewarded.LoadAd();
    }


    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        InitializeAds();
    }






    public void ShowAd()
    {
        unityad.ShowAd();
    }

    public void ShowRewardedAd()
    {
        rewarded.ShowAd();
    }


    #endregion
}
