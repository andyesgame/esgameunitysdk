using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ESRefreshTokenHttp;

[Serializable]
public class ESLoginSuccessEvent : UnityEvent<ESUser>
{
}
[Serializable]
public class ESLoginFailureEvent : UnityEvent<ESErrorEvent>
{
}

[Serializable]
public class ESLogOutEvent : UnityEvent
{
}

[Serializable]
public class ESBillingEvent : UnityEvent<ESBillingResult>
{
}

[Serializable]
public class ESWebBillingEvent : UnityEvent<ESBillingWebResult>
{
}

[Serializable]
public class ESDeleteAccountEvent : UnityEvent<DeleteAccountCallbackObject>
{
}

[Serializable]
public class ESUIEvent : UnityEvent<Boolean>
{
}

[Serializable]
public class ESErrorEvent
{
    public int code;
    public string message;

    public ESErrorEvent(int code, string message)
    {
        this.code = code;
        this.message = message;
    }
}

[Serializable]
public class ESBillingResult
{
    public string productId;
    public string orderId;

    public ESBillingResult(string productId, string orderId)
    {
        this.productId = productId;
        this.orderId = orderId;
    }
}

[Serializable]
public class ESBillingWebResult
{
    public string productId;
    public int price;

    public ESBillingWebResult(string productId, int price)
    {
        this.productId = productId;
        this.price = price;
    }
}
public class ESGameSDK : ConfigLoader,ISDK
{

    public static ESGameSDK instance;
    /**
     * use Unity device id method or not , only has effect in android
    * only set it to true , if the old sdk version is running 
    */
    public static bool useAndroidUnityDeviceId = false;
    /**
 * use Unity device id method or not , only has effect in ios
* only set it to true , if the old sdk version is running 
*/
    public static bool useIosUnityDeviceId = false;
    /**
     * set to false if you want to disable auto login
     */
    public static bool autoLogin = true;
    public GameObject loadingView;
    public GameObject floatingView;
    public ESContainer esGameContainer;
    public ESInGameContainer esInGameContainer;
    public PopUpMessage messagePopUp;

    public ESLoginSuccessEvent loginSuccessEvent;
    public ESLoginFailureEvent loginFailureEvent;
    public ESLogOutEvent logOutEvent;
    public ESBillingEvent billingEvent;
    public ESWebBillingEvent billingWebEvent;
    public ESDeleteAccountEvent deleteAccountEvent;
    public ESUIEvent uiEvent;
    private ISDK sdk;
    

    protected override void OnStart()
    {
        base.OnStart();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        init(this);
    }
    public void onATTrackingResult(string type)
    {
        
    }

    public void init(ESGameSDK gameSDK)
    {
        if (Application.isEditor)
        {
            sdk = new SDKDesktopImpl();
        }
        else
        {
#if PLATFORM_ANDROID
            sdk = new SDKAndroidImpl();
#elif PLATFORM_IPHONE
        sdk = new SDKIosImpl();
#else
        sdk = new SDKDesktopImpl();
#endif
        }



        sdk.init(gameSDK);
        Debug.Log("autoLogin");
        if (autoLogin)
        {
            sdk.login();
        }
    }

    public ISDK GetSDK()
    {
        return sdk;
    }

    public void checkSession(bool requireConfig = true)
    {
        sdk.checkSession(requireConfig);
    }

    public void billing(string productId, string serverId, string playerId, string extra)
    {
        sdk.billing(productId, serverId, playerId, extra);
    }

    public void webBilling(string productId, string serverId, string playerId, string extra)
    {
        sdk.webBilling(productId, serverId, playerId, extra);
    }

    public void logOut()
    {
        sdk.logOut();
        if (autoLogin)
        {
            sdk.login();
        }
    }

    public List<string> getProducts()
    {
        return sdk.getProducts();
    }

    public List<string> getWebProducts()
    {
        return sdk.getWebProducts();
    }

    public void login()
    {
        sdk.login();
    }


    public void startInGameMain() {
        sdk.startInGameMain();
    }

    public ESUser getCurrentUser()
    {
        return sdk.getCurrentUser();
    }

    public void onLoginSuccess(string data)
    {
        sdk.onLoginSuccess(data);
    }

    public void onLoginFailure(string data)
    {
        sdk.onLoginFailure(data);
    }

    public void onLogout(string data)
    {
        sdk.onLogout(data);
    }

    public void onDeleteUserCallback(string data)
    {
        sdk.onDeleteUserCallback(data);
    }

    public void onBillingResult(string data)
    {
        sdk.onBillingResult(data);
    }

    public void onWebBillingResult(string data)
    {
        sdk.onWebBillingResult(data);
    }

    public string getClientId()
    {
        return sdk.getClientId();
    }

    public string getClientSecret()
    {
        return sdk.getClientSecret();
    }

    public void setConfig(string appsflyerDevKey, int clientId, string clientSecret, string ggId)
    {
        sdk.setConfig(appsflyerDevKey, clientId, clientSecret, ggId);
    }

    public void trackingEvent(string eventName, Dictionary<string, object> prs)
    {
        sdk.trackingEvent(eventName, prs);
    }

    public void deleteAccount()
    {
        sdk.deleteAccount();
    }
    public void setFloatingButtonEnable(bool floatingButtonEnable)
    {
        sdk.setFloatingButtonEnable(floatingButtonEnable);
    }
}
