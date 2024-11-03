using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : BaseGame
{
    [SerializeField] private UnityArmatureComponent dataTouch;

    public void SetPos(Vector3 pos)
    {
        TF.position = pos;
        dataTouch.animation.Play("newAnimation");
    }

    public void Fire()
    {
        FishGameManager.instance.player.OnFire(TF.position);
    }

    public Vector3 getPos()
    {
        return TF.position;
    }
}
