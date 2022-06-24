using System;
using UnityEngine;

public class ESUserDataManager
{
    private static string currentToken = null;
    private static string currentRefreshToken = null;
    public static string getAccesstoken()
    {
        if(currentToken != null)
        {
            return currentToken;
        }
        return PlayerPrefs.GetString("es_access_token");
    }

    public static string getRefreshtoken()
    {
        if (currentRefreshToken != null)
        {
            return currentRefreshToken;
        }
        return PlayerPrefs.GetString("es_refresh_token");
    }

    public static void saveESUserToken(ESAuth eSAuth)
    {
        PlayerPrefs.SetString("es_access_token",eSAuth.access_token);
        PlayerPrefs.SetString("es_refresh_token", eSAuth.refresh_token);
        currentToken = eSAuth.access_token;
        currentRefreshToken = eSAuth.refresh_token;
        PlayerPrefs.Save();
    }
    public static void saveLastNormalUser(ESUser eSUser)
    {
        if(eSUser.provider!=null && !eSUser.provider.Equals("app_device")
            && !eSUser.provider.Equals("google")&&!eSUser.provider.Equals("facebook")
            && !eSUser.provider.Equals("apple"))
        PlayerPrefs.SetString("es_normal_user_id", eSUser.name);
        PlayerPrefs.Save();
    }
    public static string getESUserString()
    {
        return PlayerPrefs.GetString("es_normal_user_id");
    }
   
    internal static void logOut()
    {
        PlayerPrefs.SetString("es_access_token", null);
        PlayerPrefs.SetString("es_refresh_token", null);
        currentToken = null;
        currentRefreshToken = null;
        PlayerPrefs.Save();
    }
}
