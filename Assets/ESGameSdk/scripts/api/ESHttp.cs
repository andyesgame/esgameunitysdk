using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Threading.Tasks;

public abstract class ESHttp<R,T> where R:ESResponse<T>,new() 
{
    private string url;
    private object[] input;
    private UnityAction<R> complete;
    private UnityAction<int, string> error;
    public ESHttp(string url, object[] input)
    {
        this.url = url;
        this.input = input;
    }

    public void execute(MonoBehaviour script,UnityAction<R> complete, UnityAction<int, string> error)
    {
        this.complete = complete;
        this.error = error;
        Debug.LogError("start  execute " + url);
        UnityWebRequest request = onExecute(url,input);
        Debug.LogError("start  corotuine " + request.url);
        script.StartCoroutine(executeRequest(request,complete,error));
    }

    public void execute(MonoBehaviour script)
    {
        this.execute(script, null, null);
    }

    internal abstract UnityWebRequest onExecute(string url, object[] input);



    // Use this for initialization
    public IEnumerator executeRequest(UnityWebRequest request, UnityAction<R> complete, UnityAction<int,string> error)
    {
        Debug.LogError("start request "+request.url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            DispatchFailure(-1,request.error);
        }
        else
        {
            string s = request.downloadHandler.text;
            Debug.LogError("http response "+s);
            R response = JsonUtility.FromJson<R>(s);
            Debug.LogError("http response obj " + response);
            if (response.code != 200)
            {
                DispatchFailure(response.code, response.message);
            }
            else
            {
                DispatchSuccess(response);
            }
            
        }

    }

    protected virtual void DispatchSuccess(R data)
    {
        complete?.Invoke(data);
    }

    protected virtual void DispatchFailure(int code,string message)
    {
        error?.Invoke(code, message);
    }
}
