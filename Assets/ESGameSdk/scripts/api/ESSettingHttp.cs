using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESSettingHttp : ESGetHttp<ESConfigResponse,ESConfig>
{
    public ESSettingHttp() : base(ApiConfig.setting, new string[0])
    {
       
    }

    protected override string onConfigRequest(string url, object[] input)
    {
        return url + "?" + "client_id=" + ESGameSDK.instance.getClientId();
    }


    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ESUserDataManager.getAccesstoken());
    }
}
