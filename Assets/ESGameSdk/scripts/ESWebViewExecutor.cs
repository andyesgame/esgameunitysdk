using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public interface ESWebViewInterface
{
    void showWebView(string url, string title);
}

#if PLATFORM_ANDROID
public class ESWebViewExecutor : ESWebViewInterface
{
    public void showWebView(string url, string title)
    {

        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.Call("showWeb", url,title);
            }
        }

    }
}

#else

public class ESWebViewExecutor : ESWebViewInterface
{

    public void showWebView(string url, string title)
    {

        Application.OpenURL(url);

    }
}
#endif
