using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;

public class UILevelCompleteScreen : UIScreen
{
    [SerializeField] private Text m_TxtLevel;
    [SerializeField] private Text m_TxtAnswer;
    [SerializeField] private AudioClip m_ShowSFX;

    public Action OnLevelCompletePlayClicked = null;
    public Action OnLevelCompleteMenuClicked = null;

    public void ShowData(int level, string[] answerArray)
    {
        m_TxtLevel.text = "Level " + level.ToString();

        string answer = "";
        for(int i = 0; i < answerArray.Length; i++)
        {
            answer += answerArray[i];
        }

        m_TxtAnswer.text = answer;
    }

    public override void SetVisibility(bool isVisible, bool playAnim = true)
    {
        base.SetVisibility(isVisible, playAnim);

        if(isVisible)
            SoundManager.Play(m_ShowSFX, SoundType.OneShotSFX);
    }

    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if (item.name == "BtnPlay")
            OnLevelCompletePlayClicked?.Invoke();
        else if (item.name == "BtnMenu")
            OnLevelCompleteMenuClicked?.Invoke();
        else if (item.name == "BtnWhatsapp")
            StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        Debug.Log(Application.temporaryCachePath);
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        /*new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("Hello world!")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();*/

        // Share on WhatsApp only, if installed (Android only)
        if( NativeShare.TargetExists( "com.whatsapp" ) )
        	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
}
