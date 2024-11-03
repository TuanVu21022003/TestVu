using UnityEngine;

[SerializeField]
public class DataAccessObject
{
    public string idRole;
    public string action;

    public string GetToken()
    {
        return $"{idRole};{action}";
    }
}

[SerializeField]
public class ServerDataAccessObject<T> : DataAccessObject
{
    public T data;
}