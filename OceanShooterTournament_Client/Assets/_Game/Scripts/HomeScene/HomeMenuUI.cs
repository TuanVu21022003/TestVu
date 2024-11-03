using System.Collections;
using Core.StateMachine;
using DG.Tweening;
using UnityEngine;

public class HomeMenuUI : Popup
{
    [SerializeField] private CanvasGroup _canvasGroup;

    protected override IEnumerator ShowPopup()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        yield return _canvasGroup.DOFade(1, appearDuration).WaitForCompletion();
        showCallback?.Invoke();
        OnAppeared();
    }

    protected override IEnumerator HidePopup()
    {
        yield return _canvasGroup.DOFade(0, disappearDuration).WaitForCompletion();
        hideCallback?.Invoke();
        OnDisappeared();
        gameObject.SetActive(false);
    }

    public void BtnListener_FindingRoom()
    {
        if (GameStateManager.instance.ChangeGameState(GameState.FindingScreen))
        {
            Hide();
        }
    }
}