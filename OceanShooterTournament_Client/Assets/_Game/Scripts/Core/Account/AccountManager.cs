using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AccountManager : MonoSingletonDontDestroyOnLoad<AccountManager>
{
    public bool IsLoggedIn { get; private set; } = false;
    [SerializeField] private AvatarSO _avatars;
    private Account _currentAccount;

    public override void Init()
    {
        _currentAccount = null;
    }

    public bool Login(Account account, out Action markLoggedIn)
    {
        _currentAccount = account;
        markLoggedIn = () => IsLoggedIn = true;
        return true;
    }

    public string GetAccountId()
    {
        return _currentAccount?.accountData.userID;
    }

    public Account GetCurrentAccount()
    {
        return _currentAccount;
    }

    public void ClearCurrentAccount()
    {
        _currentAccount = null;
    }

    public string CreateToken(string action)
    {
        return $"{_currentAccount?.accountData.userID ?? "client"};{action}";
    }

    public UniTask<AccountData> GetAccountDataFromId(string id)
    {
        if (id == GetAccountId())
        {
            return UniTask.FromResult(_currentAccount.accountData);
        }

        var accountData = new AccountData(id, "Test", 1, "avatar001");
        return UniTask.FromResult(accountData);
    }

    public Sprite GetAvatarFromId(string id, AvatarType type)
    {
        return _avatars.GetAvatarFromID(id, type);
    }
}