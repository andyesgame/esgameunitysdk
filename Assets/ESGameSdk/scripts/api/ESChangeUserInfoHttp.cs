using System;
using UnityEngine;
using UnityEngine.Networking;

public class ESChangeUserInfo : ESHttpPost<ESLoginResponse, ESDataLogin>
{
    private string accessToken;

    public ESChangeUserInfo(string accessToken) : base(ApiConfig.changeccUserInfo, new string[0] { })
    {
        this.accessToken = accessToken;
    }

    public ESChangeUserInfo(string accessToken, string fullName, string nationId, string birthday,int gender, string address) : this(accessToken)
    {
        this.fullName = fullName;
        this.nationId = nationId;
        this.birthday = birthday;
        this.gender = gender;
        this.address = address;
    }

    public string fullName { get; }
    public string nationId { get; }
    public string birthday { get; }
    public int gender { get; }
    public string address { get; }

    protected override void onConfigForm(WWWForm form, object[] inputs)
    {
        form.AddField("full_name", fullName);
        form.AddField("national_id", nationId);
        form.AddField("birthday", birthday);
        form.AddField("gender", gender);
        form.AddField("address", address);
    }

    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }
}
