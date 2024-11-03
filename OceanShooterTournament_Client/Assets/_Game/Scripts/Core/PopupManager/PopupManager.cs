using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoSingletonDontDestroyOnLoad<PopupManager>
{
    [SerializeField] private PopupDataSO _popupDataSO;
    private List<Popup> _popupQueue = new();
    private Popup _currentPopup;

    public void ForceShowPopup(
        PopupType type,
        Transform parent = null,
        Action<Popup> initPopup = null,
        Action onShown = null,
        Action onClosed = null
    )
    {
        ForceShowPopup<Popup>(type, parent, initPopup, onShown, onClosed);
    }

    public void ForceShowPopup<T>(
        PopupType type,
        Transform parent = null,
        Action<T> initPopup = null,
        Action onShown = null,
        Action onClosed = null
    ) where T : Popup
    {
        var popup = _popupDataSO.GetPopup(type, parent ?? transform) as T;
        if (popup == null)
        {
            return;
        }

        popup.transform.localPosition = Vector3.zero;
        initPopup?.Invoke(popup);
        popup.Init(
            () => { onShown?.Invoke(); },
            () =>
            {
                ObjectPoolManager.instance.Push(popup);
                onClosed?.Invoke();
                ShowNextPopup();
            }
        );
        popup.Show();
    }

    public void ShowPopup(
        PopupType type,
        Transform parent = null,
        Action<Popup> initPopup = null,
        Action onShown = null,
        Action onClosed = null
    )
    {
        var popup = _popupDataSO.GetPopup(type, parent ?? transform);
        if (popup == null)
        {
            return;
        }

        initPopup?.Invoke(popup);
        _popupQueue.Add(popup);
        _popupQueue.Sort(_popupDataSO.ComparisonPopup);
        popup.Init(
            () =>
            {
                _currentPopup = popup;
                onShown?.Invoke();
            },
            () =>
            {
                ObjectPoolManager.instance.Push(popup);
                _currentPopup = null;
                onClosed?.Invoke();
                ShowNextPopup();
            }
        );
        ShowNextPopup();
    }

    private void ShowNextPopup()
    {
        if (_popupQueue.Count <= 0 || _currentPopup != null)
        {
            return;
        }

        var popup = _popupQueue[0];
        _popupQueue.RemoveAt(0);
        popup.Show();
    }
}