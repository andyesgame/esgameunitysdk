using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ESBaseView : MonoBehaviour
{
    public Text version;
    public ESContainer container;

    // Use this for initialization
    protected virtual void Start()
    {
        version.text = "version "+Application.version;
    }

    public void onClose()
    {
        container.gameObject.SetActive(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).loginFailureEvent.Invoke(new ESErrorEvent(-1, null));
    }

    public void onBack()
    {
        container.onBackAction(this);
    }

    public virtual int getStackIndex()
    {
        return 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
