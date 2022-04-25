using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESUpgrageAccountHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    string accesstoken;
    public ESUpgrageAccountHttp(string accesstoken,string uName,string uPwd
        ,string rePwd,string email
        ) : base(ApiConfig.upgradeAccount, new object[4] { uName,uPwd,rePwd,email })
    {
        this.accesstoken = accesstoken;
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("name" + inputs[0]);
        form.AddField("name", (string)inputs[0]);
        form.AddField("password", (string)inputs[1]);
        form.AddField("password_confirmation", (string)inputs[2]);
        form.AddField("email", (string)inputs[3]);
    }
    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + accesstoken);
        Debug.Log("Authorization" + request.GetRequestHeader("Authorization"));
    }
}
