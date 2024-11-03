using System.Collections;
using System.Collections.Generic;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CONST;

public class PlayerDataInRoom
{
    public int index;
    public float progress;
    public int numberCoin;
}

public class DataInRoomGame
{
    public Dictionary<string, PlayerDataInRoom> playerDatas;
}

public class PlayBattleLoadingPopup : Popup
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] private List<SlotLoading> _slotLoadings;
    private JoinRoomData _joinRoomData;
    private static List<float> _progresses;

    protected override IEnumerator ShowPopup()
    {
        _canvasGroup.alpha = 0;
        yield return _canvasGroup.DOFade(1, appearDuration).WaitForCompletion();
        showCallback?.Invoke();
        OnAppeared();
    }

    protected override IEnumerator HidePopup()
    {
        yield return _canvasGroup.DOFade(0, disappearDuration).WaitForCompletion();
        hideCallback?.Invoke();
        gameObject.SetActive(false);
        OnDisappeared();
    }

    public void Init(JoinRoomData joinRoomData)
    {
        if (joinRoomData == null) return;
        var accountDatas = joinRoomData.accountDatas;

        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken(ALL_PLAYER_LOADED_COMPLETE),
            StartGame
        );
        _joinRoomData = joinRoomData;

        for (var i = 0; i < _slotLoadings.Count; i++)
        {
            if (i >= accountDatas.Count)
            {
                _slotLoadings[i].gameObject.SetActive(false);
                continue;
            }

            _slotLoadings[i].Init(accountDatas[i]);
        }

        for (var i = 0; i < accountDatas.Count; i++)
        {
            var index = i;
            var accountID = accountDatas[index].userID;
            ObserverManager.instance.Register(
                $"{accountID};{LOADING_PROGRESS}",
                data =>
                {
                    var dataInRoomGame = data.ToObject<PlayerDataInRoomSDAO>().data;
                    _slotLoadings[index].SetProgress(
                        dataInRoomGame.progress
                    );
                }
            );
        }

        UniTask.WhenAll(LoadingSceneGameplay());
    }

    private void StartGame(string data)
    {
        var playerDataInRoom = data.ToObject<DataInRoomSDAO>().data;
        if (GameStateManager.instance.ChangeGameState(GameState.Playing))
        {
            FishGameManager.instance.OnInit(_joinRoomData, playerDataInRoom);
            Hide();
        }
    }

    private async UniTask LoadingSceneGameplay()
    {
        await UniTask.Delay(3000);
        _progresses = new List<float> { 0.1f, 0.35f, 0.75f, 0.85f, 2f };
        var asyncLoad = SceneManager.LoadSceneAsync(SCENE_GAMEPLAY);
        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = false;
            var value = 0f;
            while (value < 0.85f)
            {
                value = Mathf.Lerp(value, asyncLoad.progress, 0.05f);
                if (value > _progresses[0])
                {
                    _progresses.RemoveAt(0);
                    NetworkManager.instance.Request(
                        LOADING_PROGRESS,
                        $"{_joinRoomData.roomID},{value}"
                    );
                }

                await UniTask.Delay(50);
            }

            asyncLoad.allowSceneActivation = true;

            while (!FishGameManager.IsInitialized)
            {
                await UniTask.Delay(100);
            }

            NetworkManager.instance.Request(
                LOADING_PROGRESS,
                $"{_joinRoomData.roomID},{1f}"
            );
        }
    }
}