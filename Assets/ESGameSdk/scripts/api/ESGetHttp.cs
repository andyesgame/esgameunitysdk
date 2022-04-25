
using UnityEngine.Networking;

public abstract class ESGetHttp<R, T> : ESHttp<R, T> where R : ESResponse<T>, new()
{
    public ESGetHttp(string url, object[] input) : base(url, input)
    {

    }

    internal override UnityWebRequest onExecute(string url, object[] input)
    {
        UnityWebRequest www = UnityWebRequest.Get(onConfigRequest(url,input));
        return www; 
    }

    protected abstract string onConfigRequest(string url, object[] input);
}
