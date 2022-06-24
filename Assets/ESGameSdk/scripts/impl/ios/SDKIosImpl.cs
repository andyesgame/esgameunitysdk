using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if PLATFORM_IOS
public class SDKIosImpl : ISDK
{
    [DllImport("__Internal")]
    private static extern void _login();
    [DllImport("__Internal")]
    private static extern void _startInGameMain();
    [DllImport("__Internal")]
    private static extern void _setFloatingButtonEnable(bool floatingButtonEnable);
    [DllImport("__Internal")]
    private static extern void _billing(string productId, string serverId, string playerId, string extra);
    [DllImport("__Internal")]
    private static extern void _setProducts(string productString);
    [DllImport("__Internal")]
    private static extern void _checkSession();
    [DllImport("__Internal")]
    private static extern void _logout();
    [DllImport("__Internal")]
    private static extern void _config(string appsflyerDevKey, string clientId, string clientSecret, string ggId, string appleId);
    [DllImport("__Internal")]
    private static extern void _trackingEvent(string eventName, string data);
    [DllImport("__Internal")]
    private static extern void _setDeviceManagement(string deviceId);
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
        setConfig(config.appFlyerKey, Int32.Parse(config.clientIdIos), config.clientSecretIos, config.ggClientIdIos);
        initProducts();
        if (ESGameSDK.useIosUnityDeviceId)
            setDeviceManagement(SystemInfo.deviceUniqueIdentifier);
    }
    public void trackingEvent(string eventName, Dictionary<string, object> prs)
    {
        _trackingEvent( eventName, DictHelper.toJsonString(prs));
    }
    public void initProducts()
    {
        
        List<string> products = getProducts();
        Debug.Log("product "+ products.Count);
        if(products!=null && products.Count > 0)
        {
            string productString = "";
            for(var i = 0; i< products.Count; i++)
            {
                
                productString += products[i];
                if (i < products.Count - 1)
                {
                    productString += ",";
                }
            }
            Debug.Log("product s " + productString);
            _setProducts(productString);
        }
        
    }
    public void billing(string productId, string serverId, string playerId, string extra)
    {
        _billing(productId, serverId, playerId, extra);
    }

    public void checkSession(bool requireConfig = true)
    {
        _checkSession();
    }


    public void webBilling(string productId, string serverId, string playerId, string extra)
    {
        throw new NotSupportedException();
    }

    public List<string> getProducts()
    {
        List<string> rs = new List<string>();
        if (productConfig.iosComsumables != null && productConfig.iosComsumables.Count > 0)
        {
            rs.AddRange(productConfig.iosComsumables);
        }
        if (productConfig.iosSubscriptions != null && productConfig.iosSubscriptions.Count > 0)
        {
            rs.AddRange(productConfig.iosSubscriptions);
        }
        return rs;
    }

    public List<string> getWebProducts()
    {
        return productConfig.webs;
    }

    public void logOut()
    {
        _logout();
    }

    public void login()
    {
        Debug.Log("call Login");
        _login();
    }

    public ESUser getCurrentUser()
    {
        return usr;
    }

    public void startInGameMain()
    {
        if (usr != null)
        {
            _startInGameMain();
        }
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
        gameSDK.billingWebEvent?.Invoke(new ESBillingWebResult(ev.itemId, ev.price));
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
        _config(appsflyerDevKey, clientId+"", clientSecret, ggId,config.appleId);
    }

    public void setFloatingButtonEnable(bool floatingButtonEnable)
    {
        _setFloatingButtonEnable(floatingButtonEnable);
    }

    public void setDeviceManagement(string deviceId)
    {
        _setDeviceManagement(deviceId);
    }
}
#endif
