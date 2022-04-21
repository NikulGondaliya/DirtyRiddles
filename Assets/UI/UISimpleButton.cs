using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UISimpleButton : UIItem
{
    [SerializeField] private Button m_Button;
    [SerializeField] private AudioClip m_ClickClip;
    public Action<UIItem> OnClickCallback = null;

    protected override void Start()
    {
        base.Start();

        m_Button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        m_Button.onClick.RemoveListener(OnClick);
        OnClickCallback = null;
    }

    private void OnClick()
    {
        OnClickCallback?.Invoke(this);
        SoundManager.Play(m_ClickClip, SoundType.OneShotSFX);
    }
}
