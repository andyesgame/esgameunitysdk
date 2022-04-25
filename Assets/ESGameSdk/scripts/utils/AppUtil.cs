using System;
using UnityEngine;

public class AppUtil
{
    public static bool isPhone()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
           Application.platform == RuntimePlatform.OSXPlayer)
        {
            return true;
        }
            return false;
    }

    public static int CalculateAgeCorrect(string birthDate)
    {
        try
        {
            DateTime time = DateTime.ParseExact(birthDate, "yyyy-MM-dd", null);
            return CalculateAgeCorrect(time);
        }
        catch(Exception e)
        {

        }

        return CalculateAgeCorrect(new DateTime());

    }

        public static int CalculateAgeCorrect(DateTime birthDate)
    {
        if(birthDate == null)
        {
            return 0;
        }
        DateTime now = new DateTime();
        int age = now.Year - birthDate.Year;

        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            age--;

        return age;
    }
}
