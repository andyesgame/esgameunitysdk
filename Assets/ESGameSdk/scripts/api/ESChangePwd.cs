using System;
using UnityEngine;
using UnityEngine.Networking;

public class ChangePwdResponse : ESResponse<object>
{

}

public class ESChangePwd : ESHttpPost<ChangePwdResponse, object>
{
    private string accessToken;

    public ESChangePwd(string accessToken) : base(ApiConfig.changePwd, new string[0] { })
    {
        this.accessToken = accessToken;
    }

    public ESChangePwd(string accessToken, string password, string new_password) : this(accessToken)
    {
        this.password = password;
        this.new_password = new_password;
        
    }

    public string password { get; }
    public string new_password { get; }

    protected override void onConfigForm(WWWForm form, object[] inputs)
    {

        form.AddField("password", password);
        form.AddField("new_password", new_password);
    }

    protected override void onConfigRequest(UnityWebRequest request)
    {
        base.onConfigRequest(request);
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }
}
