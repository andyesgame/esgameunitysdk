using System;
public class TextUtil
{
    public static bool isEmpty(string input)
    {
        if(input != null)
        {
            if(input.Length > 0)
            {
                return false;
            }
        }
        return true;
    }
}
