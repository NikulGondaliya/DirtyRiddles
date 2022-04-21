using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] private UIQuestionsMenu m_UIQuestionMenu;
    [SerializeField] private UIAnswerMenu m_UIAnswerMenu;
    [SerializeField] private UILevelCompleteScreen m_UILevelCompleteScreen;
    [SerializeField] private UIWatchVideo m_UIWatchVideo = null;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private GameObject m_HintItemTemplate;
    [SerializeField] private AudioClip m_SetHintClip = null;
    [SerializeField] private UIItem uIItem;

    private int mCurrentAnswerSlot = 0;
    private string[] mAnswerCharArray;
    private string[] mSelectedAnswerArray;
    private string[] mOptionsArray;
    private float mLastInteractionTime;
    public int pLevelIndex { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        m_UIQuestionMenu.OnOptionClick += OnOptionClick;
        m_UIAnswerMenu.OnUndoClicked += OnUndoClicked;
        m_UIAnswerMenu.OnHintClicked += OnHintClicked;
    }

    private void OnOptionClick(UIItem optionItem)
    {
        mLastInteractionTime = 0;
        int index = 0;
        while (!string.IsNullOrEmpty(mSelectedAnswerArray[index]) || mSelectedAnswerArray[index] == "-")
            index++;

        if(index <= mAnswerCharArray.Length - 1)
        {
            UIItem answerSlotItem = m_UIAnswerMenu.FindItem(index);
            optionItem.transform.parent = answerSlotItem.transform;
            optionItem.pRectTransform.anchoredPosition = Vector3.zero;
            optionItem.pRectTransform.sizeDelta = new Vector2(80, 80);
            mSelectedAnswerArray[index] = optionItem.pText;
        }

        if(index == mAnswerCharArray.Length - 1)
        {
           for(int i = 0; i < mAnswerCharArray.Length; i++)
           {
                if (mAnswerCharArray[i] == mSelectedAnswerArray[i])
                    continue;
                else
                {
                    OnFilledWithWrongAnswers();
                    return;
                }
           }

            m_GameManager.OnLevelComplete();
        }
    }

    public void SetAnswer(string[] answerCharArray)
    {
        mAnswerCharArray = answerCharArray;
        mSelectedAnswerArray = new string[mAnswerCharArray.Length];
        for(int i = 0; i < mAnswerCharArray.Length; i++)
        {
            if (mAnswerCharArray[i] == "-")
                mSelectedAnswerArray[i] = "-";
        }
    }

    public void SetOptionsArray(string[] optionsArray)
    {
        mOptionsArray = optionsArray;
    }

    private void OnUndoClicked()
    {
        mLastInteractionTime = 0;
        for (int i = mAnswerCharArray.Length - 1; i >=0; i--)
        {
            UIItem parentItem = m_UIAnswerMenu.FindItem(i);
            if(parentItem.transform.childCount > 0 && parentItem.name != "Space")
            {
                Transform child = parentItem.transform.GetChild(0);
                if(child.name != "Revealed" )
                {
                    UIItem childItem = child.GetComponent<UIItem>();
                    int optionIndex = (int)childItem.pData;
                    UIItem optionSlotItem = m_UIQuestionMenu.FindItem(optionIndex);
                    child.transform.parent = optionSlotItem.transform;
                    childItem.pRectTransform.anchoredPosition = Vector3.zero;
                    childItem.pRectTransform.sizeDelta = new Vector2(130, 130);
                    childItem._Background.color = Color.white;
                    childItem._Text.color = Color.black;
                    mSelectedAnswerArray[i] = null;
                    break;
                }
            }
        }
    }

    private void OnHintClicked()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        if (coins < 50)
        {
            m_UIWatchVideo.SetVisibility(true);
            AdManager.instance.HideAdmobBanner();
            return;
        }
        coins -= 50;
        PlayerPrefs.SetInt("Coins", coins);
        print("Coins == " + coins);
        uIItem.pText = coins .ToString();
        mLastInteractionTime = 0;
        
        //Get Hint to be placed
        for (int i = 0; i < mSelectedAnswerArray.Length; i++)
        {
            if(string.IsNullOrEmpty(mSelectedAnswerArray[i]))
            {
                UIItem answerSlotItem = m_UIAnswerMenu.FindItem(i);
                GameObject go = Instantiate(m_HintItemTemplate, answerSlotItem.transform);
                UIItem hintItem = go.GetComponent<UIItem>();
                hintItem.pRectTransform.anchoredPosition = Vector3.zero;
                hintItem.pRectTransform.sizeDelta = new Vector2(80, 80);
                hintItem.pText = mSelectedAnswerArray[i] = mAnswerCharArray[i];
                go.name = "Revealed";
                SetHintItem(mAnswerCharArray[i]);
                SoundManager.Play(m_SetHintClip, SoundType.OneShotSFX);
                if (i == mAnswerCharArray.Length - 1)
                {
                    for (int j = 0; j < mAnswerCharArray.Length; j++)
                    {
                        if (mAnswerCharArray[j] == mSelectedAnswerArray[j])
                            continue;
                        else
                        {
                            OnFilledWithWrongAnswers();
                            return;
                        }
                    }

                    m_GameManager.OnLevelComplete();
                }
                break;
            }
        }
    }

    public void SetHintItem(string str)
    {
        int index = System.Array.FindIndex(mOptionsArray, x => x == str);
        UIItem slotItem = m_UIQuestionMenu.FindItem(index);
        Transform child = slotItem.transform.GetChild(0);
        UIItem childUiItem = child.GetComponent<UIItem>();
        childUiItem._Background.color = Color.green;
        childUiItem._Text.color = Color.white;
        childUiItem.pState = State.NON_INTERACTIVE;
    }

    private void OnFilledWithWrongAnswers()
    {
        for (int i = 0; i < mAnswerCharArray.Length; i++)
        {
            UIItem parentItem = m_UIAnswerMenu.FindItem(i);
            if (parentItem.transform.childCount > 0)
            {
                Transform child = parentItem.transform.GetChild(0);
                if (child.name != "Revealed")
                {
                    UIItem childItem = child.GetComponent<UIItem>();
                    childItem._Background.color = Color.red;
                    childItem._Text.color = Color.white;
                }
            }
        }

        m_GameManager.OnlevelFailed();
        m_UIAnswerMenu.AnimateHint();
    }

    private void Update()
    {
        if(m_UIAnswerMenu._Visible)
        {
            mLastInteractionTime += Time.deltaTime;
            if (mLastInteractionTime >= 10)
            {
                m_UIAnswerMenu.AnimateHint();
                mLastInteractionTime = 0;
            }
              
        }
    }
}
