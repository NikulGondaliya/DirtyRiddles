using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIAnswerMenu : UIMenu
{

    [SerializeField] private UIButton m_BtnUndo = null;
    [SerializeField] private UIButton m_BtnHint = null;
    [SerializeField] private Animator m_HintAnimator = null;

    public Action OnUndoClicked = null;
    public Action OnHintClicked = null;

    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        if (item.name == m_BtnUndo.name)
            OnUndoClicked?.Invoke();
        else if (item.name == m_BtnHint.name)
        {
            OnHintClicked?.Invoke();
        }

    }

    public void AnimateHint()
    {
        m_HintAnimator.SetTrigger("Shake");
    }
}
