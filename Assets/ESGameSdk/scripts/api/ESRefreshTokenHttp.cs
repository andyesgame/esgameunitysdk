using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESRefreshTokenHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    public delegate void OnTokenRefreshed(ESLoginResponse data,int code, string message);
    private OnTokenRefreshed callback;
    public ESRefreshTokenHttp(string refreshToken, OnTokenRefreshed delegateCallback) : base(ApiConfig.refreshToken, new string[1] { refreshToken })
    {
        this.callback = delegateCallback;
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("refreshToken" + inputs[0]);
        form.AddField("refreshToken", (string)inputs[0]);
    }

    protected override void DispatchSuccess(ESLoginResponse data)
    {
        callback?.Invoke(data, ApiConfig.SUCCESS, "");
    }

    protected override void DispatchFailure(int code, string message)
    {
        callback?.Invoke(null, code, message);
    }
}
