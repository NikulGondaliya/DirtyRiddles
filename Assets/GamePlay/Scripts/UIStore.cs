using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Purchasing;

[System.Serializable]
public class IAPButtunInfo
{
    public string _IAPId;
    public UIButton _Button = null;
}
public class UIStore : UIScreen
{
    [SerializeField] private UIMainMenu m_UIMainMenu = null;
    [SerializeField] private UIItem m_TxtCoins = null;
    [SerializeField] private int m_ItemsCount = 5;
    [SerializeField] private AudioClip m_SwipeClip;
    [SerializeField] private IAPButtunInfo[] m_IAPButtonInfo = null;
    [SerializeField] private UIItem uIItem;
    private int mCurrentSwipeClipCount = -1;
    public Action ExitCallback = null;
    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if (item.name == "BtnCoins10000")
        {
            string id = (string)item.pData;
            IAPManager.BuyProductID(id, 10000, OnCoinPurchaseComplete);
        }
        else if (item.name == "BtnCoins3000")
        {
            string id = (string)item.pData;
            IAPManager.BuyProductID(id, 3000, OnCoinPurchaseComplete);
        }
        else if (item.name == "BtnCoins1000")
        {
            string id = (string)item.pData;
            IAPManager.BuyProductID(id, 1000, OnCoinPurchaseComplete);
        }
        else if (item.name == "BtnCoins50")
        {
            AdManager.instance.ShowAdmobRewardVideoForContinue();
        }
        else if (item.name == "BtnBack")
        {
            ExitCallback += ExitFromStore;
            ExitCallback?.Invoke();
        }
    }

    public override void SetVisibility(bool isVisible, bool playAnim = true)
    {
        base.SetVisibility(isVisible);

        m_TxtCoins.pText = PlayerPrefs.GetInt("Coins", 0).ToString();
        if(isVisible)
        {
            mCurrentSwipeClipCount = 0;
            StartCoroutine(StartSwipeClip());
        }  
        
        if(isVisible && IAPManager._StoreController != null)
        {
            foreach (IAPButtunInfo iapBtnInfo in m_IAPButtonInfo)
            {
                Product product = IAPManager._StoreController.products.WithID(iapBtnInfo._IAPId);
                iapBtnInfo._Button.pText = product.metadata.localizedPriceString;
                print("ddd " +iapBtnInfo._Button.pText );
                iapBtnInfo._Button.pData = iapBtnInfo._IAPId;
                print("bbb " + iapBtnInfo._Button.pData);
            }
        }
    }

    private void ExitFromStore()
    {
        SetVisibility(false);
        ExitCallback -= ExitFromStore;
    }

    IEnumerator StartSwipeClip()
    {
        
        while(mCurrentSwipeClipCount < m_ItemsCount)
        {
            SoundManager.Play(m_SwipeClip, SoundType.OneShotSFX);
            mCurrentSwipeClipCount++;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnCoinPurchaseComplete(int quantity)
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coins += quantity;
        PlayerPrefs.SetInt("Coins", coins);
        m_TxtCoins.pText = coins.ToString();
        //uIItem.pText = coins.ToString();
    }

    public void OnVideoAdComplete()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coins += 50;
        PlayerPrefs.SetInt("Coins", coins);
        m_TxtCoins.pText = coins.ToString();
        //uIItem.pText = coins.ToString();
    }
}
