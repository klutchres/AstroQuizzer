using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public Page activePage;
    public Page[] Pages;
    void Start()
    {
        SwitchPage(activePage);
    }

    public void SwitchPage(Page page)
    {
        activePage = page;
        RefreshPages();
    }

    public void SwitchPage(string id)
    {
        foreach (var p in Pages)
        {
            if (p.id == id) activePage = p;
            RefreshPages();
        }
        Debug.Log($"Page: {id}");
        AudioManager.instance.ClickSFX();
    }

    public void SwitchPage(int position)
    {
        activePage = Pages[position];
        RefreshPages();
    }

    void RefreshPages() { foreach (var p in Pages) { p.active = (activePage == p); p.gameObject.SetActive(p.active); } }
}
