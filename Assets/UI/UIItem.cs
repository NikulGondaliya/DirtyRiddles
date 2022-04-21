using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : UIBase 
{
	public Image _Background;
	public Text _Text;
    public object pData { get; set; }

	protected UIScreen[] mParentUI;

	public string pText
	{
		get
		{
			if (_Text != null)
				return _Text.text;
			
			return null;
		}
		set 
		{
			if (_Text != null)
				_Text.text = value;
		}
	}

	protected virtual void Awake()
	{
		onStateChage += OnItemStateChange;
		onVisibilityChange += OnItemVisibilityChange;
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();

		mParentUI = GetComponentsInParent<UIScreen> ();
	}

	protected virtual void OnItemStateChange()
	{
		if (mState == State.INTERACTIVE)
			SetInteractive ();
		else if (mState == State.NON_INTERACTIVE)
			SetNonIteractive ();
		else
			SetDisabled ();
	}

	protected virtual void SetInteractive()
	{
		if(_Background != null)
			_Background.raycastTarget = true;
		
		if(_Text != null)
			_Text.raycastTarget = true;
	}

	protected virtual void SetNonIteractive()
	{
		if(_Background != null)
			_Background.raycastTarget = false;
		
		if(_Text != null)
			_Text.raycastTarget = false;
	}

	protected virtual void SetDisabled()
	{
		if(_Background != null)
			_Background.raycastTarget = false;
		
		if(_Text != null)
			_Text.raycastTarget = false;
	}

    protected virtual void OnItemVisibilityChange()
    {
        //gameObject.SetActive (_Visible);
        if (_Background != null)
            _Background.enabled = _Visible;

        if (_Text != null)
            _Text.enabled = _Visible;

        UIItem[] childItems = transform.GetComponentsInChildren<UIItem>();
        foreach (UIItem uiItem in childItems)
            uiItem.SetVisibility(_Visible);
    }
}
