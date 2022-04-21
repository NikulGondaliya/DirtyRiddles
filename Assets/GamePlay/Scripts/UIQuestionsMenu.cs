using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIQuestionsMenu : UIMenu
{
    [SerializeField] private QuestionManager m_QuestionManager;
    [SerializeField] private GridLayoutGroup m_GridLayout;
    [SerializeField] private Text m_TxtQuestion;

    public Action<UIItem> OnOptionClick = null;
    private string[] mOptionsArray;

    // Start is called before the first frame update
    public void SetGridColumnCount(int column)
    {
        m_GridLayout.constraintCount = column;
    }

    public void SetQuestionText(string question)
    {
        m_TxtQuestion.text = question;
    }

    public override void OnClick(UIItem item, PointerEventData pointerEventData)
    {
        base.OnClick(item, pointerEventData);

        OnOptionClick?.Invoke(item);
    }
}
