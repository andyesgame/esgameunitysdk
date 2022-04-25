using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESRegisterHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    public ESRegisterHttp(string uName,string uPwd
        ,string rePwd,string email
        ) : base(ApiConfig.register, new object[4] { uName,uPwd,rePwd,email })
    {
       
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("device_id"+ inputs[0]);
        form.AddField("name", (string)inputs[0]);
        form.AddField("password", (string)inputs[1]);
        form.AddField("password_confirmation", (string)inputs[2]);
        form.AddField("email", (string)inputs[3]);
    }

}
