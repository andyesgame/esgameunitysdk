using System;
using System.Collections.Generic;
using System.Linq;
using AFMiniJSON;
using UnityEngine;

public class DictHelper
{
    public static string toJsonString(Dictionary<String, object> dict)
    {
        string json = Json.Serialize(dict);
        return json;
    }

}
