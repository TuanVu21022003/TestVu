using DG.Tweening;
using DragonBones;
using UnityEngine;

public abstract class BaseFish : BaseServerItem
{
    [SerializeField] protected UnityArmatureComponent _armatureComponent;

    public float targetScale = 0.1f; // Desired scale factor
    public float fadeDuration = 0.5f; // Duration for scaling and fading

    public override BaseServerItem Init()
    {
        TF.localScale = Vector3.zero;
        TF.DOScale(1, 0);
        return this;
    }

    public override BaseServerItem SetDataServer(ItemDataServer itemData)
    {
        base.SetDataServer(itemData);
        _armatureComponent.armature.flipY = itemData.velocity.x < 0;
        return this;
    }

    public override void DestroyItem()
    {
        TF.DOScale(targetScale, fadeDuration).OnComplete(base.DestroyItem);
    }
}