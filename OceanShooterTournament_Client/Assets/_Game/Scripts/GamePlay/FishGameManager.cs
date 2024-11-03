using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FishGameManager : MonoSingleton<FishGameManager>
{
    [SerializeField] private List<Transform> listPosPlayer = new ();
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Sprite coinMe;
    [SerializeField] private Sprite coinOther;
    [SerializeField] private TouchPoint _touchPoint;
    [SerializeField] public float timeContinuos = 3f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private EffectCoin effectCoinMe;
    [SerializeField] private EffectCoin effectCoinOther;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private PrepareGame prepareGamePopup;

    private Dictionary<string, Player> listPlayer = new();
    private Dictionary<string, BaseServerItem> listBullets = new();
    private Dictionary<string, BaseServerItem> listFishes = new();
    private Player _player;
    private bool isEndGame;

    public bool isFireContinuous = false;

    public Player player => _player;

    public TouchPoint touchPoint => _touchPoint;

    public string roomID;

    public void OnInit(JoinRoomData roomData, DataInRoomGame dataInRoomGame)
    {
        roomID = roomData.roomID;
        GeneratePlayer(roomData, dataInRoomGame);
        listBullets = new();
        listFishes = new();
        ObserverManager.instance.Register(
            $"{roomID};update_new_frame",
            UpdateFrame
        );
        ObserverManager.instance.Register(
            $"{roomID};receive_coin",
            OnHandleReceiveCoin
        );
        ObserverManager.instance.Register(
            $"{roomID};change_bet",
            OnHandleChangeBet
        );
        ObserverManager.instance.Register(
            $"{roomID};update_timer",
            OnHandleUpdateTimer
        );
        ObserverManager.instance.Register(
            $"{roomID};end_game",
            OnHandleEndGame
        );
        ObserverManager.instance.Register(
            $"{roomID};have_client_quit",
            OnHandleHaveClientQuit
        );
        ObserverManager.instance.Register(
            $"{AccountManager.instance.GetAccountId()};quit_room_complete",
            OnHandleQuitRoom
        );
        SetController(false);
        prepareGamePopup.Show(() => SetController(true));
    }

    public void SetController(bool active)
    {
        isEndGame = !active;
        SetTimerLabel(active);
    }

    public void SetTimerLabel(bool active)
    {
        timerLabel.gameObject.SetActive( active );
    }

    private void OnDisable()
    {
        ObserverManager.instance.Unregister(
            $"{roomID};update_new_frame",
            UpdateFrame
        );
        ObserverManager.instance.Unregister(
            $"{roomID};receive_coin",
            OnHandleReceiveCoin
        );
        ObserverManager.instance.Unregister(
            $"{roomID};change_bet",
            OnHandleChangeBet
        );
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void GeneratePlayer(JoinRoomData joinRoomData, DataInRoomGame dataInRoomGame)
    {
        List<AccountData> listAccountData = joinRoomData.accountDatas;
        for (int i = 0; i < listAccountData.Count; i++)
        {
            AccountData accountData = listAccountData[i];
            PlayerDataInRoom dataInRoom = dataInRoomGame.playerDatas[accountData.userID];
            if (accountData.userID.Equals(AccountManager.instance.GetAccountId()))
            {
                // Nếu là meMap
                Player playerTmp = Instantiate(playerPrefab);
                playerTmp.OnInit(dataInRoom.index + 1,
                    listPosPlayer[dataInRoom.index].position, coinMe, accountData.userID,
                    accountData.userName, dataInRoom.numberCoin
                );
                _player = playerTmp;
                listPlayer.Add(accountData.userID, playerTmp);
            }
            else
            {
                Player playerTmp = Instantiate(playerPrefab);
                playerTmp.OnInit(dataInRoom.index + 1,
                    listPosPlayer[dataInRoom.index].position, coinOther, accountData.userID,
                    accountData.userName, dataInRoom.numberCoin
                );
                listPlayer.Add(accountData.userID, playerTmp);
            }
        }
    }

    public void GenerateBullet(ItemDataServer bulletData)
    {
        var bullet = ObjectPoolManager.instance.Get(
            "Bullet",
            null,
            () => Instantiate(bulletPrefab)
        ).Init().SetDataServer(bulletData);
        Player player = listPlayer[bulletData.userID];
        player.RotationPlayer(bullet.TF.position);
        listBullets.Add(bulletData.itemID, bullet);
    }

    public void GenerateFish(ItemDataServer fishData)
    {
        var fish = ObjectPoolManager.instance.Get(
            FishFactory.instance.GetFishPoolID((FishType)fishData.itemType),
            null,
            () => FishFactory.instance.CreateFish<BaseFish>(
                (FishType)fishData.itemType,
                Vector3.zero,
                Quaternion.identity,
                null
            )
        ).Init().SetDataServer(fishData);
        listFishes.Add(fishData.itemID, fish);
    }

    public void UpdateFrame(string data)
    {
        Debug.Log("CO UPDATE");
        var platformData = data.ToObject<NewFrameGame>().data;
        UpdateAllItemInFrame(platformData);
        RemoveAllItemLeft(platformData);
    }

    private void UpdateAllItemInFrame(PlatformData platformData)
    {
        foreach (var bulletData in platformData.bullets)
        {
            if (listBullets.TryGetValue(bulletData.itemID, out var bullet))
            {
                bullet.SetDataServer(bulletData);
            }
            else
            {
                GenerateBullet(bulletData);
            }
        }

        foreach (var fishData in platformData.fishs)
        {
            if (listFishes.TryGetValue(fishData.itemID, out var fish))
            {
                fish.SetDataServer(fishData);
            }
            else
            {
                GenerateFish(fishData);
            }
        }
    }

    private void RemoveAllItemLeft(PlatformData platformData)
    {
        var newListFish = new Dictionary<string, BaseServerItem>();
        foreach (var keyValuePair in listFishes)
        {
            if (!platformData.fishs.Exists(
                    fishData => fishData.itemID == keyValuePair.Key
                )
               )
            {
                keyValuePair.Value.DestroyItem();
            }
            else
            {
                newListFish.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
        
        listFishes = newListFish;
        
        var newListBullet = new Dictionary<string, BaseServerItem>();
        
        foreach (var keyValuePair in listBullets)
        {
            if (!platformData.bullets.Exists(
                    bulletData => bulletData.itemID == keyValuePair.Key
                )
               )
            {
                keyValuePair.Value.DestroyItem();
            }
            else
            {
                newListBullet.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
        
        listBullets = newListBullet;
    }

    public void OnHandleReceiveCoin(string data)
    {
        ReceiveCoinData dataReceiveCoin = data.ToObject<ServerDataAccessObject<ReceiveCoinData>>().data;
        Player player = listPlayer[dataReceiveCoin.userID];
        Vector3 posStart = new Vector3(dataReceiveCoin.posX, dataReceiveCoin.posY, 0);
        Vector3 posEnd = player.TF.position;
        if (AccountManager.instance.GetAccountId().Equals(dataReceiveCoin.userID))
        {
            GenerateCoins(effectCoinMe, dataReceiveCoin.coinReceive, posStart, posEnd, () => player.ReceiveCoin(dataReceiveCoin.coinTotal), "Effect_Coin_Me");
        }
        else
        {
            GenerateCoins(effectCoinOther, dataReceiveCoin.coinReceive, posStart, posEnd, () => player.ReceiveCoin(dataReceiveCoin.coinTotal), "Effect_Coin_Other");
        }
    }
    
    public void GenerateCoins(EffectCoin effectCoin, int countCoin, Vector3 posStart, Vector3 posEnd, Action action, string idPoolName)
    {
        for (int i = 0; i < countCoin; i++)
        {
            float deltaX = Utils.RandomBetweenWithStep(-0.5f, 0.5f, 0.1f);
            float deltaY = Utils.RandomBetweenWithStep(-0.5f, 0.5f, 0.1f);
            posStart += new Vector3(deltaX, deltaY, 0);
            if (i == 0)
            {
                EffectCoin efCoin = ObjectPoolManager.instance.Get<EffectCoin>(idPoolName, null, () => Instantiate(effectCoin)).OnInit(posStart, posEnd, action);
            }
            else
            {
                EffectCoin efCoin = ObjectPoolManager.instance.Get<EffectCoin>(idPoolName, null, () => Instantiate(effectCoin)).OnInit(posStart, posEnd, null);
            }
        }
    }

    public void OnHandleChangeBet(string data)
    {
        ChangeBetData changeBetData = data.ToObject<ServerDataAccessObject<ChangeBetData>>().data;
        Player player = listPlayer[changeBetData.userID];
        if (player != null)
        {
            player.ChangeBetOther(changeBetData.index);
        }
    }

    public void OnHandleUpdateTimer(string data)
    {
        int timer = data.ToObject<ServerDataAccessObject<int>>().data;
        timerLabel.text = Utils.FormatTime(timer);
    }

    public void OnHandleEndGame(string data)
    {
        Debug.LogError("END GAME");
        isEndGame = true;
        isFireContinuous = false;
    }

    public void OnHandleHaveClientQuit(string data)
    {
        HaveClientQuitData haveClientQuitData = data.ToObject<ServerDataAccessObject<HaveClientQuitData>>().data;
        Player playerQuit = listPlayer[haveClientQuitData.userQuit];
        if (playerQuit != null)
        {
            listPlayer.Remove(haveClientQuitData.userQuit);
            Destroy(player.gameObject);
        }
    }

    public void OnHandleQuitRoom(string data)
    {
        Debug.LogError("QUIT ROOM SUCCESS");
    }

    private void Update()
    {
        if(isEndGame)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
            {
                return;
            }

            OnTouchStart();
        }

        if (Input.GetMouseButton(0))
        {
            if (IsPointerOverUI())
            {
                return;
            }

            OnTouchMove();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnTouchEnd();
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnTouchStart()
    {
        Debug.Log("Touch start");
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        _touchPoint.SetPos(worldPosition);
        _player.RotationPlayer(worldPosition);
        isFireContinuous = true;
        _touchPoint.Fire();
    }

    public void OnTouchMove()
    {
        Debug.Log("Touch move");
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        _touchPoint.SetPos(worldPosition);
        _player.RotationPlayer(worldPosition);
    }

    public void OnTouchEnd()
    {
        Debug.Log("Touch end");
        isFireContinuous = false;
        player.StopFireContinuous();
    }
}

[Serializable]
public class PlatformData
{
    public List<ItemDataServer> bullets;
    public List<ItemDataServer> fishs;
}

public class HaveClientQuitData
{
    public string userQuit;
    public string adminID;
}