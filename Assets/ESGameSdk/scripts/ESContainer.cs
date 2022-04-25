using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESContainer : MonoBehaviour
{
    
    public List<ESBaseView> views;

    public GameObject loadingView;

    private ESBaseView currentView;
    private Stack<ESBaseView> stacks = new Stack<ESBaseView>();

    // Start is called before the first frame update
    void Start()
    {
        stacks.Clear();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void showView(int type,bool addStack = true)
    {
        ESBaseView actionView = null;
        foreach(ESBaseView view in views)
        {
            Debug.Log(view + " " + view.getStackIndex());
            if (view.getStackIndex() == type)
            {
                actionView = view;
                break;
            }
        }
        if (actionView != null)
        {
            if (currentView != actionView)
            {
                if (currentView != null)
                {
                    currentView.gameObject.SetActive(false);
                    if (addStack)
                    {
                        stacks.Push(currentView);
                    }

                }
                actionView.gameObject.SetActive(true);
                gameObject.SetActive(true);
                currentView = actionView;
            }
            else
            {
                gameObject.SetActive(true); 
                actionView.gameObject.SetActive(true);
            }
        }
        
    }

    internal void startSignIn()
    {
        Debug.LogError("startSignIn");
        gameObject.SetActive(true);
        showView(0);

    }

    internal void onBackAction(ESBaseView eSBaseView)
    {
        Debug.LogError("stack size "+ stacks.Count);
        ESBaseView gameObject = stacks.Pop();
        Debug.LogError("show gameObject " + gameObject);
        if (gameObject != null)
        {
            showView(gameObject.GetComponent<ESBaseView>().getStackIndex(),false);
        }
    }

    internal void startUpgradeAccount()
    {
        showView(3);
    }

    public void showLoading(bool show) {
        loadingView.SetActive(show);
    }

    public void showNotify(string message)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage(message);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
