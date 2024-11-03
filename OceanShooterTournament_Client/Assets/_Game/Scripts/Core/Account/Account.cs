using System;

public class Account
{
    public string username;
    public string password;
    public AccountData accountData;

    public Account(
        string username,
        string password,
        string id,
        string name,
        int level,
        string avatarID
    )
    {
        this.username = username;
        this.password = password;
        accountData = new AccountData(id, name, level, avatarID);
    }

    public Account(string username, string password, AccountData accountData)
    {
        this.username = username;
        this.password = password;
        this.accountData = accountData;
    }
}

[Serializable]
public class AccountData
{
    public string userID;
    public string userName;
    public int userLevel;
    public string avatarID;

    public AccountData(string userID, string userName, int userLevel, string avatarID)
    {
        this.userID = userID;
        this.userName = userName;
        this.userLevel = userLevel;
        this.avatarID = avatarID;
    }
}