using System;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

#if !PLATFORM_ANDROID && !PLATFORM_IPHONE
public class TrackingHelper
{
    public static void tracking(string ev, Dictionary<string, object> prs)
    {
        Dictionary<string, string> appflyerParams = new Dictionary<string, string>();
        Dictionary<string, object>.KeyCollection keyColl = prs.Keys;
        var tmp = 0;

        foreach (string s in keyColl)
        {
            object value = null;
            prs.TryGetValue(s, out value);
            appflyerParams.Add(s, value.ToString());
            tmp++;
        }

        AppsFlyer.sendEvent(ev, appflyerParams);
      
    }

    public static void trackingPurchase(ESPurchaseResponse arg0)
    {
        if (arg0 == null || arg0.data == null)
        {
            return;
        }
        decimal price = 0;
        try
        {
            price = Decimal.Parse(arg0.data.item_price);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        System.Collections.Generic.Dictionary<string, string> purchaseEvent = new System.Collections.Generic.Dictionary<string, string>();
        purchaseEvent.Add(AFInAppEvents.CURRENCY, "VND");
        purchaseEvent.Add(AFInAppEvents.REVENUE, arg0.data.item_price);
        purchaseEvent.Add(AFInAppEvents.CONTENT_ID, arg0.data.item_id);
        purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, "category_a");
        AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, purchaseEvent);

        
    }

    public static void trackingLogin(ESLoginResponse arg0)
    {
        if (arg0 == null || arg0.data == null || arg0.data.user == null)
        {
            return;
        }
        System.Collections.Generic.Dictionary<string, string> purchaseEvent = new System.Collections.Generic.Dictionary<string, string>();
        purchaseEvent.Add(AFInAppEvents.CURRENCY, "VND");
        purchaseEvent.Add(AFInAppEvents.CUSTOMER_USER_ID, arg0.data.user.name);
        purchaseEvent.Add("provider", arg0.data.user.provider);
        purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, "category_a");
        AppsFlyer.sendEvent(AFInAppEvents.LOGIN, purchaseEvent);

        if (arg0.data.user.is_new)
        {
            trackingRegister(arg0);
        }
    }

    public static void trackingRegister(ESLoginResponse arg0)
    {
        if (arg0 == null || arg0.data == null || arg0.data.user == null)
        {
            return;
        }
        System.Collections.Generic.Dictionary<string, string> purchaseEvent = new System.Collections.Generic.Dictionary<string, string>();
        purchaseEvent.Add(AFInAppEvents.CURRENCY, "VND");
        purchaseEvent.Add(AFInAppEvents.CUSTOMER_USER_ID, arg0.data.user.name);
        purchaseEvent.Add("provider", arg0.data.user.provider);
        purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, "category_a");
        AppsFlyer.sendEvent("af_register", purchaseEvent);

        

        
    }

    internal static void trackingWebPurchase(string itemId, int price)
    {
        System.Collections.Generic.Dictionary<string, string> purchaseEvent = new System.Collections.Generic.Dictionary<string, string>();
        purchaseEvent.Add(AFInAppEvents.CURRENCY, "VND");
        purchaseEvent.Add(AFInAppEvents.REVENUE, price +"");
        purchaseEvent.Add(AFInAppEvents.CONTENT_ID, itemId);
        purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, "category_a");
        AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, purchaseEvent);

        


    }
}
#endif