using System;
[Serializable]
public class ESConfig
{
    public static  int TRUE = 1;
    public static int FALSE = 0;
    public int government_check;
    public int full_register;
    public int overtime_check;
    public int overtime_condition_minutes;
    public int overtime_condition_year;
    public int toggle_main_login;
    public int toggle_register;
    public int toggle_facebook_auth;
    public int toggle_google_auth;
    public int toggle_apple_auth;
    public int toggle_play_now;
    public string url_news_sdk;

    internal static ESConfig createDefault()
    {
        ESConfig rs = new ESConfig();
        rs.full_register = FALSE;
        rs.government_check = FALSE;
        rs.overtime_condition_minutes = 3 * 60;
            rs.overtime_check = FALSE;
        rs.overtime_condition_year = 14;
        return rs;
    }

    internal bool isGovernmentCheck()
    {
        return government_check == TRUE;
    }
}

public class ESConfigResponse : ESResponse<ESConfig>
{
    
}
