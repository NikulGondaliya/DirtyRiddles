using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScalableButton : UIButton
{
    [SerializeField] private Transform mParent;
    [SerializeField] private GameObject m_ScableTemplate;

    private GameObject mScalableButton;
    protected override void OnPointerEnter(PointerEventData pointerEventData)
    {
       /* mScalableButton = Instantiate(m_ScableTemplate, mParent);
        mScalableButton.transform.position = transform.position;
        UIItem uiItem = mScalableButton.GetComponent<UIItem>();
        uiItem.pState = State.NON_INTERACTIVE;
        uiItem.pRectTransform.sizeDelta = new Vector2(170, 170);
        uiItem.pText = pText;
        uiItem._Background.sprite = _Background.sprite;*/

        base.OnPointerEnter(pointerEventData);

    }

    protected override void OnPointerExit(PointerEventData pointerEventData)
    {
        base.OnPointerExit(pointerEventData);

       /* if (mScalableButton != null)
            Destroy(mScalableButton);*/
    }
}
