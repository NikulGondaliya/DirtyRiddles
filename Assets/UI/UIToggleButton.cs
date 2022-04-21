using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIToggleButton : UIItem
{
    public Toggle pToggle { get; private set; }
    protected override void Start()
    {
        base.Start();

        pToggle = GetComponent<Toggle>();
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pState == State.DISABLED || pState == State.NON_INTERACTIVE)
            return;

        foreach (UIScreen uiScreen in mParentUI)
            uiScreen.OnToggleButtonClick(this, pointerEventData);
    }

    protected override void SetDisabled()
    {
        base.SetDisabled();

      //  _Background.sprite = _DisabledImage;
    }
}
