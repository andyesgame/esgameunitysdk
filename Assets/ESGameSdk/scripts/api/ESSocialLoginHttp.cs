using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESSocialLoginHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    public ESSocialLoginHttp(string token,string provider) : base(ApiConfig.loginSocial, new string[2] { token , provider })
    {
       
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("token"+ inputs[0]);
        form.AddField("token", (string)inputs[0]);
        form.AddField("provider", (string)inputs[1]);
        form.AddField("platform", "web");
    }

}
