using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESRegisterFullHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    public ESRegisterFullHttp(string uName,string uPwd
        ,string rePwd,string email,string fullName,string address,string birthday,string phone,string cmt,int gender
        ) : base(ApiConfig.registerFull, new object[10] { uName,uPwd,rePwd,email,fullName,address, birthday, phone,cmt,gender })
    {
       
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("device_id"+ inputs[0]);
        form.AddField("name", (string)inputs[0]);
        form.AddField("password", (string)inputs[1]);
        form.AddField("password_confirmation", (string)inputs[2]);
        form.AddField("email", (string)inputs[3]);
        form.AddField("full_name", (string)inputs[4]);
        form.AddField("address", (string)inputs[5]);
        form.AddField("birthday", (string)inputs[6]);
        form.AddField("phone", (string)inputs[7]);
        form.AddField("national_id", (string)inputs[8]);
        form.AddField("gender", (int)inputs[9]);
        Debug.LogError("cmt "+ inputs[8]);
    }

}
