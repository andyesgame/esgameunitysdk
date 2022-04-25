using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ESWebPurchaseHttp : ESHttpPost<ESWebPaymentResponse, EsWebPayment>
{
    public ESWebPurchaseHttp(string itemId,string clientId, string serverId
        , string player_id, string extra_data
        ) : base(ApiConfig.webPurchase, new object[5] { itemId,clientId, serverId, player_id, extra_data })
    {
    }

    protected override void onConfigForm(WWWForm form, object[] inputs)
    {
        Debug.Log("name" + inputs[0]);
        form.AddField("item_id", (string)inputs[0]);
        form.AddField("client_id", (string)inputs[1]);
        form.AddField("server_id", (string)inputs[2]);
        form.AddField("player_id", (string)inputs[3]);
        form.AddField("extra_data", (string)inputs[4]);
    }
    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ESUserDataManager.getAccesstoken());
    }
}

