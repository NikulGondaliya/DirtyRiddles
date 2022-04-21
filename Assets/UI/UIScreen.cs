using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScreen : UIBase
{
    [SerializeField] protected UIScreen m_ParentUIScreen = null;
    private Canvas mCanvas;
    private GraphicRaycaster mGraphicRaycaster;
    protected Canvas pCanvas
    {
        get
        {
            if (mCanvas == null)
                mCanvas = GetComponent<Canvas>();

            return mCanvas;
        }
    }


    protected virtual void Awake()
	{
        onStateChage += OnScreenStateChange;
		onVisibilityChange += OnUIScreenVisibilityChange;
	}

    protected override void Start()
    {
        mGraphicRaycaster = GetComponent<GraphicRaycaster>();

        base.Start();
    }

    public virtual void OnClick(UIItem item, PointerEventData pointerEventData)
	{

	}
    public virtual void OnToggleButtonClick(UIToggleButton toggleButton, PointerEventData pointerEventData)
    {

    }

	protected virtual void OnUIScreenVisibilityChange()
	{
        pCanvas.enabled = _Visible;
        if (mGraphicRaycaster != null)
            mGraphicRaycaster.enabled = _Visible;
        foreach (UIBase uibase in mChildUIItem)
            uibase.SetVisibility(_Visible);
    }

    protected virtual void OnScreenStateChange()
    {
        if (mGraphicRaycaster != null)
        {
            if (pState == State.INTERACTIVE)
                mGraphicRaycaster.enabled = true;
            else
                mGraphicRaycaster.enabled = false;
        }
    }
}
