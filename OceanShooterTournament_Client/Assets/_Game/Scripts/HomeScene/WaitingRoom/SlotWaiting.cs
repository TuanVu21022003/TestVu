using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotWaiting : MonoBehaviour
{
    [SerializeField] protected TMP_Text playerName;
    [SerializeField] protected Image playerAvatar;
    [SerializeField] protected TMP_Text playerLevel;
    [SerializeField] protected GameObject levelIcon;
    [SerializeField] protected GameObject addPlayerIcon;
    [SerializeField] protected GameObject accessBg;

    public void Init(AccountData accountData)
    {
        SetIsHavePlayer(true);
        SetPlayerInfo(
            accountData.userName,
            AccountManager.instance.GetAvatarFromId(accountData.avatarID, AvatarType.XL),
            accountData.userLevel
        );
    }

    public void SetIsHavePlayer(bool isHavePlayer)
    {
        levelIcon.SetActive(isHavePlayer);
        addPlayerIcon.SetActive(!isHavePlayer);
        playerAvatar.gameObject.SetActive(isHavePlayer);
        playerLevel.gameObject.SetActive(isHavePlayer);
        playerName.gameObject.SetActive(isHavePlayer);
        accessBg.SetActive(isHavePlayer);
    }

    private void SetPlayerInfo(string name, Sprite avatar, int level)
    {
        playerName.text = name;
        playerAvatar.sprite = avatar;
        playerLevel.text = level.ToString();
    }
}