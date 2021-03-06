using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ESUpgradeView : ESBaseView
{
    public InputField uName;
    public InputField email;
    public InputField pwd;
    public InputField rePwd;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        /** var usr = ESGameSDK.instance.getCurrentUser();
        uName.text = usr.name;
        email.text = usr.email;*/
        pwd.text = "";
        rePwd.text = "";
    }

    public void upgrade() {
        string message = "";
        if (TextUtil.isEmpty(uName.text))
        {
            message = "Vui lòng nhập tên đăng nhập!";
        }else
        if (TextUtil.isEmpty(email.text))
        {
            message = "Vui lòng nhập email!";
        }else
        if (!EmailUtil.validateEmail(email.text))
        {
            message = "Email không đúng định dạng!";
        }else
        if (TextUtil.isEmpty(pwd.text))
        {
            message = "Vui lòng nhập mật khẩu!";
        }else
        if (TextUtil.isEmpty(rePwd.text))
        {
            message = "Vui lòng nhập lại mật khẩu!";
        }else
        if (pwd.text.Equals(rePwd))
        {
            message = "Mật khẩu và mật khẩu nhập lại phải giống nhau! ";
        }
        if(!TextUtil.isEmpty(message)){
            container.showNotify(message);
            return;
        }
        new ESUpgrageAccountHttp(ESUserDataManager.getAccesstoken(), uName.text, pwd.text,
           rePwd.text, email.text
           ).execute(this, this.onApiSuccess, this.onApiError);
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

    public void skipUpgrade()
    {
        ((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).skipUpgradeAccount();
    }
    override
   public int getStackIndex()
    {
        return 3;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
