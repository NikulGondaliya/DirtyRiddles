using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWatchVideo : UIScreen
{
    [SerializeField] private UIStore m_UIStore = null;
    [SerializeField] private AudioClip m_VisibilityChangeClip = null;
    [SerializeField] private UIGamePlay m_UIGamePlay = null;
    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if (item.name == "BtnStore")
        {
            m_UIStore.SetVisibility(true);
            SetVisibility(false);
            m_UIStore.ExitCallback += ExitFromStore;
        }
        else if (item.name == "BtnVideo")
        {
            AdManager.instance.ShowAdmobRewardVideoForContinue();
        }
        else if (item.name == "BtnBack")
        {
            TweenAnim[] tweenAnims = GetComponentsInChildren<TweenAnim>();
            foreach (TweenAnim anim in tweenAnims)
                anim.PlayExitAnim();

            StartCoroutine(HideWithDelay());
            //AdManager.instance.ShowBannerAd();
        }
    }

    IEnumerator HideWithDelay()
    {
        yield return new WaitForSeconds(0);

        SetVisibility(false, false);
    }

    public override void SetVisibility(bool isVisible, bool playAnim = true)
    {
        if (isVisible)
            SoundManager.Play(m_VisibilityChangeClip, SoundType.OneShotSFX);
        base.SetVisibility(isVisible, playAnim);
    }

    private void ExitFromStore()
    {
        m_UIStore.SetVisibility(false);
        SetVisibility(false);
        m_UIStore.ExitCallback -= ExitFromStore;
    }
    public void OnVideoAdComplete()
    {
        Debug.Log("Get Reward ....................................................................................!!!!!!!!");
        int coins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("..............................................!!!!!!!!!!!!!!!!!! ...................Coin = " + coins + "....................................................");
        coins += 50;
        PlayerPrefs.SetInt("Coins", coins);
        Debug.Log("..............................................!!!!!!!!!!!!!!!!!! ...................UPDATED  Coin = " + coins +"....................................................");
        m_UIGamePlay.SetCointText();
    }
}
