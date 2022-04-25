using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESGetUserInfoHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    private string accessToken;

    public ESGetUserInfoHttp(string accessToken) : base(ApiConfig.getUserInfo, new string[0] {  })
    {
        this.accessToken = accessToken;
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        
    }

    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept","application/json");
        request.SetRequestHeader("Authorization", "Bearer "+accessToken);
    }
}
