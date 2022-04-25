using System;
using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;
using UnityEngine.Events;
using static ESRefreshTokenHttp;

public class SDKDesktopImpl : ISDK, IAppsFlyerConversionData
{

    public ESGameSDK gameSDK;
    public GameObject loadingView;
    public GameObject floatingView;
    public ESContainer esGameContainer;
    public PopUpMessage messagePopUp;

    public ESLoginSuccessEvent loginSuccessEvent;
    public ESLoginFailureEvent loginFailureEvent;
    public ESLogOutEvent logOutEvent;
    public ESBillingEvent billingEvent;
    public ESWebBillingEvent billingWebEvent;
    public ESUIEvent uiEvent;

    private ESConfig esConfig;
    protected ESGameConfig config;
    protected ESGameProductConfig productConfig;
    private ESLoginResponse usr;
    private string productId;
    private string serverId;
    private string playerId;
    private string extra;
    private bool billingWaitSignIn;
    private bool floatingButtonEnable = true;
    private ESWebViewExecutor webViewExecutor = new ESWebViewExecutor();

    public void init(ESGameSDK gameSDK)
    {
        this.gameSDK = gameSDK;
        loadingView = gameSDK.loadingView;
        floatingView = gameSDK.floatingView;
        esGameContainer = gameSDK.esGameContainer;
        messagePopUp = gameSDK.messagePopUp;
        loginSuccessEvent = gameSDK.loginSuccessEvent;
        loginFailureEvent = gameSDK.loginFailureEvent;
        logOutEvent = gameSDK.logOutEvent;
        billingEvent = gameSDK.billingEvent;
        billingWebEvent = gameSDK.billingWebEvent;
        uiEvent = gameSDK.uiEvent;
        config = gameSDK.getConfig();
        AppsFlyer.setIsDebug(false);
        AppsFlyer.initSDK(config.appFlyerKey, getAppflyerId());
        AppsFlyer.startSDK();
        new ESSettingHttp().execute(gameSDK, complete: this.onSettingSuccess, this.onSettingFailure);
    }

    private void OnHideUnity(bool isGameShown)
    {

    }
    internal void skipUpgradeAccount()
    {
        internalDispatchUserLoginSuccess(usr);
    }

    internal void showLoading(bool v)
    {
        loadingView.SetActive(v);
    }

    public void fetchESConfig(UnityAction<ESConfig> callback)
    {
        if(esConfig != null)
        {
            callback(esConfig);
        }
        else
        {
            new ESSettingHttp().execute(gameSDK, complete: (configResponse) =>
            {
                this.esConfig = configResponse.data;
                callback(esConfig);
            }, (code, message) =>
            {
                callback(null);
            });
        }
        
    }

    public void checkSession(bool requireConfig = true)
    {
        Debug.Log("ESUserDataManager.getAccesstoken()");
        if (requireConfig && esConfig == null)
        {
            new ESSettingHttp().execute(gameSDK, complete: (configResponse) =>
            {
                this.esConfig = configResponse.data;
                checkSession();
            }, (code, message) =>
            {
                checkSession(false);
            });
            return;
        }
        if (!TextUtil.isEmpty(ESUserDataManager.getAccesstoken()))
        {
            getUserInfo(ESUserDataManager.getAccesstoken());
        }
        else
        {
            esGameContainer.startSignIn();
        }
    }

    private void getUserInfo(string accesstoken)
    {
        loadingView.SetActive(true);
        new ESGetUserInfoHttp(accesstoken).execute(gameSDK, this.onUserInfoSuccess, this.onUserInfoFailure);
    }

    private void onUserInfoSuccess(ESLoginResponse arg0)
    {
        loadingView.SetActive(false);
        if (arg0.code == ApiConfig.SUCCESS)
        {
            var usr = arg0.data.user;
            DispatchLoginSuccess(arg0);

        }
        else if (arg0.code == ApiConfig.INVALID_ACCESS_TOKEN)
        {

            refreshToken((ESLoginResponse data, int code, string message) =>
            {
                if (code == ApiConfig.SUCCESS)
                {

                    DispatchLoginSuccess(data);
                }
                else
                {
                    esGameContainer.startSignIn();
                }
            });
        }
        else
        {

            esGameContainer.startSignIn();
        }
    }
    private void refreshToken(OnTokenRefreshed callback)
    {
        new ESRefreshTokenHttp(ESUserDataManager.getRefreshtoken(), callback).execute(gameSDK);
    }
    private void onUserInfoFailure(int arg0, string arg1)
    {
        loadingView.SetActive(false);
        esGameContainer.startSignIn();

    }

