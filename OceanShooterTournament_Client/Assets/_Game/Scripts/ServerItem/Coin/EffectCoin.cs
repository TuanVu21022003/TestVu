using DG.Tweening;
using DragonBones;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ANIM_COIN
{
    public static string DROP = "drop";
    public static string ROTATION = "rotation";
}

public class EffectCoin : BaseGame, IGetPoolID
{
    [SerializeField] private UnityArmatureComponent dataCoin;
    [SerializeField] private string idPoolCoin;

    private Vector3 endPos;
    private float duration = 1f;

    private Action action;
    public EffectCoin OnInit(Vector3 posStart, Vector3 endPos, Action action)
    {
        this.action = action;
        TF.position = posStart;
        this.endPos = endPos;
        dataCoin.animation.Play(ANIM_COIN.DROP);

        // Add listener for the animation complete event
        dataCoin.AddDBEventListener(EventObject.COMPLETE, OnDropComplete);
        return this;
    }

    public void OnDropComplete(string type, EventObject eventObject)
    {
        if (eventObject.animationState.name.Equals(ANIM_COIN.DROP))
        {
            dataCoin.animation.Play(ANIM_COIN.ROTATION);
            MoveToPlayer();
        }
    }

    public void MoveToPlayer()
    {
        Vector3 startPoint = TF.position;

        // Calculate the midpoint with added height for the arc
        Vector3 midPoint = (startPoint + endPos) / 2;
        midPoint.y += 2f; // Adjust the height to create the parabola

        // Use a sequence to move along a parabolic path
        Sequence parabolicSequence = DOTween.Sequence();
        parabolicSequence.Append(transform.DOPath(new Vector3[] { startPoint, midPoint, endPos }, duration, PathType.CatmullRom)
            .SetEase(Ease.InCubic))
            .OnComplete(() => DestroyEffect()); ;

    }

    public void DestroyEffect()
    {
        action?.Invoke();
        ObjectPoolManager.instance.Push(this);
    }

    public string GetPoolID()
    {
        return idPoolCoin;
    }
}
