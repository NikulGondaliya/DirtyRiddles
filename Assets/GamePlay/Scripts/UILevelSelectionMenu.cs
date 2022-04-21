using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILevelSelectionMenu : UIMenu
{
    [SerializeField] private Sprite m_CompletedLevelIcon;
    [SerializeField] private Sprite m_LockedLevelIcon;
    [SerializeField] private Sprite m_LevelToCompleteIcon;
    [SerializeField] private Image m_SlidingArea;
    [SerializeField] private UIStore m_UIStore = null;
    [SerializeField] private UIMainMenu m_UIMainMenu = null;
    [SerializeField] private UIItem m_TxtCoins = null;

    public Action<int> OnLevelSelected = null;
    public void SetUpMenu(int menuLength)
    {
        Clear();
        int levelCompletedCount = PlayerPrefs.GetInt("CurrentLevel", 0);

        int i = 0;

        while (i < levelCompletedCount)
        {
            UIItem uiItem = AddItem(i.ToString());
            UISimpleButton uiButton = (UISimpleButton)uiItem;
            uiButton._Background.sprite = m_CompletedLevelIcon;
            uiButton.pText = (i + 1).ToString();
            uiButton.pState = State.INTERACTIVE;
            uiButton.pData = i;
            uiButton.OnClickCallback = OnLevelButtonClicked;
            i++;
        }

        while(i < menuLength)
        {
            if(i == PlayerPrefs.GetInt("CurrentLevel"))
            {
                UIItem uiItem = AddItem(i.ToString());
                UISimpleButton uiButton = (UISimpleButton)uiItem;
                uiButton._Background.sprite = m_LevelToCompleteIcon;
                uiButton.pText = (i + 1).ToString();
                uiButton.pState = State.INTERACTIVE;
                uiButton.pData = i;
                uiButton.OnClickCallback = OnLevelButtonClicked;
            }
            else
            {
                UIItem uiItem = AddItem(i.ToString());
                UISimpleButton uiButton = (UISimpleButton)uiItem;
                uiButton._Background.sprite = m_LockedLevelIcon;
                uiButton.pText = "";
                uiButton.pState = State.INTERACTIVE;
            }

            i++;
        }
    }

    private void OnLevelButtonClicked(UIItem item)
    {
        if (item.pData != null)
        {
            int i = (int)item.pData;
            OnLevelSelected?.Invoke(i);
        }
    }

    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if(item.name == "BtnBack")
        {
            m_UIMainMenu.SetVisibility(true);
            SetVisibility(false);
        }
        else if(item.name == "BtnStore")
        {
            m_UIStore.SetVisibility(true);
            m_UIStore.ExitCallback += ExitFromStore;
            SetVisibility(false);
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
