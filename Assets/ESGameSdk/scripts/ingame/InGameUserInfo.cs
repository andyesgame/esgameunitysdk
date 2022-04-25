using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUserInfo : BaseInGame
{
    public Text name;
    public Text id;
    public Toggle toggle;
    public InputField fullName;
    public InputField address;
    public InputField cmnd;
    public InputField birthday;
    private bool enableUpdateInfo;

    private void OnEnable()
    {
        this.enableUpdateInfo = false;
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(true);
        new ESGetUserInfoHttp(ESUserDataManager.getAccesstoken()).execute(this, this.onUserInfoSuccess, this.onUserInfoFailure);
    }

    private void onUserInfoFailure(int code, string message)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage(message);
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
        fullName.text = emtptyOrNA(usr.full_name ) ? "": usr.full_name;
        cmnd.text = emtptyOrNA(usr.national_id) ? "" : usr.national_id;
        birthday.text = emtptyOrNA(usr.birthday) ? "" : usr.birthday;
        address.text = emtptyOrNA(usr.address) ? "" : usr.address;
        toggle.isOn = usr.gender == 1;
        this.enableUpdateInfo = false;
    }

    private Boolean emtptyOrNA(string s)
    {
        return s == null || s.Length == 0 || s.Equals("N/A");
    }

    public void onInputChange()
    {
        Debug.Log("onInputChange " );
        this.enableUpdateInfo = true;
    }

    public void updateInfo()
    {
        if (enableUpdateInfo)
        {
            int gender = toggle.isOn ? 0 : 1;
            ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(true);
            new ESChangeUserInfo(ESUserDataManager.getAccesstoken(), fullName.text,cmnd.text,birthday.text, gender,address.text).execute(this, this.onUpdateInfoSuccess, this.onUpdateInfoFailure);
        }
    }

    private void onUpdateInfoFailure(int arg0, string arg1)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage(arg1);
    }

    private void onUpdateInfoSuccess(ESLoginResponse arg0)
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showLoading(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).showMessage("Updage Info success");
        onUserInfoSuccess(arg0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
