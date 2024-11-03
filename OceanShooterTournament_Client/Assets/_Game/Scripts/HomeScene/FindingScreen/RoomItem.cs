using System;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour, IGetPoolID
{
    [SerializeField] private TMP_Text _nameRoom;
    [SerializeField] private TMP_Text _numberPeople;

    private RoomData _roomData;
    private Action<string> OnJoinRoom;

    public string GetPoolID()
    {
        return nameof(RoomItem);
    }

    public RoomItem Init(RoomData roomData, Action<string> onJoinRoom)
    {
        _roomData = roomData;
        _nameRoom.text = roomData.roomName;
        _numberPeople.text = $"{roomData.countPlayer}/4";
        OnJoinRoom = onJoinRoom;
        return this;
    }

    public void BtnListener_JoinRoom()
    {
        if (_roomData == null) return;
        if (_roomData.roomID.IsNullOrWhitespace()) return;
        OnJoinRoom?.Invoke(_roomData?.roomID);
    }
}