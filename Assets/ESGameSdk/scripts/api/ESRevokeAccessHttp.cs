using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESRevokeAccessHttp : ESGetHttp<ESConfigResponse,ESConfig>
{
    public ESRevokeAccessHttp() : base(ApiConfig.revokeAccess, new string[0])
    {
       
    }

    protected override string onConfigRequest(string url, object[] input)
    {
        return url;
    }
    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ESUserDataManager.getAccesstoken());
    }
}
