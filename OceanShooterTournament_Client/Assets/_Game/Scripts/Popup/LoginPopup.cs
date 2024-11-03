using TMPro;
using UnityEngine;

public class LoginPopup : Popup
{
    [SerializeField] private TMP_InputField _userName;
    [SerializeField] private TMP_InputField _password;

    public override void Show()
    {
        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken("login_complete"),
            EventLoginComplete
        );
        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken("login_failed"),
            EventLoginFailed
        );
        base.Show();
    }

    private void EventLoginComplete(string data)
    {
        Debug.Log("Login data received: " + data);
        ObserverManager.instance.Unregister(
            AccountManager.instance.CreateToken("login_complete"),
            EventLoginComplete
        );
        ObserverManager.instance.Unregister(
            AccountManager.instance.CreateToken("login_failed"),
            EventLoginFailed
        );
        var accountData = data.ToObject<AccountDataSDAO>();
        var account = new Account(_userName.text, _password.text, accountData.data);
        if (AccountManager.instance.Login(account, out var markLoggedIn))
        {
            if (hideCallback != null)
            {
                hideCallback += markLoggedIn;
            }
            else
            {
                hideCallback = markLoggedIn;
            }

            Hide();
        }
    }

    private void EventLoginFailed(string data) { }

    public void BtnListener_Login()
    {
        NetworkManager.instance.Request("login", $"{_userName.text},{_password.text}");
    }
}