    private void onSettingFailure(int arg0, string arg1)
    {
        esConfig = ESConfig.createDefault();
    }

    private void onSettingSuccess(ESConfigResponse arg0)
    {
        esConfig = arg0.data;
    }

    private string getAppflyerId()
    {
        return config.appFlyerDesktopId;
    }

    internal string getGGClientId()
    {
        return config.ggClientIdAndroid;
    }

    public ESConfig GetESConfig()
    {
        if (esConfig == null)
        {
            esConfig = ESConfig.createDefault();
        }
        return esConfig;
    }

    public void login()
    {
        checkSession();
    }

    public void logOut()
    {
        ESUserDataManager.logOut();
        if (ESMainView.instance != null)
        {
            ESMainView.instance.logout();
        }
        gameSDK.StopAllCoroutines();
        Debug.Log("logOutEvent 4" + logOutEvent.ToString());
        showFloatingView(false);
        if (logOutEvent != null)
            logOutEvent.Invoke();
    }

    public void billing(string productId, string serverId, string playerId, string extra)
    {
        throw new NotSupportedException();
    }

    public List<string> getAndroidSubscriptionProducts()
    {
        return productConfig.ggSubscriptions;
    }


    public List<string> getWebProducts()
    {
        return productConfig.webs;
    }

    public void webBilling(string productId, string serverId, string playerId, string extra)
    {
        this.productId = productId;
        this.serverId = serverId;
        this.playerId = playerId;
        this.extra = extra;
        if (TextUtil.isEmpty(ESUserDataManager.getAccesstoken()))
        {
            this.billingWaitSignIn = true;

            esGameContainer.startSignIn();
        }
        else
        {
            Debug.Log("webBilling " + productId);
            new ESWebPurchaseHttp(productId, getClientId(), serverId, playerId, extra).execute(gameSDK, (ESWebPaymentResponse arg0) =>
            {
                showWebView(arg0.data.url, "Payment");
            }, (int code, string message) =>
            {
                showLoading(false);
                if (code == ApiConfig.INVALID_ACCESS_TOKEN)
                {
                    refreshToken((ESLoginResponse data, int code1, string message1) =>
                    {
                        if (code1 == ApiConfig.SUCCESS)
                        {

                            webBilling(productId, serverId, playerId, extra);
                        }
                        else
                        {
                            esGameContainer.startSignIn();
                        }
                    });
                }
                else
                {
                    showMessage(message);
                }
            });
        }
    }

    public void showWebView(string url, string title)
    {
        webViewExecutor.showWebView(url, title);
    }

    private void trackingWebPurchase(string itemId, int price)
    {
#if !PLATFORM_ANDROID && !PLATFORM_IPHONE
       TrackingHelper.trackingWebPurchase(itemId, price);
#endif

    }

    private void trackingPurchase(ESPurchaseResponse arg0)
    {
#if !PLATFORM_ANDROID && !PLATFORM_IPHONE
        TrackingHelper.trackingPurchase(arg0);
#endif

    }

    internal void DispatchLoginSuccess(ESLoginResponse arg0)
    {
        Debug.LogError("auth insert " + arg0.data.auth.access_token);

        if (arg0.data.auth != null && arg0.data.auth.access_token != null && arg0.data.auth.refresh_token != null)
        {
            ESUserDataManager.saveESUserToken(arg0.data.auth);
        }
        ESUserDataManager.saveLastNormalUser(arg0.data.user);
        this.usr = arg0;
        /**
         * show upgrade accountview 
        Debug.LogError("isuser device " + arg0.data.user.isUserDevice());
        if (arg0.data.user.isUserDevice())
        {
            esGameContainer.startUpgradeAccount();
            return;
        }
        */
        internalDispatchUserLoginSuccess(arg0);
    }

