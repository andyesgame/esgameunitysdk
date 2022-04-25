using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESLoginHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    public ESLoginHttp(string uName, string uPwd) : base(ApiConfig.login, new string[2] { uName, uPwd })
    {
       
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("login"+ inputs[0]);
        Debug.Log("password" + inputs[1]);
        form.AddField("login",(string)inputs[0]);
        form.AddField("password", (string)inputs[1]);
    }

}
