using System;
using UnityEngine;
using UnityEngine.Networking;



public class ESFetchSocialiteToken : ESHttpPost<ESFetchSocialTokenResponse, ESFetchSocialTokenData>
{
    private string uuid;

    public ESFetchSocialiteToken(string uuid) : base(ApiConfig.fetch_social_token, new string[0] { })
    {
        this.uuid = uuid;
    }

    protected override void onConfigForm(WWWForm form, object[] inputs)
    {
        form.AddField("uuid", uuid);
    }

    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
    }
}
