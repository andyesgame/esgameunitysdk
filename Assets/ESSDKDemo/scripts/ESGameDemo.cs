using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESGameDemo : MonoBehaviour
{

    public Text welcome;
    public Text status;

    private void OnEnable()
    {
        welcome.text = ESGameSDK.instance.getCurrentUser().name;
        ESGameSDK.instance.logOutEvent.AddListener(onLogOut);
        ESGameSDK.instance.billingEvent.AddListener(ESBillingEvent);
        ESGameSDK.instance.billingWebEvent.AddListener(ESBillingWebEvent);
        ESGameSDK.instance.uiEvent.AddListener(this.ESUIEvent);
        ESGameSDK.instance.deleteAccountEvent.AddListener(this.ESDeleteAccountEvent);

        ESGameSDK.instance.startInGameMain();
    }

    private void ESDeleteAccountEvent(DeleteAccountCallbackObject arg0)
    {
        status.text += string.Format("ESDeleteAccountEvent {0}", arg0.status) + "\n";
    }

    private void ESBillingWebEvent(ESBillingWebResult arg0)
    {
        status.text += string.Format("billing {0} {1}", arg0.productId,arg0.price) + "\n";
    }

    private void OnDestroy()
    {
        ESGameSDK.instance.logOutEvent.RemoveListener(onLogOut);
        ESGameSDK.instance.billingEvent.RemoveListener(ESBillingEvent);
        ESGameSDK.instance.uiEvent.RemoveListener(this.ESUIEvent);
    }
    private void ESUIEvent(bool arg0)
    {
        Debug.LogError("ESUIEvent " + arg0);
    }
    protected void onLogOut()
    {
        Debug.Log("onLogOut");
        status.text +=("user signout" + "\n");
        SceneManager.LoadScene("ESSDKD");
    }

    public void onLogoutClick()
    {
        ESGameSDK.instance.logOut();
    }

    public void ESBillingEvent(ESBillingResult billingResult)
    {
        status.text += string.Format("billing success {0} ", billingResult.productId) + "\n";
    }

    public void onBilling()
    {
        Debug.Log("ESGameSDK.instance.getProducts().Count " + ESGameSDK.instance.getProducts());
        if (ESGameSDK.instance.getProducts() == null || ESGameSDK.instance.getProducts().Count == 0)
            return;
        Debug.Log("ESGameSDK.instance.getProducts().Count "+ ESGameSDK.instance.getProducts().Count);
        string productId = ESGameSDK.instance.getProducts()[0];
        
        if (productId == null)
        {
            return;
        }
        string serverId = "1";
        string playerId = "1";
        string extra = "213";
        Debug.Log("ESGameSDK.instance.getProducts(). product id  " + productId);
        ESGameSDK.instance.billing(productId, serverId, playerId, extra);
    }

    public void onBillingWeb()
    {
        string productId = null;
        Debug.Log("count "+ ESGameSDK.instance.getWebProducts().Count);
        if (ESGameSDK.instance.getWebProducts().Count > 0)
        {
            productId = ESGameSDK.instance.getWebProducts()[0];
        }
        if(productId == null)
        {
            return;
        }

        string serverId = "1";
        string playerId = "1";
        string extra = "213";
        ESGameSDK.instance.webBilling(productId, serverId, playerId, extra);
    }
}
