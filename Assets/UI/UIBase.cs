using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public enum State
{
    NON_INTERACTIVE,
    INTERACTIVE,
    DISABLED,
}
public class UIBase : MonoBehaviour 
{

	public bool _Visible;
	public State _State;

	protected List<UIBase> mChildUIItem = new List<UIBase> ();
	protected bool mVisible;
	protected State mState;
	protected delegate void OnStateChange();
	protected OnStateChange onStateChage;
	protected delegate void OnVisibilityChange();
	protected OnVisibilityChange onVisibilityChange;
    protected RectTransform mRectTransform;

    [SerializeField] private Animator m_Animator = null;
    [SerializeField] private string m_EntryAnimState = "EntryAnim";
    [SerializeField] private string m_ExitAnimState = null;

    public RectTransform pRectTransform
    {
        get
        {
            if (mRectTransform == null)
                mRectTransform = GetComponent<RectTransform>();

            return mRectTransform;
        }
    }

    public virtual State pState
	{
		get{ return mState; }

		set
		{
			if (mState != value) 
			{
				_State = mState = value;
				if (onStateChage != null)
					onStateChage();
			}
		}
	}

	// Use this for initialization
	protected virtual void Start ()
	{
		pState = _State;
		UIBase[] uiBase = GetComponentsInChildren<UIBase> ();
		for (int i = 0; i < uiBase.Length; i++)
			AddUIItem (uiBase[i]);        

		SetVisibility(_Visible);
        if (onVisibilityChange != null)
            onVisibilityChange();
    }

	protected virtual void AddUIItem(UIBase uiItem)
	{
        if (uiItem == this)
            return;

		mChildUIItem.Add (uiItem);
	}

	protected virtual void Update()
	{
		pState = _State;
	}
    public virtual void SetVisibility(bool isVisible, bool playAnim = true)
	{
         if (_Visible != isVisible)
         {
            _Visible = isVisible;
            if (onVisibilityChange != null)
                onVisibilityChange();
         }

        if(playAnim)
        {
            TweenAnim tweenAnim = GetComponent<TweenAnim>();

            if (tweenAnim != null)
            {
                if (isVisible)
                    tweenAnim.PlayEnrtyAnim();
                else
                    tweenAnim.PlayExitAnim();
            }
        }

    }

    public virtual void TweenAndHide(float hideDelay)
    {
        TweenAnim tweenAnim = GetComponent<TweenAnim>();

        if (tweenAnim != null)
        {
           tweenAnim.PlayExitAnim();
            StartCoroutine(HideOnDelay(hideDelay));
        }

    }

    IEnumerator HideOnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetVisibility(false);
    }
    
}
