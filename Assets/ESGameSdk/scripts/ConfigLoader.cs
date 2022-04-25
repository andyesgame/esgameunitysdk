using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{

    public TextAsset configFile;
    public TextAsset productFile;

    protected ESGameConfig config;
    protected ESGameProductConfig productConfig;

    void Start()
    {
        config = JsonUtility.FromJson<ESGameConfig>(configFile.text);
        productConfig = JsonUtility.FromJson<ESGameProductConfig>(productFile.text);
        OnStart();
    }

    protected virtual void OnStart()
    {
       
    }

    public ESGameConfig getConfig()
    {
        return config;
    }

    public ESGameProductConfig getProductConfig()
    {
        return productConfig;
    }
}
