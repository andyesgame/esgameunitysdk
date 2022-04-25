using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESInGameContainer : MonoBehaviour
{
    public List<GameObject> views;
    private GameObject current;
    public ESFloatingButton floatingView;

    public void onClickFloatingView()
    {
        Debug.Log("onClickFloatingView ");
        if (!floatingView.showFullDisplayIfNeed())
        {
            show(0);
        }
    }

    public void show(int index = 0)
    {
        Debug.LogError("ESInGameContainer show "+ index +" "+views.Count);
        if(index < views.Count)
        {
            this.gameObject.SetActive(true);
            if(current!=null && views[index] != current)
            {
                current.SetActive(false);
            }

            views[index].SetActive(true);
            current = views[index];

        }
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showFloatingView(false);
    }

    internal void hide()
    {
        this.gameObject.SetActive(false);
        if (ESGameSDK.instance.getCurrentUser() != null)
        {
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showFloatingView(true);
        }
    }

    public void showSupportPage()
    {
        var url = "https://help.esgame.vn/?token=" + ESUserDataManager.getAccesstoken();
        Debug.Log("showSupportPage "+ url);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showWebView(url, "Báo Lỗi");
    }
}
