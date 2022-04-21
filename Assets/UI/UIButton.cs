using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : UIItem
{
	public Sprite _NormalImage;
	public Sprite _HoverImage;
	public Sprite _DisabledImage;
    public bool _UpdateTextColor = false;
	public Color _NormalTextColor = Color.white;
	public Color _HoverTextColor = Color.white;
	public Color _DisabledTextColor = Color.white;
	public AudioClip _HoverClip;
	public AudioClip _ClickClip;

	protected EventTrigger mEventTrigger;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
		trigger.triggers.Add(entry);

		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
		trigger.triggers.Add(entry);

		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
		trigger.triggers.Add(entry);
	}

    private void OnDestroy()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.RemoveListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Remove(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.RemoveListener((data) => { OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Remove(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.RemoveListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Remove(entry);
    }

    protected virtual void OnPointerClick(PointerEventData pointerEventData)
	{
        if (pState == State.DISABLED || pState == State.NON_INTERACTIVE)
            return;

        foreach (UIScreen uiScreen in mParentUI)
			uiScreen.OnClick (this, pointerEventData);

        if (_ClickClip != null)
            SoundManager.Play(_ClickClip, SoundType.OneShotSFX);
	}

	protected virtual void OnPointerEnter(PointerEventData pointerEventData)
	{
        if (pState == State.DISABLED || pState == State.NON_INTERACTIVE)
            return;

		if (_Background != null && _HoverImage != null)
			_Background.sprite = _HoverImage;
		if (_Text != null && _UpdateTextColor)
			_Text.color = _HoverTextColor;
	}

	protected virtual void OnPointerExit(PointerEventData pointerEventData)
	{
        if (pState == State.DISABLED || pState == State.NON_INTERACTIVE)
            return;

        if (_Background != null && _NormalImage != null)
			_Background.sprite = _NormalImage;
		if (_Text != null && _UpdateTextColor)
			_Text.color = _NormalTextColor;
	}

	protected override void SetDisabled ()
	{
		base.SetDisabled ();

		_Background.sprite = _DisabledImage;
	}

	protected override void OnItemVisibilityChange()
	{
		base.OnItemVisibilityChange ();

		OnPointerExit (null);
	}

    protected override void SetInteractive()
    {
        base.SetInteractive();

        if (_Background != null && _NormalImage != null)
            _Background.sprite = _NormalImage;
        if (_Text != null)
            _Text.color = _NormalTextColor;
    }
}
