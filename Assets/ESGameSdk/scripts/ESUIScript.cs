using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESUIScript : MonoBehaviour
{
    private void OnEnable()
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).notifyUIChange(true);
    }

    private void OnDisable()
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).notifyUIChange(false);
    }
}
