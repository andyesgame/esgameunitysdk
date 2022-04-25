using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PopUpMessage : MonoBehaviour
{
    public Text message;
    private string mMessage;
    public ContentSizeFitter fitter;
    public VerticalLayoutGroup group;
    // Use this for initialization

    public void show(string message)
    {
        this.message.text = (message);
        this.mMessage = (message);
        
        gameObject.SetActive(true);
        forceLayoutRefresh();
    }

    private void forceLayoutRefresh()
    {
        fitter.gameObject.SetActive(false);
        fitter.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
    }

    private void OnEnable()
    {
        this.message.text = (mMessage);
        forceLayoutRefresh();
        
    }
    private int i = 0;

    private IEnumerator genText()
    {
        yield return new WaitForSeconds(1);
        show(mMessage + "\n"+i);
        if(i > 6)
        {
            i = 0;
        }
        StartCoroutine(this.genText());
    }

    public void hide()
    {
        gameObject.SetActive(false);
       
    }

    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.message.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
    }


    // Update is called once per frame
    void Update()
    {

    }
}
