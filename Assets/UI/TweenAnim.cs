using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimVector
{
    public LeanTweenType _TweenType = LeanTweenType.notUsed;
    public Vector3 _StartVector;
    public Vector3 _EndVector;
    public float _Delay;
    public float _Duration;
}
public class TweenAnim : MonoBehaviour
{
    [SerializeField] private AnimVector m_EntryAnimMoveVector;
    [SerializeField] private AnimVector m_ExitAnimMoveVector;
    [SerializeField] private AnimVector m_EntryAnimScaleVector;
    [SerializeField] private AnimVector m_ExitAnimScaleVector;

    private RectTransform mRectTransform;
    void Start()
    {
        
    }

    public void PlayEnrtyAnim()
    {
       if(m_EntryAnimMoveVector._TweenType != LeanTweenType.notUsed)
        {
            if(mRectTransform == null)
                mRectTransform = GetComponent<RectTransform>();

            mRectTransform.anchoredPosition = m_EntryAnimMoveVector._StartVector;
            LeanTween.moveLocal(gameObject, m_EntryAnimMoveVector._EndVector, m_EntryAnimMoveVector._Duration)
                .setDelay(m_EntryAnimMoveVector._Delay)
                .setEase(m_EntryAnimMoveVector._TweenType);
        }

       if(m_EntryAnimScaleVector._TweenType != LeanTweenType.notUsed)
        {

            transform.localScale = m_EntryAnimScaleVector._StartVector;
            LeanTween.scale(gameObject, m_EntryAnimScaleVector._EndVector, m_EntryAnimScaleVector._Duration)
                .setDelay(m_EntryAnimScaleVector._Delay)
                .setEase(m_EntryAnimScaleVector._TweenType);
        }
    }

    public void PlayExitAnim()
    {
        if (m_ExitAnimMoveVector._TweenType != LeanTweenType.notUsed)
        {
            if (mRectTransform == null)
                mRectTransform = GetComponent<RectTransform>();

            mRectTransform.anchoredPosition = m_ExitAnimMoveVector._StartVector;
            LeanTween.moveLocal(gameObject, m_ExitAnimMoveVector._EndVector, m_ExitAnimMoveVector._Duration)
                .setDelay(m_ExitAnimMoveVector._Delay)
                .setEase(m_ExitAnimMoveVector._TweenType);
        }

        if (m_ExitAnimScaleVector._TweenType != LeanTweenType.notUsed)
        {

            transform.localScale = m_ExitAnimScaleVector._StartVector;
            LeanTween.scale(gameObject, m_ExitAnimScaleVector._EndVector, m_ExitAnimScaleVector._Duration)
                .setDelay(m_ExitAnimScaleVector._Delay)
                .setEase(m_ExitAnimScaleVector._TweenType);
        }
    }
}
