using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour
{
   
    void Start()
    {
        Debug.Log("start ");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        /**
         * set it if you wwant open esgame auto login behavior or not
         */
        ESGameSDK.autoLogin = false;
        /**
         * Deprecated , use if you want use unity method to get device id .Usefull if you have already production mode
         */
        ESGameSDK.useAndroidUnityDeviceId = true;
        /**
        * Deprecated , use if you want use unity method to get device id .Usefull if you have already production mode
        */
        ESGameSDK.useIosUnityDeviceId = true;
        StartCoroutine(this.waitESSDKInit());
    }

    private IEnumerator waitESSDKInit()
    {
        yield return new WaitForEndOfFrame();
        if (ESGameSDK.instance != null)
        {
            Debug.LogError("SDK inited " + ESGameSDK.instance);
            
            ESGameSDK.instance.loginSuccessEvent.AddListener(this.onUserSignIn);
            ESGameSDK.instance.loginFailureEvent.AddListener(this.onUserSignInError);
            ESGameSDK.instance.logOutEvent.AddListener(this.onUserSignOut);
            ESGameSDK.instance.billingEvent.AddListener(this.ESBillingEvent);
            ESGameSDK.instance.uiEvent.AddListener(this.ESUIEvent);
            /**
             * Open login behavio , if usser token is available then sdk wwil notify login success event,.
             * If user tojen is unavailble then sdk wil open login view
             */
            ESGameSDK.instance.login();
        }
        else
        {
            StartCoroutine(this.waitESSDKInit());
        }
    }


    private void ESUIEvent(bool changed)
    {
        Debug.LogError("ESUIEvent " + changed);
    }

    private void OnDestroy()
    {
        ESGameSDK.instance.loginSuccessEvent.RemoveListener(this.onUserSignIn);
        ESGameSDK.instance.loginFailureEvent.RemoveListener(this.onUserSignInError);
        ESGameSDK.instance.logOutEvent.RemoveListener(this.onUserSignOut);
        ESGameSDK.instance.billingEvent.RemoveListener(this.ESBillingEvent);
        ESGameSDK.instance.uiEvent.RemoveListener(this.ESUIEvent);
    }

    public void onUserSignIn(ESUser eSUser)
    {
        //Debug.LogError("Welcome User "+eSUser.id+" signIn");
        //SceneManager.LoadScene("ESGameDemo", LoadSceneMode.Single);
        ESGameSDK.instance.trackingEvent("upbtn_gold", new Dictionary<string, object>());
        ESGameSDK.instance.startInGameMain();
    }

    public void onUserSignInError(ESErrorEvent eSErrorEvent)
    {
        Debug.Log(string.Format("Error code {0} message {1}", eSErrorEvent.code,eSErrorEvent.message) + "\n");
    }

    public void ESBillingEvent(ESBillingResult billingResult)
    {
        Debug.Log(string.Format("billing success {0} ", billingResult.productId) + "\n");
    }

    public void onUserSignOut()
    {
        Debug.Log("user signout" + "\n");
    }

    public void onLoginClick()
    {
        ESGameSDK.instance.login();
    }

    public void onLogoutClick()
    {
        ESGameSDK.instance.logOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
