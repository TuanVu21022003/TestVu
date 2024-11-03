using System;
using System.Collections.Generic;

public class ObserverManager : MonoSingletonDontDestroyOnLoad<ObserverManager>
{
    private Dictionary<string, Action<string>> NotificationList = new();

    public void Register(string notificationName, Action<string> action)
    {
        if (!NotificationList.TryAdd(notificationName, action))
        {
            NotificationList[notificationName] += action;
        }
    }

    public void Unregister(string notificationName, Action<string> action)
    {
        if (NotificationList.ContainsKey(notificationName))
        {
            NotificationList[notificationName] -= action;
        }
    }

    public void SendMessageNotification(string notificationName, string message)
    {
        if (NotificationList.ContainsKey(notificationName))
        {
            NotificationList[notificationName]?.Invoke(message);
        }
    }
}