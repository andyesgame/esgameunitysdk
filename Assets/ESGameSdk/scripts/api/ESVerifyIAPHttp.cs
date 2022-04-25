using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESVerifyIAPHttp : ESHttpPost<ESPurchaseResponse,ESPurchaseData>
{
    public ESVerifyIAPHttp(string receipt,string serverId
        ,string player_id,string extra_data
        ) : base(ApiConfig.verifyIOSIAP, new object[4] { receipt, serverId, player_id, extra_data })
    {
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("name" + inputs[0]);
        form.AddField("receipt", (string)inputs[0]);
        form.AddField("server_id", (string)inputs[1]);
        form.AddField("player_id", (string)inputs[2]);
        form.AddField("extra_data", (string)inputs[3]);
    }
    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ESUserDataManager.getAccesstoken());
    }
}
