using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class ESRegisterView : ESBaseView
{
    public InputField uName;
    public InputField email;
    public InputField pwd;
    public InputField rePwd;
    public InputField fullName;
    public GameObject toggleContainer;
    public Toggle toggle;
    public InputField birthday;
    public InputField phone;
    public InputField address;
    public InputField cmt;
    public UI.Dates.DatePicker datePicker;

    private void OnEnable()
    {
       
        if (((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).GetESConfig().full_register == ESConfig.TRUE && !AppUtil.isPhone())
        {
            toggleContainer.gameObject.SetActive(true);
            birthday.gameObject.SetActive(true);
            phone.gameObject.SetActive(true);
            address.gameObject.SetActive(true);
            cmt.gameObject.SetActive(true);
            fullName.gameObject.SetActive(true);
        }
        else
        {
            fullName.gameObject.SetActive(false);
            toggleContainer.gameObject.SetActive(false);
            birthday.gameObject.SetActive(false);
            phone.gameObject.SetActive(false);
            address.gameObject.SetActive(false);
            cmt.gameObject.SetActive(false);
        }
    }


    public void register() {
        int gender = toggle.isOn ? 0 : 1;
        
        string message = "";
        if (TextUtil.isEmpty(uName.text))
        {
            message = "Vui lòng nhập tên đăng nhập!";
        }
        else
        if (TextUtil.isEmpty(email.text))
        {
            message = "Vui lòng nhập email!";
        }
        else
        if (!EmailUtil.validateEmail(email.text))
        {
            message = "Email không đúng định dạng!";
        }
        else
        if (TextUtil.isEmpty(pwd.text))
        {
            message = "Vui lòng nhập mật khẩu!";
        }
        else
        if (TextUtil.isEmpty(rePwd.text))
        {
            message = "Vui lòng nhập lại mật khẩu!";
        }
        else
        if (pwd.text.Equals(rePwd))
        {
            message = "Mật khẩu và mật khẩu nhập lại phải giống nhau! ";
        }
        
        if (((SDKDesktopImpl)ESGameSDK.instance.GetSDK()).GetESConfig().full_register == ESConfig.TRUE && !AppUtil.isPhone())
        {
            if (TextUtil.isEmpty(fullName.text))
            {
                message = "Vui lòng nhập họ và tên!";
            }
            else
            if (TextUtil.isEmpty(birthday.text))
            {
                message = "Vui lòng nhập ngày sinh!";
            }
            else
            if (TextUtil.isEmpty(phone.text))
            {
                message = "Vui lòng nhập số điện thoại!";
            }
            else
            if (IsPhoneNumber(phone.text))
            {
                message = "Số điện thoại không đúng định dạng!";
            }
            else
            if (TextUtil.isEmpty(address.text))
            {
                message = "Vui lòng nhập địa chỉ!";
            }
            else
            if (TextUtil.isEmpty(cmt.text))
            {
                message = "vui lòng nhập số chứng minh thư!";
            }
           
            if (!TextUtil.isEmpty(message))
            {
                container.showNotify(message);
                return;
            }
            container.showLoading(true);
            new ESRegisterFullHttp(uName.text, pwd.text,
           rePwd.text, email.text, fullName.text, address.text,birthday.text, phone.text, cmt.text, gender
           ).execute(this, this.onApiSuccess, this.onApiError);
        }
        else
        {
            if (!TextUtil.isEmpty(message))
            {
                container.showNotify(message);
                return;
            }
            container.showLoading(true);
            new ESRegisterHttp(uName.text, pwd.text,
           rePwd.text, email.text
           ).execute(this, this.onApiSuccess, this.onApiError);
        }
    }

    public void onpenDateSelect()
    {
        datePicker.gameObject.SetActive(true);
        datePicker.Show(true);
    }

    public void onDateSelected(DateTime dateTime)
    {
        Debug.Log(dateTime);
        birthday.text = dateTime.ToString("dd-MM-yyyy");
        datePicker.Hide();
    }

    public static bool IsPhoneNumber(string number)
    {
        return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
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

   override
   public  int getStackIndex()
    {
        return 2;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
