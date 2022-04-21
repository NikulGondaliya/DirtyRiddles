using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIMainMenu m_UIMainMenu = null;
    [SerializeField] private UILevelSelectionMenu m_UILevelSelectionMenu = null;
    [SerializeField] private QuestionManager m_QuestionManager;
    [SerializeField] private UIQuestionsMenu m_UIQuestionMenu;
    [SerializeField] private UIAnswerMenu m_UIAnswerMenu;
    [SerializeField] private UILevelCompleteScreen m_UILevelCompleteScreen;
    [SerializeField] private UIGamePlay m_UIGamePlay;
    [SerializeField] private UIButton m_OptionTemplate;
    [SerializeField] private AnswerManager m_AnswerManager;
    [SerializeField] private TextAsset m_LevelJson;
    [SerializeField] private AudioClip[] m_LevelCompleteVOs;
    [SerializeField] private AudioClip[] m_LevelFailedVos;
    [SerializeField] private AdManager m_AdsManager;

    private int mCurrentLevelIndex;
    private string[] mAnswerCharArray;

    // Start is called before the first frame update
    void Start()
    {
        m_UIMainMenu.OnPlayButtonClicked += OnPlayButtonClicked;
        m_UIMainMenu.OnMenuButtonClicked += OnMenuButtonClicked;
        m_UILevelSelectionMenu.OnLevelSelected += OnLevelSelected;
        m_UILevelCompleteScreen.OnLevelCompletePlayClicked += OnLevelCompletePlayClicked;
        m_UILevelCompleteScreen.OnLevelCompleteMenuClicked += OnLevelCompleteMenuClicked;
        m_UIGamePlay.ExitCallback += ExitFromGamePlay;
        m_UIGamePlay.OpenStoreCallback += OpenStore;
        LevelInfo.Init(m_LevelJson.text);
        if(PlayerPrefs.GetInt("FirstTimePlay", 0) != 0)
        {
            //AdManager.instance.RequestInterstitial();
            StartCoroutine(ShowFullScreenAd());
        }

        PlayerPrefs.SetInt("FirstTimePlay", 1);
    }

    IEnumerator ShowFullScreenAd()
    {
        yield return new WaitForSeconds(1);

        AdManager.instance.ShowAdmobInterstitial();
    }

    IEnumerator ShowBannerWithDelay()
    {
        yield return new WaitForSeconds(1);

        AdManager.instance.ShowAdmobBanner();
    }

    public void ShowQuestion(int index)
    {
        //AdManager.instance.DestroyFullScreenAd();
        //AdManager.instance.RequestInterstitial();
        StartCoroutine(ShowBannerWithDelay());
        m_UIQuestionMenu.Clear();
        m_UIAnswerMenu.Clear();

        LevelInfo questionInfo = LevelInfo.GetLevelInfo(index); //m_QuestionManager.GetQuestion(0);

        List<string> optionsList = new List<string>();
        
        mAnswerCharArray = questionInfo.AnswerWords[0].Split(',');
        for (int i = 0; i < mAnswerCharArray.Length; i++)
        {
            if (mAnswerCharArray[i] != "")
            {
                mAnswerCharArray[i] = mAnswerCharArray[i].ToUpper();
                optionsList.Add(mAnswerCharArray[i]);
            }               
            else
                mAnswerCharArray[i] = "-";          
        }

        int takeExtra = questionInfo.GridSize * 2 - optionsList.Count;
        string[] extraCharArray = questionInfo.Extra.Split(',');
        Shuffle(extraCharArray);
        for (int i = 0; i < takeExtra; i++)
        {
            optionsList.Add(extraCharArray[i].ToUpper());
        }

        Shuffle(optionsList);
        m_UIQuestionMenu.SetGridColumnCount(questionInfo.GridSize);

        for(int i = 0; i < optionsList.Count; i++)
        {
           UIItem slotItem = m_UIQuestionMenu.AddItem(optionsList[i]);
           GameObject go = Instantiate(m_OptionTemplate.gameObject, slotItem.transform);
           UIItem optionItem = go.GetComponent<UIItem>();
           optionItem.pText = optionsList[i];
           optionItem.pRectTransform.sizeDelta = new Vector2(130, 130);
           optionItem.pData = i;
        }

        m_UIQuestionMenu.SetQuestionText(questionInfo.Riddle);

        //Set Answer Menu
        for (int i = 0; i < mAnswerCharArray.Length; i++)
        {
           UIItem slotItem = m_UIAnswerMenu.AddItem(mAnswerCharArray[i]);
            if (mAnswerCharArray[i] == "-")
                slotItem.name = "Space";
        }
           

        m_UIAnswerMenu.gameObject.SetActive(false);
        StartCoroutine(ShowAnswerMenu());
        m_AnswerManager.SetAnswer(mAnswerCharArray);
        m_AnswerManager.SetOptionsArray(optionsList.ToArray());
        m_AnswerManager.pLevelIndex = index;
        m_UIQuestionMenu.SetVisibility(true);
        m_UIAnswerMenu.SetVisibility(true);
        m_UIGamePlay.SetVisibility(true);
        m_UIGamePlay.SetLevelNumber(index + 1);
    }

    IEnumerator ShowAnswerMenu()
    {
        yield return new WaitForEndOfFrame();

        m_UIAnswerMenu.gameObject.SetActive(true);
    }

    private void Shuffle(List<string> str)
    {
        System.Random rng = new System.Random();
        int n = str.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = str[k];
            str[k] = str[n];
            str[n] = value;
        }
    }

    private void Shuffle(string[] charArray)
    {
        System.Random rng = new System.Random();
        int n = charArray.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = charArray[k];
            charArray[k] = charArray[n];
            charArray[n] = value;
        }
    }

    private void OnPlayButtonClicked()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        mCurrentLevelIndex = currentLevel;
        ShowQuestion(currentLevel);
        m_UIMainMenu.SetVisibility(false);
        m_UIQuestionMenu.SetVisibility(true);
        m_UIAnswerMenu.SetVisibility(true);
    }

    private void OnMenuButtonClicked()
    {
        m_UIMainMenu.SetVisibility(false);
        m_UILevelSelectionMenu.SetVisibility(true);
        m_UILevelSelectionMenu.SetUpMenu(LevelInfo.GetLevelCount());
    }

    private void OnLevelSelected(int levelIndex)
    {
        mCurrentLevelIndex = levelIndex;
        ShowQuestion(levelIndex);
        m_UILevelSelectionMenu.SetVisibility(false);
        m_UIMainMenu.SetVisibility(false);
        m_UIQuestionMenu.SetVisibility(true);
        m_UIAnswerMenu.SetVisibility(true);
    }

    public void OnLevelComplete()
    {
       int levelToComplete = PlayerPrefs.GetInt("CurrentLevel", 0);
       if(mCurrentLevelIndex == levelToComplete)
            PlayerPrefs.SetInt("CurrentLevel", levelToComplete + 1);

        AdManager.instance.ShowAdmobInterstitial();
        int i = Random.Range(0, m_LevelCompleteVOs.Length);
        SoundManager.Play(m_LevelCompleteVOs[i], SoundType.OneShotSFX);
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coins += 10;
        PlayerPrefs.SetInt("Coins", coins);
        StartCoroutine(ShowLevelCompleteScreen());
    }

    IEnumerator ShowLevelCompleteScreen()
    {
        yield return new WaitForSeconds(1);

        m_UIAnswerMenu.SetVisibility(false);
        m_UIQuestionMenu.SetVisibility(false);
        m_UIGamePlay.SetVisibility(false);
        m_UILevelCompleteScreen.SetVisibility(true);
        m_UILevelCompleteScreen.ShowData(mCurrentLevelIndex + 1, mAnswerCharArray);
    }

    public void OnlevelFailed()
    {
        int i = Random.Range(0, m_LevelFailedVos.Length);
        SoundManager.Play(m_LevelFailedVos[i], SoundType.OneShotSFX);
    }

    private void OnLevelCompletePlayClicked()
    {
        int levelToComplete = PlayerPrefs.GetInt("CurrentLevel");
        if (mCurrentLevelIndex <= levelToComplete)
        {
            mCurrentLevelIndex++;
            ShowQuestion(mCurrentLevelIndex);
        }
        else
            ShowQuestion(levelToComplete);
        
        m_UILevelCompleteScreen.SetVisibility(false);
    }

    private void OnLevelCompleteMenuClicked()
    {
        m_UILevelCompleteScreen.SetVisibility(false);
        m_UILevelSelectionMenu.SetVisibility(true);
        m_UILevelSelectionMenu.SetUpMenu(LevelInfo.GetLevelCount());
    }

    private void ExitFromGamePlay()
    {

    }

    private void OpenStore()
    {

    }

}
