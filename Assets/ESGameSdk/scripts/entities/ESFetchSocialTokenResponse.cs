using System;
[Serializable]
public class ESFetchSocialTokenData 
{
    public string provider;
    public string token;
}
[Serializable]
public class ESFetchSocialTokenResponse : ESResponse<ESFetchSocialTokenData>
{

}
