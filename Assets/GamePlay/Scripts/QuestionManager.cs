using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private LevelInfo[] m_QuestionsInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public LevelInfo GetQuestion(int index)
    {
        return m_QuestionsInfo[index];
    }
}
