using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
[Serializable]
public enum LoginType
{
    Normal, Fast, Google, Facebook, Apple
}

    [Serializable]
public class ESUser
{
    public int id;
    public string name;
    public int active;
    public string address;
    public string birthday;
    public string email;
    public string full_name;
    public int gender;
    public string national_id;
    public string phone;
    public string provider;
    public bool is_new;
    public string loginType;

    
    public LoginType getLoginType()
    {
        if(loginType == null)
        {
            return LoginType.Normal;
        }
        try
        {
            return (LoginType)Enum.Parse(typeof(LoginType), loginType, true);
        }
        catch(Exception e)
        {
            return LoginType.Normal;
        }
    }
    

    internal bool isUserDevice()
    {
        if (!TextUtil.isEmpty(provider)&&provider.Equals("app_device",StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }
        return false;
    }

    internal bool needOverTimeNotify(ESConfig eSConfig)
    {
        if(eSConfig.overtime_check == ESConfig.TRUE && !TextUtil.isEmpty(birthday)&& !birthday.Equals("N/A")){
            if (AppUtil.CalculateAgeCorrect(birthday)<= eSConfig.overtime_condition_year)
            {
                return true;
            }
        }
        Debug.LogError("birthday "+birthday);
        return false;
    }
}
