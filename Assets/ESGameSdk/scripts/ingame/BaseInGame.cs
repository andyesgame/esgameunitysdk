using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseInGame : MonoBehaviour
{
    public ESInGameContainer container;


    private void OnDisable()
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(false);
    }

    public void onClose()
    {
        container.hide();
    }

    public void onBack()
    {
        container.show(0);
    }

}
