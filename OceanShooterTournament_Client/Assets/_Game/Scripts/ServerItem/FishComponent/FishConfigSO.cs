using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FishConfig
{
    public FishType fishType;
    public BaseFish fishPrefabs;
}

[CreateAssetMenu(fileName = "FishConfig", menuName = "Data/Fish/FishConfig")]
public class FishConfigSO : ScriptableObject
{
    public List<FishConfig> listFishConfigs;

    public T CreateFish<T>(
        FishType fishType,
        Vector3 spawnPoint,
        Quaternion rotation,
        Transform parent
    ) where T : BaseFish
    {
        if (listFishConfigs.FirstOrDefault(config => fishType == config.fishType)
            is not { } fishConfig)
        {
            return null;
        }

        if (fishConfig.fishPrefabs is not T)
        {
            return null;
        }

        var fish = Instantiate(fishConfig.fishPrefabs, spawnPoint, rotation, parent) as T;
        return fish;
    }
}

public enum FishType
{
    Ankangyu,
    Baolongwang,
    Baoxiang,
    Denglongyu,
    Doubaoyu,
    Douxia,
    Haidaochuan,
    Haigui,
    Haitun,
    HBBYShuimu,
    Hetun,
    Hongshanyu,
    Huangjunchutousha,
    Hudieyu,
    Jiewenyu,
    Jinjiahusha,
    Jinlongwang,
    Jinlongyu,
    Longliyu,
    Meirenyu,
    Meirenyu2,
    Shenhaifufenwang,
    Shiziyu,
    Tuobaoguiwang,
    Xiaochouyu,
    Zhangyu,
    Zhangyuwang,
}