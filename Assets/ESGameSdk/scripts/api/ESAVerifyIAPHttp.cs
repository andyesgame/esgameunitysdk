using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESAVerifyIAPHttp : ESHttpPost<ESPurchaseResponse, ESPurchaseData>
{
    public ESAVerifyIAPHttp(string responseData,string signature,string serverId
        ,string player_id,string extra_data
        ) : base(ApiConfig.verifyGGIAP, new object[5] { responseData, signature, serverId, player_id, extra_data })
    {
    }

    protected override void onConfigForm(WWWForm form, object[] inputs){
        Debug.Log("name" + inputs[0]);
        form.AddField("responseData", (string)inputs[0]);
        form.AddField("signature", (string)inputs[1]);
        form.AddField("server_id", (string)inputs[2]);
        form.AddField("player_id", (string)inputs[3]);
        form.AddField("extra_data", (string)inputs[4]);
    }
    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ESUserDataManager.getAccesstoken());
        Debug.Log("Authorization" + request.GetRequestHeader("Authorization"));
    }
}
