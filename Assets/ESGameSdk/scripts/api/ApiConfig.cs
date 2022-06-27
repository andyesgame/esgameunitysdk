using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ApiConfig
{
    public static int SUCCESS = 200;
    public static int INVALID_ACCESS_TOKEN = 800;

    public static string baseUrl = "https://game-api.esgame.vn/v1/";
    public static string setting = baseUrl + "misc/settings";
    public static string revokeAccess = baseUrl + "/auth/revoke-game-access";
    public static string login = baseUrl+"auth/login";
    public static string register = baseUrl + "auth/register";
    public static string registerFull = baseUrl + "auth/register-full";
    public static string loginSocial = baseUrl + "auth/login-socialite";
    public static string fastLogin = baseUrl + "auth/login-device";
    public static string upgradeAccount = baseUrl + "auth/set-device-account";
    public static string refreshToken = baseUrl + "auth/refresh-token";
    public static string getUserInfo = baseUrl + "auth/user";
    public static string changePwd = baseUrl + "auth/change-password";
    public static string changeccUserInfo = baseUrl + "auth/change-information";
    public static string fetch_social_token = baseUrl + "auth/fetch-app-socialite-token";
    public static string verifyIOSIAP = baseUrl + "payment/verify-apple-store-purchase";
    public static string verifyGGIAP = baseUrl + "payment/verify-play-store-purchase";
    public static string webPurchase = baseUrl + "payment/generate-web-purchase";

}