    internal void internalDispatchUserLoginSuccess(ESLoginResponse arg0)
    {
        gameSDK.StopAllCoroutines();
        if (arg0.data.user.needOverTimeNotify(GetESConfig()))
        {
            gameSDK.StartCoroutine(this.showAgeNotify());
        }
        this.esGameContainer.gameObject.SetActive(false);
        if (billingWaitSignIn)
        {
            billing(productId, serverId, playerId, extra);
        }
        billingWaitSignIn = false;
        Debug.LogError("dispatch login success " + loginSuccessEvent);
        if (loginSuccessEvent != null)
            loginSuccessEvent.Invoke(arg0.data.user);
        showFloatingView(true);
        trackingLogin(arg0);

    }

    public void trackingEvent(string ev, Dictionary<string, object> prs)
    {
#if !PLATFORM_ANDROID && !PLATFORM_IPHONE
        TrackingHelper.tracking(ev, prs);
#endif
    }

    private void trackingLogin(ESLoginResponse arg0)
    {
#if !PLATFORM_ANDROID && !PLATFORM_IPHONE
        TrackingHelper.trackingLogin(arg0);
#endif
    }

    private void trackingRegister(ESLoginResponse arg0)
    {

#if !PLATFORM_ANDROID && !PLATFORM_IPHONE
        TrackingHelper.trackingRegister(arg0);
#endif
    }

    private IEnumerator showAgeNotify()
    {
        var config = GetESConfig();
        Debug.LogError("start wait show age notify ");
        yield return new WaitForSeconds(config.overtime_condition_minutes * 60);
        Debug.LogError("show age notify ");
        showMessage(String.Format("Chơi game quá {0} phút sẽ ảnh hưởng đến sức khoẻ, vui lòng nghỉ ngơi trong ít phút", config.overtime_condition_minutes));
        gameSDK.StartCoroutine(this.showAgeNotify());
    }

    public void showMessage(string message)
    {
        if (message == null || message.Length == 0)
        {
            return;
        }
        messagePopUp.show(message);
    }

    internal void DispatchLoginFailure(int code, string message)
    {
        if (loginFailureEvent != null)
            loginFailureEvent.Invoke(new ESErrorEvent(code, message));
    }

    internal ESUser getCurrentUser()
    {
        if (usr != null && usr.data != null && usr.data.user != null)
            return usr.data.user;
        return null;
    }
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("onConversionDataFail", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }

    private bool sdkShow = false;

    public SDKDesktopImpl()
    {
    }

    internal void notifyUIChange(bool show)
    {
        if (show)
        {
            if (!sdkShow)
            {
                sdkShow = true;
                uiEvent.Invoke(show);
            }
        }
        else
        {
            if (!loadingView.active && !esGameContainer.gameObject.active && !messagePopUp.gameObject.active)
            {
                uiEvent.Invoke(show);
                sdkShow = false;
            }
        }
    }

    internal void showFloatingView(bool show)
    {
        if(!show ||  floatingButtonEnable)
        floatingView.SetActive(show);
    }

    public List<string> getProducts()
    {
        return getWebProducts();
    }

    ESUser ISDK.getCurrentUser()
    {
        if(usr!=null && usr.data!=null)
        return usr.data.user;
        return null;
    }

    public string getClientId()
    {
        return config.clientIdAndroid;
    }
    public string getClientSecret()
    {
        return config.clientSecretAndroid;
    }
    public void onLoginSuccess(string data)
    {
        
    }

    public void onLoginFailure(string data)
    {
       
    }

    public void onLogout(string data)
    {
       
    }

    public void onBillingResult(string data)
    {
       
    }

    public void onWebBillingResult(string data)
    {
   
    }

    public void setConfig(string appsflyerDevKey, int clientId, string clientSecret, string ggId)
    {
       
    }

    public void setFloatingButtonEnable(bool floatingButtonEnable)
    {
        this.floatingButtonEnable = floatingButtonEnable;
        if (!floatingButtonEnable)
        {
            showFloatingView(false);
        }
    }
}
