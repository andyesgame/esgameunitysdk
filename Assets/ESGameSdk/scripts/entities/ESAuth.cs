using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ESAuth
{
    public string access_token;
    public string refresh_token;
    public long expires_in;
}
