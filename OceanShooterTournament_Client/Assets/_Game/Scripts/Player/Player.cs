using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseGame
{
    [SerializeField] private Wing wing;
    [SerializeField] private Weapon weapon;
    [SerializeField] private SpriteRenderer coinImg;
    [SerializeField] private Transform bgPlayer;
    [SerializeField] private Transform namePanel;
    [SerializeField] private Transform coinPanel;
    [SerializeField] private Transform changeLevelPanel;
    [SerializeField] private Button btnUpLevel;
    [SerializeField] private Button btnDownLevel;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI coinLabel;

    private int levelBet = 1;

    private bool isFire = false;

    private Coroutine fireContinuos;

    public string playerID;

    private void Start()
    {
        btnUpLevel.onClick.AddListener(() =>
        {
            OnClickUpLevel();
        });

        btnDownLevel.onClick.AddListener(() =>
        {
            OnClickDownLevel();
        });
    }

    public void OnInit(int id, Vector3 pos, Sprite coinSprite, string userID, string userName, int coin)
    {
        TF.position = pos;
        coinImg.sprite = coinSprite;
        SetPlayerByID(id);
        wing.OnInit(levelBet);
        weapon.OnInit(levelBet);
        CheckMeMap(AccountManager.instance.GetAccountId().Equals(userID));
        SetNameLabel(userName);
        SetCoinLabel(coin.ToString());
    }

    public void CheckMeMap(bool check)
    {
        changeLevelPanel.gameObject.SetActive(check);
    }

    public void SetPlayerByID(int id)
    {
        switch (id)
        {
            case 2:
                bgPlayer.localScale = new Vector3(-1, 1, 1);
                namePanel.localScale = new Vector3(-1.2f, 1, 1);
                coinPanel.localScale = new Vector3(-1.2f, 1, 1);
                break;
            case 3:
                TF.rotation = Quaternion.Euler(0, 0, 180);
                namePanel.localRotation = Quaternion.Euler(0, 0, 180);
                coinPanel.localRotation = Quaternion.Euler(0, 0, 180);
                changeLevelPanel.localRotation = Quaternion.Euler(0, 0, 180);
                break;
            case 4:
                TF.rotation = Quaternion.Euler(0, 0, 180);
                bgPlayer.localScale = new Vector3(-1, 1, 1);
                namePanel.localRotation = Quaternion.Euler(180, 0, 0);
                coinPanel.localRotation = Quaternion.Euler(180, 0, 0);
                changeLevelPanel.localRotation = Quaternion.Euler(0, 0, 180);
                break;
        }
    }

    public void ChangeLevelDisplay()
    {
        wing.ChangeWing(levelBet);
        weapon.ChangeWeapon(levelBet);
    }

    public void ChangeBetOther(int level)
    {
        levelBet = level;
        ChangeLevelDisplay();
    }

    public void OnClickUpLevel()
    {
        Debug.Log("Up Level Success");
        levelBet += 1;
        if(levelBet > 6)
        {
            levelBet = 1;
        }
        ChangeLevelDisplay();
        SendChangeBet(levelBet);
    }

    public void OnClickDownLevel()
    {
        Debug.Log("Down Level Success");
        levelBet -= 1;
        if (levelBet < 1)
        {
            levelBet = 6;
        }
        ChangeLevelDisplay();
        SendChangeBet(levelBet);
    }

    public void SendChangeBet(int levelBet)
    {
        NetworkManager.instance.Request("change_bet", $"{FishGameManager.instance.roomID},{levelBet}");
    }

    public void RotationPlayer(Vector3 pos)
    {
        Vector3 direction = (pos - TF.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        wing.RotateWing(angle);
        weapon.RotateWeapon(angle);

    }

    public void OnFire(Vector3? posPoint = null)
    {
        if(FishGameManager.instance.isFireContinuous)
        {
            if(posPoint == null)
            {
                posPoint = FishGameManager.instance.touchPoint.getPos();
            }
            Debug.Log("POS: " + posPoint.Value);
            //Bullet bullet = ObjectPoolManager.instance.Get<Bullet>("Bullet", null, () => Instantiate(bulletPrefab)).OnInit(TF.position, levelBet - 1, posPoint.Value);
            Vector3 direction = (posPoint.Value - TF.position).normalized;
            Vector3 posStart = TF.position + direction * 1.5f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            string mess = $"{AccountManager.instance.CreateToken("shoot")};{FishGameManager.instance.roomID},{posStart.x},{posStart.y},{direction.x},{direction.y},{angle},{levelBet - 1}";
            NetworkManager.instance.Request("shoot", $"{FishGameManager.instance.roomID},{posStart.x},{posStart.y},{direction.x},{direction.y},{angle},{levelBet - 1}");
            
            //DOVirtual.DelayedCall(FishGameManager.instance.timeContinuos, () => OnFire());
            fireContinuos = StartCoroutine(Utils.DelayedActionCoroutine(FishGameManager.instance.timeContinuos, () => OnFire()));
        }
    }

    public void StopFireContinuous()
    {
        if(fireContinuos != null)
        {
            StopCoroutine(fireContinuos);
            fireContinuos = null;
        }
    }

    public void EffectFire()
    {
        weapon.FireWeapon();
    }

    public void SetNameLabel(string name)
    {
        nameLabel.text = name;
    }

    public void SetCoinLabel(string name)
    {
        coinLabel.text = name;
    }

    public void ReceiveCoin(int coin)
    {
        StartCoroutine(Utils.IncrementValue(int.Parse(coinLabel.text), coin, 0.5f, SetCoinLabel));
    }

}

public class ReceiveCoinData
{
    public string userID { get; set; }
    public int coinReceive { get; set; }
    public int coinTotal { get; set; }
    public float posX { get; set; }
    public float posY { get; set; }
}

public class ChangeBetData
{
    public string userID { get; set; }
    public int index { get; set; }
}