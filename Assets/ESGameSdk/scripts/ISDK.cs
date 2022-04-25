using System;
using System.Collections.Generic;

public interface ISDK
{
    void init(ESGameSDK gameSDK);
    void setFloatingButtonEnable(bool floatingButtonEnable);
    void setConfig(string appsflyerDevKey,int clientId,string clientSecret,string ggId);
    void login();
    void logOut();
    ESUser getCurrentUser();
    void onLoginSuccess(string data);
    void onLoginFailure(string data);
    void onLogout(string data);
    void onBillingResult(string data);
    void onWebBillingResult(string data);
    void checkSession(bool requireConfig = true);
    void billing(string productId, string serverId, string playerId, string extra);
    void webBilling(string productId, string serverId, string playerId, string extra);
    void trackingEvent(string eventName, Dictionary<string, object> prs);
    List<string> getProducts();
    List<string> getWebProducts();
    string getClientId();
    string getClientSecret();

}
