using System;
using UnityEngine;

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
