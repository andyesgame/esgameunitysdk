using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMain : BaseInGame
{
    public Text name;
    public Text id;
    public GameObject updateInfoView;
    public GameObject upgradeAccView;

    private void OnEnable()
    {
        var usr = ESGameSDK.instance.getCurrentUser();
        if(usr == null)
        {
            return;
        }
        name.text = usr.name;
        id.text = "ID: "+usr.id + "";
        if (usr.provider.Equals("app_device"))
        {
            updateInfoView.SetActive(false);
            upgradeAccView.SetActive(true);
        }
        else
        {
            updateInfoView.SetActive(true);
            upgradeAccView.SetActive(false);
        }
    }


    public void showEventWeb()
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showWebView(((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).GetESConfig().url_news_sdk, "Tin Tức");
    }
}
