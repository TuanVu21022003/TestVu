using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Popup : MonoBehaviour, IGetPoolID
{
    [SerializeField] protected float appearDuration = 0.2f;
    [SerializeField] protected float disappearDuration = 0.2f;

    protected Action showCallback;
    protected Action hideCallback;

    public UnityEvent preAppearingEvent;
    public UnityEvent onAppearedEvent;
    public UnityEvent onDisappearedEvent;
    
    public bool IsVisible => gameObject.activeInHierarchy;

    public PopupType popupType;
    public Transform main;
    public CanvasGroup background;

    public Popup Init(Action onShown = null, Action onClosed = null)
    {
        showCallback = onShown;
        hideCallback = onClosed;
        return this;
    }

    public virtual void Show(Action onShown, Action onClosed)
    {
        if (showCallback != null)
        {
            showCallback += onShown;
        }
        else
        {
            showCallback = onShown;
        }

        if (hideCallback != null)
        {
            hideCallback += onClosed;
        }
        else
        {
            hideCallback = onClosed;
        }

        Show();
    }

    public virtual void Show()
    {
        preAppearingEvent?.Invoke();
        gameObject.SetActive(true);
        StartCoroutine(ShowPopup());
    }

    protected virtual IEnumerator ShowPopup()
    {
        main.localScale = Vector3.one * 0.66f;
        yield return background?.DOFade(1, appearDuration);
        yield return main.DOScale(Vector3.one, appearDuration).WaitForCompletion();
        showCallback?.Invoke();
        OnAppeared();
    }

    protected virtual void OnAppeared()
    {
        onAppearedEvent?.Invoke();
    }

    public virtual void Hide()
    {
        if (gameObject is { activeSelf: true, activeInHierarchy: true })
        {
            StartCoroutine(HidePopup());
        }
        else
        {
            hideCallback?.Invoke();
        }
    }

    protected virtual IEnumerator HidePopup()
    {
        background?.DOFade(0, disappearDuration);
        yield return main.DOScale(Vector3.zero, disappearDuration).WaitForCompletion();
        hideCallback?.Invoke();
        gameObject.SetActive(false);
        OnDisappeared();
    }

    protected virtual void OnDisappeared()
    {
        onDisappearedEvent?.Invoke();
    }

    public string GetPoolID()
    {
        return PopupDataSO.GetPoolID(popupType);
    }
}