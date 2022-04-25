using System;
public interface ESCallback
{
    void onLoginSuccess(ESUser user);
    void onLoginFailure(int code, string message);
    void onBillingSuccess();
}
