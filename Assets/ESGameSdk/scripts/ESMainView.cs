using UnityEngine;
using UnityEngine.UI;
using System;

public class ESMainView : ESBaseView
{

    public static ESMainView instance;
    public GameObject esLogin;
    public GameObject ggLogin;
    public GameObject fbLogin;
    public GameObject appleLogin;
    public GameObject register;
    public GameObject playNow;
    public VerticalLayoutGroup group;
    private bool socialLogin;
    private string uuid;
    // Use this for initialization
    void Start()
    {
        base.Start();

        Transform transform = gameObject.transform;
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).fetchESConfig(callback: (esConfig) =>
        {
            if (esConfig != null)
            {
                //appleLogin.SetActive(esConfig.toggle_apple_auth == 1);
                esLogin.SetActive(esConfig.toggle_main_login == 1);
                ggLogin.SetActive(esConfig.toggle_google_auth == 1);
                fbLogin.SetActive(esConfig.toggle_facebook_auth == 1);
                register.SetActive(esConfig.toggle_register == 1);
                playNow.SetActive(esConfig.toggle_play_now == 1);
            }
        });
 
        instance = this;
        LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
        
    }

    private Transform FindChildInternal(Transform aParent, string aName)
    {
        if (aParent == null) return null;
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindChildInternal(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }

    private void OnEnable()
    {
        ESConfig config = ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).GetESConfig();
        if (config.isGovernmentCheck())
        {
            playNow.gameObject.SetActive(false);
        }

    }

    int counter = 0;
    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("OnApplicationFocus " + hasFocus);
        if (hasFocus && socialLogin)
        {
            counter = 0;
            container.showLoading(true);
            new ESFetchSocialiteToken(uuid).execute(this, this.onSocialTokenFetchSuccess, this.onSocialTokenFetchFailed);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("OnApplicationPause " + pauseStatus);
    }

    private void onSocialTokenFetchFailed(int arg0, string arg1)
    {
        if (counter < 5)
        {
            new ESFetchSocialiteToken(uuid).execute(this, this.onSocialTokenFetchSuccess, this.onSocialTokenFetchFailed);
            counter++;
        }
        else
        {
            container.showLoading(false);
            socialLogin = false;
            uuid = null;
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).DispatchLoginFailure(arg0,arg1);
        }


    }

    private void onSocialTokenFetchSuccess(ESFetchSocialTokenResponse arg0)
    {
        
        if(arg0.code == 200)
        {
            container.showLoading(true);

            new ESSocialLoginHttp(arg0.data.token, arg0.data.provider).execute(this, this.socialLoginSuccess, this.socialLoginFail);
        }
        else
        {
            container.showLoading(false);
        }
    }

    private void socialLoginFail(int arg0, string arg1)
    {
        container.showLoading(false);
        socialLogin = false;
        uuid = null;
    }

    private void socialLoginSuccess(ESLoginResponse arg0)
    {
        container.showLoading(false);
        socialLogin = false;
        uuid = null;
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).DispatchLoginSuccess(arg0);
    }

    public void onClickGoogle()
    {
        if (socialLogin)
        {
            return;
        }
        Application.OpenURL(generateUrl("google"));
    }

    public void onCLickApple()
    {
        if (socialLogin)
        {
            return;
        }
        Application.OpenURL(generateUrl("apple"));
    }

    private string generateUrl(string type)
    {
        string baseUrl = "https://id.esgame.vn/auth/{0}?scheme={1}&uuid={2}";
        uuid = System.Guid.NewGuid().ToString();
        socialLogin = true;
        return String.Format(baseUrl, type, ESGameSDK.instance.getConfig().schemaStandAlone, uuid);
    }

    public void onClickFb()
    {
        /*
        if (socialLogin)
        {
            return;
        }
        */
        string url = generateUrl("facebook");
        Debug.Log("onClickFb "+ url);
        Application.OpenURL(url);
    }
  
    public void onESGameClick()
    {
        container.showView(1);
    }


    public void onRegisterClick() { container.showView(2); }

    public void playNowClick() {
        container.showLoading(true);
        new ESFastLoginHttp(SystemInfo.deviceUniqueIdentifier).execute(this, this.onApiFastLoginSuccess, this.onApiFastLoginError);
    }

    private void onApiFastLoginError(int arg0, string arg1)
    {
        Debug.LogError("code " + arg0 + " error " + arg1);
        container.showLoading(false);
        container.showNotify(arg1);
    }

    private void onApiFastLoginSuccess(ESLoginResponse arg0)
    {
        container.showLoading(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).DispatchLoginSuccess(arg0);
    }

    public void logout()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.OSXPlayer)
        {
            
            //ggHelper.signout();
        }
    }

}
