using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBullet : BaseGame, IGetPoolID
{
    private UnityArmatureComponent dataEffect;
    private int typeEffect;

    string effectBulletBasePath = "Bullet/EFBullet/";
    public EffectBullet OnInit(int index, Vector3 pos)
    {
        TF.position = pos;
        typeEffect = index;
        GenerateEffect(index);
        return this;
    }

    public void GenerateEffect(int index)
    {
        if (index < 1 || index > 6)
        {
            Debug.LogError("Invalid wing index! Must be between 1 and 6.");
            return;
        }

        LoadEffectWithAppear(index);
    }

    public void LoadEffectWithAppear(int index)
    {
        // Load the new wing data
        string dragonBonesJSONPath = $"{effectBulletBasePath}kyby_zidan{index}-hit_ske";
        string textureAtlasJSONPath = $"{effectBulletBasePath}kyby_zidan{index}-hit_tex";
        Utils._LoadData(dragonBonesJSONPath, textureAtlasJSONPath);
        this.dataEffect = UnityFactory.factory.BuildArmatureComponent($"kyby_zidan{index}-hit", "", "", "", this.gameObject);
        dataEffect.sortingMode = SortingMode.SortByOrder;
        dataEffect.sortingLayerName = "TopLayerTable";
        dataEffect.sortingOrder = 3;
        // Play the appear animation
        dataEffect.animation.Play($"kyby_zidan{index}-hit");
        Invoke(nameof(DestroyEffect), 0.5f);

    }

    public void DestroyEffect()
    {
        ObjectPoolManager.instance.Push(this);
    }

    public string GetPoolID()
    {
        return "Effect_Bullet";
    }
}
