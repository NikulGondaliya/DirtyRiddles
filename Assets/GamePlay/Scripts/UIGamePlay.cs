using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UIGamePlay : UIScreen
{
    [SerializeField] private UIItem m_TxtLevelNumber;
    [SerializeField] private UIAnswerMenu m_UIAnswerMenu = null;
    [SerializeField] private UIQuestionsMenu m_UIQuestionMenu = null;
    [SerializeField] private UIMainMenu m_UIMainMenu = null;
    [SerializeField] private UIStore m_UIStore = null;
    [SerializeField] private UIItem m_TxtCoins = null;
    public Action ExitCallback = null;
    public Action OpenStoreCallback = null;

    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if (item.name == "BtnBack")
        {
            ExitCallback?.Invoke();
            m_UIAnswerMenu.SetVisibility(false);
            m_UIQuestionMenu.SetVisibility(false);
            SetVisibility(false);
            m_UIMainMenu.SetVisibility(true);
        }
            
        else if (item.name == "BtnStore")
        {
            OpenStoreCallback?.Invoke();
            m_UIAnswerMenu.SetVisibility(false);
            m_UIQuestionMenu.SetVisibility(false);
            SetVisibility(false);
            m_UIStore.SetVisibility(true);
            m_UIStore.ExitCallback += ExitFromStore;
        }          
    }

    public void SetLevelNumber(int levelNumber)
    {
        m_TxtLevelNumber.pText = "Level " + levelNumber.ToString();
    }

    public override void SetVisibility(bool isVisible, bool playAnim = true)
    {
        base.SetVisibility(isVisible, playAnim);

        m_TxtCoins.pText = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    private void ExitFromStore()
    {
        m_UIAnswerMenu.SetVisibility(true);
        m_UIQuestionMenu.SetVisibility(true);
        SetVisibility(true);
        m_UIStore.SetVisibility(false);
        m_UIStore.ExitCallback -= ExitFromStore;
    }

    public void SetCointText()
    {
        m_TxtCoins.pText = PlayerPrefs.GetInt("Coins", 0).ToString();
    }
}
