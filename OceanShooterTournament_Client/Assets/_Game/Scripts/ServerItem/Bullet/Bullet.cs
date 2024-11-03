using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseServerItem
{
    [SerializeField] private SpriteRenderer bulletSprite;
    [SerializeField] private List<Sprite> listSprites = new();
    [SerializeField] private EffectBullet effectBulletPrefab;

    private Vector3 directionBullet;

    public override BaseServerItem SetDataServer(ItemDataServer itemData)
    {
        base.SetDataServer(itemData);
        bulletSprite.sprite = listSprites[itemType];
        return this;
    }

    public override void DestroyItem()
    {
        base.DestroyItem();
        SpawnEffectDestroy();
    }

    private void SpawnEffectDestroy()
    {
        ObjectPoolManager.instance
                         .Get(
                             "Effect_Bullet",
                             null,
                             () => Instantiate(effectBulletPrefab)
                         ).OnInit(itemType + 1, TF.position);
    }
}