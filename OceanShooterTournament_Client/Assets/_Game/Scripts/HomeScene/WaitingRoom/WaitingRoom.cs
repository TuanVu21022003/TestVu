using System.Collections.Generic;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using static CONST;

public class WaitingRoom : Popup
{
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private List<SlotWaiting> slots;
    [SerializeField] private GameObject stateReady;
    [SerializeField] private GameObject stateLoading;
    [SerializeField] private GameObject stateWait;
    private JoinRoomData _joinRoomData;
    private bool isStartGame = false;

    public void Show(JoinRoomData joinRoomData)
    {
        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken("new_client_join_room"),
            NewClientJoinRoom
        );
        Init(joinRoomData);
        base.Show();
    }

    public override void Show()
    {
        if (isActiveAndEnabled) return;
        base.Show();
    }

    private void Init(JoinRoomData joinRoomData)
    {
        var isAdminRoom = joinRoomData.adminID == AccountManager.instance.GetAccountId();
        isStartGame = false;
        _joinRoomData = joinRoomData;
        roomName.text = _joinRoomData.roomName;
        stateWait.SetActive(!isAdminRoom);
        stateReady.SetActive(isAdminRoom);
        ChangeButtonReddy();
        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken(START_GAME),
            StartGame
        );
        GeneratePlayerUI();
    }

    private void GeneratePlayerUI()
    {
        if (_joinRoomData == null) return;

        for (var i = 0; i < slots.Count; i++)
        {
            if (i >= _joinRoomData.accountDatas.Count)
            {
                slots[i].SetIsHavePlayer(false);
                continue;
            }

            slots[i].Init(_joinRoomData.accountDatas[i]);
        }
    }

    private void NewClientJoinRoom(string data)
    {
        var accountData = JsonUtility.FromJson<AccountDataSDAO>(data).data;
        if (accountData == null) return;
        if (_joinRoomData == null) return;
        _joinRoomData.accountDatas.Add(accountData);
        GeneratePlayerUI();
    }

    public void BtnListener_Back()
    {
        if (isStartGame) return;
        if (GameStateManager.instance.ChangeGameState(GameState.FindingScreen))
        {
            Hide();
        }
    }

    public void BtnListener_StartGame()
    {
        if (isStartGame) return;
        NetworkManager.instance.Request(
            START_GAME,
            _joinRoomData.roomID
        );
    }

    private void StartGame(string data)
    {
        isStartGame = true;
        ChangeButtonReddy();
        if (GameStateManager.instance.ChangeGameState(GameState.LoadingGamePlay))
        {
            ObserverManager.instance.Unregister(
                AccountManager.instance.CreateToken(START_GAME),
                StartGame
            );
            UniTask.WhenAll(OpenLoadingPopup());
        }
    }

    public async UniTask OpenLoadingPopup()
    {
        await UniTask.Delay(1000);
        PopupManager.instance.ForceShowPopup<PlayBattleLoadingPopup>(
            PopupType.LoadingGamePlay,
            null,
            popup => popup.Init(_joinRoomData)
        );
    }

    private void ChangeButtonReddy()
    {
        stateReady.SetActive(!isStartGame);
        stateLoading.SetActive(isStartGame);
    }
}