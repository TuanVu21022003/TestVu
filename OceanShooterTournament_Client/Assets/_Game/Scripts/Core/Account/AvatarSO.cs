using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AvatarType
{
    S,
    M,
    L,
    XL
}

[Serializable]
public class AvatarStyle
{
    public AvatarType avatarType;
    public Sprite avatarSprite;
}

[Serializable]
public class Avatar
{
    public string avatarID;
    public Sprite avatarS;
    public Sprite avatarM;
    public Sprite avatarL;
    public Sprite avatarXL;
    public Sprite defaultAvatar;
}

[CreateAssetMenu(fileName = "Avatar", menuName = "Account/Avatar", order = 0)]
public class AvatarSO : ScriptableObject
{
    public List<Avatar> avatars;

    public Sprite GetAvatarFromID(string id, AvatarType type)
    {
        var avatar = avatars.Find(a => a.avatarID == id);
        if (avatar == null) return null;
        return type switch
               {
                   AvatarType.S => avatar.avatarS ?? avatar.defaultAvatar,
                   AvatarType.M => avatar.avatarM ?? avatar.defaultAvatar,
                   AvatarType.L => avatar.avatarL ?? avatar.defaultAvatar,
                   AvatarType.XL => avatar.avatarXL ?? avatar.defaultAvatar,
                   _ => avatar.defaultAvatar
               };
    }
}