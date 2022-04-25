using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESFastLoginHttp : ESHttpPost<ESLoginResponse,ESDataLogin>
{
    public ESFastLoginHttp(string deviceId) : base(ApiConfig.fastLogin, new string[1] { deviceId })
    {
       
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("device_id"+ inputs[0]);
        form.AddField("device_id", (string)inputs[0]);
    }

}
