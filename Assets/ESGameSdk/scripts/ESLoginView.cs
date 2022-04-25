using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ESLoginView : ESBaseView
{

    public InputField uName;
    public InputField uPwd;
   

    private void OnEnable()
    {
        uName.text = "";
        uPwd.text = "";
        uName.text = ESUserDataManager.getESUserString();
    }

    public void login()
    {
        string message = "";
        if (TextUtil.isEmpty(uName.text))
        {
            message = "Vui lòng nhập tên đăng nhập hoặc địa chỉ email!";
        }
        else
         if (TextUtil.isEmpty(uPwd.text))
        {
            message = "Vui lòng nhập mật khẩu!";
        }
        if (!TextUtil.isEmpty(message))
        {
            container.showNotify(message);
            return;
        }
        container.showLoading(true);
        new ESLoginHttp(uName.text, uPwd.text).execute(this, this.onApiSuccess, this.onApiError);
    }

    private void onApiError(int arg0, string arg1)
    {
        Debug.LogError("code " + arg0 + " error " + arg1);
        container.showLoading(false);
        container.showNotify(arg1);
    }

    private void onApiSuccess(ESLoginResponse arg0)
    {
        container.showLoading(false);
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).DispatchLoginSuccess(arg0);
    }

    public void onRegisterClick() { container.showView(2); }

    override
    public int getStackIndex()
    {
        return 1;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
