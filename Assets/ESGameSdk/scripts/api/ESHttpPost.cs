using System;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ESHttpPost<R,T> : ESHttp<R,T> where R: ESResponse<T>,new()
{
    public ESHttpPost(string url, object[] input) : base(url, input)
    {
       
    }

    internal override UnityEngine.Networking.UnityWebRequest onExecute(string url, object[] input)
    {
        UnityEngine.WWWForm form = new UnityEngine.WWWForm();
        form.AddField("client_id", ESGameSDK.instance.getClientId());
        form.AddField("client_secret",ESGameSDK.instance.getClientSecret());
        Debug.Log("client_id " + ESGameSDK.instance.getClientId() + " client_secret " + ESGameSDK.instance.getClientSecret());
        onConfigForm(form,input);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        return www;
    }

    protected abstract void onConfigForm(UnityEngine.WWWForm form, object[] inputs);
}
