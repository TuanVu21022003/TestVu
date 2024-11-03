using System.Collections.Generic;
using Core.StateMachine;
using UnityEngine;

public class RoomData
{
    public string roomID;
    public string roomName;
    public int countPlayer;
}

public class JoinRoomData : RoomData
{
    public string adminID;
    public List<AccountData> accountDatas;

    public JoinRoomData(
        string roomID,
        string roomName,
        List<AccountData> accountDatas
    )
    {
        this.roomID = roomID;
        this.roomName = roomName;
        this.accountDatas = accountDatas;
    }
}

public class FindingScreen : Popup
{
    [SerializeField] private RoomItem _roomItemPrefab;
    [SerializeField] private Transform _roomItemContainer;
    [SerializeField] private WaitingRoom _waitingRoom;

    private List<RoomItem> _roomItems = new();

    public override void Show()
    {
        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken("join_room_complete"),
            JoinRoom
        );
        ObserverManager.instance.Register(
            AccountManager.instance.CreateToken("get_list_room"),
            InitAllRoom
        );
        NetworkManager.instance.Request("get_list_room", "");
        if (IsVisible) return;
        base.Show();
    }

    public override void Hide()
    {
        ObserverManager.instance.Unregister(
            AccountManager.instance.CreateToken("join_room_complete"),
            JoinRoom
        );
        ObserverManager.instance.Unregister(
            AccountManager.instance.CreateToken("get_list_room"),
            InitAllRoom
        );
        base.Hide();
    }

    private void InitAllRoom(string data)
    {
        var listRoomRequestData = data.ToObject<AllRoomDataSDAO>().data;

        while (listRoomRequestData.Count > _roomItems.Count)
        {
            var roomItem = ObjectPoolManager.instance.Get(
                nameof(RoomItem),
                _roomItemContainer,
                () => Instantiate(_roomItemPrefab, _roomItemContainer)
            );
            _roomItems.Add(roomItem);
        }

        while (listRoomRequestData.Count < _roomItems.Count)
        {
            var roomItem = _roomItems[0];
            _roomItems.RemoveAt(0);
            ObjectPoolManager.instance.Push(roomItem);
        }

        for (var i = 0; i < listRoomRequestData.Count; i++)
        {
            _roomItems[i].Init(listRoomRequestData[i], RequestJoinRoom);
        }
    }

    public void BtnListener_BackToHome()
    {
        if (GameStateManager.instance.ChangeGameState(GameState.Home))
        {
            Hide();
        }
    }

    public void BtnListener_CreateRoom()
    {
        NetworkManager.instance.Request("create_room", "");
    }

    private void RequestJoinRoom(string roomID)
    {
        NetworkManager.instance.Request("join_room", roomID);
    }

    private void JoinRoom(string roomJsonData)
    {
        var roomData = roomJsonData.ToObject<RoomRequestDataSDAO>();
        if (GameStateManager.instance.ChangeGameState(GameState.WaitingRoom))
        {
            _waitingRoom.Show(roomData.data);
        }
    }
}