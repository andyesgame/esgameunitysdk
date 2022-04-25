using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUpdatePwd: BaseInGame
{
    public Text name;
    public Text id;

    public InputField oldPwd;
    public InputField pwd;
    public InputField confirmPwd;
    public GrayScale grayScale;
    private bool enableUpdateInfo;

    private void OnEnable()
    {
        this.enableUpdateInfo = false;
        grayScale.grayScale = true;
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(true);
        new ESGetUserInfoHttp(ESUserDataManager.getAccesstoken()).execute(this, this.onUserInfoSuccess, this.onUserInfoFailure);
    }

    private void onUserInfoFailure(int code, string message)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage(message);
        grayScale.grayScale = true;
        this.enableUpdateInfo = false;
    }

    private void onUserInfoSuccess(ESLoginResponse arg0)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(false);
        if (arg0.data.auth != null && arg0.data.auth.access_token != null && arg0.data.auth.refresh_token != null)
        {
            ESUserDataManager.saveESUserToken(arg0.data.auth);
        }
        ESUserDataManager.saveLastNormalUser(arg0.data.user);
        var usr = arg0.data.user;
        name.text = usr.name;
        id.text = "ID: "+usr.id +"";
        grayScale.grayScale = true;
        this.enableUpdateInfo = false;
    }

    public void onInputChange()
    {
        Debug.Log("onInputChange " );
        this.enableUpdateInfo = true;
        grayScale.grayScale = false;
    }

    public void updateInfo()
    {
        var pwdS = pwd.text.Trim();
        var oldPwdS = pwd.text.Trim();
        var confirmPwdS = pwd.text.Trim();
        if(oldPwdS == null || oldPwdS.Length == 0)
        {
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage("Vui lòng nhập mật khẩu cũ");
            return;
        }
        if (pwdS == null || pwdS.Length == 0)
        {
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage("Vui lòng nhập mật khẩu mới");
            return;
        }
        if (confirmPwdS == null || confirmPwdS.Length == 0)
        {
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage("Vui lòng nhập xác nhận mật khẩu");
            return;
        }
        if (!confirmPwdS.Equals(pwdS))
        {
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage("Mật khẩu mới và mật khẩu xác nhận phải trùng nhau");
            return;
        }
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(true);
        new ESChangePwd(ESUserDataManager.getAccesstoken(), oldPwd.text, pwd.text).execute(this, this.onUpdatePwdSuccess, this.onUpdatePwdFailure);
    }

    private void onUpdatePwdFailure(int arg0, string arg1)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage(arg1);
    }

    private void onUpdatePwdSuccess(ChangePwdResponse arg0)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage("Cập nhật mật khẩu thành công");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
