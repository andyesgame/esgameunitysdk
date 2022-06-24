using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if PLATFORM_ANDROID
public class SDKAndroidImpl : ISDK
{
    public ESGameSDK gameSDK;
  

    public ESLoginSuccessEvent loginSuccessEvent;
    public ESLoginFailureEvent loginFailureEvent;
    public ESLogOutEvent logOutEvent;
    public ESBillingEvent billingEvent;
    public ESWebBillingEvent billingWebEvent;
    public ESUIEvent uiEvent;
    protected ESGameConfig config;
    protected ESGameProductConfig productConfig;
    private ESUser usr;
    public void init(ESGameSDK gameSDK)
    {
        this.gameSDK = gameSDK;
        loginSuccessEvent = gameSDK.loginSuccessEvent;
        loginFailureEvent = gameSDK.loginFailureEvent;
        logOutEvent = gameSDK.logOutEvent;
        billingEvent = gameSDK.billingEvent;
        billingWebEvent = gameSDK.billingWebEvent;
        uiEvent = gameSDK.uiEvent;
        config = gameSDK.getConfig();
        productConfig = gameSDK.getProductConfig();
        setConfig(config.appFlyerKey, Int32.Parse(config.clientIdAndroid), config.clientSecretAndroid, config.ggClientIdAndroid);
        if(ESGameSDK.useAndroidUnityDeviceId)
        setDeviceManagement(SystemInfo.deviceUniqueIdentifier);
    }

    public void billing(string productId, string serverId, string playerId, string extra)
    {
        string skuType = "inapp";
        if(productConfig.ggSubscriptions!=null && productConfig.ggSubscriptions.Contains(productId))
        {
            skuType = "subs";
        }
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("billing", productId, skuType, serverId,playerId,extra);
            }
        }
    }

    public void checkSession(bool requireConfig = true)
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("checkSession");
            }
        }
    }


    public void webBilling(string productId, string serverId, string playerId, string extra)
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("billingWeb", productId, serverId, playerId, extra);
            }
        }
    }

    public List<string> getProducts()
    {
        List<string> rs = new List<string>();
        if (productConfig.ggComsumables!=null && productConfig.ggComsumables.Count > 0)
        {
            rs.AddRange(productConfig.ggComsumables);
        }
        if (productConfig.ggSubscriptions != null && productConfig.ggSubscriptions.Count > 0)
        {
            rs.AddRange(productConfig.ggSubscriptions);
        }
        return rs;
    }

    public List<string> getWebProducts()
    {
        return productConfig.webs;
    }

    public void logOut()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("logout");
            }
        }
    }

    public void login()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("login");
            }
        }
    }

    public ESUser getCurrentUser()
    {
        return usr;
    }

    public void onLoginSuccess(string data)
    {
        LoginSuccessData loginSuccessData = JsonUtility.FromJson<LoginSuccessData>(data);
        usr = loginSuccessData.user;
        gameSDK.loginSuccessEvent?.Invoke(usr);
    }

    public void onLoginFailure(string data)
    {
        ESErrorEvent ev = JsonUtility.FromJson<ESErrorEvent>(data);
        usr = null;
        gameSDK.loginFailureEvent?.Invoke(ev);
    }

    public void onLogout(string data)
    {
        gameSDK.logOutEvent?.Invoke();
    }

    public void onBillingResult(string data)
    {
        ESBillingResult ev = JsonUtility.FromJson<ESBillingResult>(data);
        gameSDK.billingEvent?.Invoke(ev);
    }

    public void onWebBillingResult(string data)
    {
        WebBillingResultData ev = JsonUtility.FromJson<WebBillingResultData>(data);
        gameSDK.billingWebEvent?.Invoke(new ESBillingWebResult(ev.itemId,ev.price));
    }

    public string getClientId()
    {
        return config.clientIdAndroid;
    }
    public string getClientSecret()
    {
        return config.clientSecretAndroid;
    }

    public void setConfig(string appsflyerDevKey, int clientId, string clientSecret, string ggId)
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("setConfig",appsflyerDevKey,clientId,clientSecret,ggId);
            }
        }
    }

    public void trackingEvent(string eventName, Dictionary<string, object> prs)
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("trackingEvent", eventName,DictHelper.toJsonString(prs));
            }
        }
    }

    public void setFloatingButtonEnable(bool floatingButtonEnable)
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("setFloatingButtonEnable", floatingButtonEnable);
            }
        }
    }

    public void setDeviceManagement(string deviceId)
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("setDeviceManagement", deviceId);
            }
        }
    }

    public void startInGameMain()
    {
        if (usr != null)
        {
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    obj_Activity.Call("startInGameMain");
                }
            }
        }
    }
}
#endif
