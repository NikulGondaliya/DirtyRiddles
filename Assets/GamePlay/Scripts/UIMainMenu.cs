using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UIMainMenu : UIScreen
{
    [SerializeField] private UIStore m_UIStore;
    [SerializeField] private UIItem m_TxtCoins;

    public Action OnPlayButtonClicked = null;
    public Action OnMenuButtonClicked = null;

    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if (item.name == "BtnPlay")
            OnPlayButtonClicked?.Invoke();
        else if (item.name == "BtnMenu")
            OnMenuButtonClicked?.Invoke();
        else if(item.name == "BtnStore")
        {
            SetVisibility(false);
            m_UIStore.SetVisibility(true);
            m_UIStore.ExitCallback += ExitFromStore;
        }
    }

    public override void SetVisibility(bool isVisible, bool playAnim = true)
    {
        base.SetVisibility(isVisible, playAnim);

        m_TxtCoins.pText = PlayerPrefs.GetInt("Coins", 0).ToString();
    }
    private void ExitFromStore()
    {
        m_UIStore.SetVisibility(false);
        m_UIStore.ExitCallback -= ExitFromStore;
        SetVisibility(true);
    }
}
