using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PopupType
{
    Login,
    MainMenu,
    FindingScreen,
    WaitingRoom,
    LoadingGamePlay,
}

[Serializable]
public class PopupDataItem
{
    public int priority;
    public Popup popupPrefabs;
}

[CreateAssetMenu(fileName = "PopupData", menuName = "PopupManager/PopupData", order = 0)]
public class PopupDataSO : ScriptableObject
{
    public List<PopupDataItem> popupDataItems;

    public Popup GetPopup(PopupType type, Transform parent)
    {
        return ObjectPoolManager.instance.Get(
            GetPoolID(type),
            parent,
            () => CreateNewPopup(type, parent)
        );
    }

    private Popup CreateNewPopup(PopupType type, Transform parent)
    {
        if (popupDataItems
            .FirstOrDefault(x => x.popupPrefabs.popupType == type)?
            .popupPrefabs is not { } popupPrefabs)
        {
            Debug.LogError($"PopupDataItem not found with type: {type}");
            return null;
        }

        var popup = Instantiate(popupPrefabs, parent);
        return popup;
    }

    public static string GetPoolID(PopupType type)
    {
        return $"Popup.{type.ToString()}";
    }

    public int ComparisonPopup(Popup a, Popup b)
    {
        var aPriority = popupDataItems.FirstOrDefault(x => x.popupPrefabs == a)?.priority ?? 0;
        var bPriority = popupDataItems.FirstOrDefault(x => x.popupPrefabs == b)?.priority ?? 0;
        return aPriority.CompareTo(bPriority);
    }

    [Button]
    public void UpdatePriorityPopup()
    {
        for (var index = 0; index < popupDataItems.Count; index++)
        {
            popupDataItems[index].priority = index;
        }
    }
}