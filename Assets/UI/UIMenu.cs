using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIScreen
{
    [SerializeField] private UIItem m_Template = null;
    [SerializeField] private Transform m_Content = null;
    [SerializeField] private bool m_HorizontalScroll = true;
    [SerializeField] private ScrollRect m_ScrollRect = null;
    [SerializeField] private RectTransform m_ScrollViewRectTransform = null;
    [SerializeField] private int m_FixedConstrainCount = 1;

    public int _TotalItems;

    private List<UIItem> mMenuItemPoolList = new List<UIItem>();
    private List<UIItem> mMenuVisibleItemList = new List<UIItem>();
    private int mRows;
    private int mColumns;
    private Graphic gr;

    public UIItem AddItem(string name)
    {
        
        UIItem uiItem = null;
        if(mMenuItemPoolList.Count > 0)
        {
            uiItem = mMenuItemPoolList[0];
            mMenuItemPoolList.RemoveAt(0);
        }
        else
        {
            GameObject go = Instantiate(m_Template.gameObject);
            uiItem = go.GetComponent<UIItem>();
        }

        uiItem.transform.SetParent(m_Content);
        uiItem.transform.localScale = Vector3.one;
        mMenuVisibleItemList.Add(uiItem);
        mRows = Mathf.CeilToInt(mMenuVisibleItemList.Count / m_FixedConstrainCount);
        return uiItem;
    }

    public UIItem FindItem(int itemIndex)
    {
        if (itemIndex > mMenuVisibleItemList.Count - 1 || itemIndex < 0)
            return null;

        return mMenuVisibleItemList[itemIndex];
    }

    public int FindItemIndex(UIItem item)
    {
        for(int i = 0; i < m_Content.childCount; i++)
        {
            UIItem childItem = m_Content.GetChild(i).GetComponent<UIItem>();
            if (childItem.Equals(item))
                return i;
        }

        return -1;
    }

    public void RemoveItem(UIItem uiItem, bool destroy = true)
    {
        if(mMenuVisibleItemList.Contains(uiItem))
        {
            mMenuVisibleItemList.Remove(uiItem);

            if (destroy)
                Destroy(uiItem.gameObject);
        }
    }

    public virtual void OnScrollValueChange()
    {

        
    }

    private bool IsItemVisibleInMenu(UIItem item)
    {
        Vector2 size = new Vector2(m_ScrollViewRectTransform.rect.xMax * 2, m_ScrollViewRectTransform.rect.yMax * 2);
        Bounds scrollRectBound = new Bounds(new Vector3(m_ScrollViewRectTransform.position.x, m_ScrollViewRectTransform.position.y, 0), size);
        Bounds imageRectBound = new Bounds(new Vector3(item.pRectTransform.position.x, item.pRectTransform.position.y, 0), new Vector3(item.pRectTransform.rect.xMax * 2, item.pRectTransform.rect.yMax * 2, 0));
        return scrollRectBound.Intersects(imageRectBound);
    }

    public virtual void Clear()
    {
        for(int i = 0; i < m_Content.childCount; i++)
        {
            Destroy(m_Content.GetChild(i).gameObject);
        }

        mMenuVisibleItemList.Clear();
    }
}
