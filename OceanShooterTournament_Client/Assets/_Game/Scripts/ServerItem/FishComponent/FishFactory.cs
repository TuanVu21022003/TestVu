using System.Linq;
using UnityEngine;

public class FishFactory : MonoSingletonDontDestroyOnLoad<FishFactory>
{
    [SerializeField] private FishConfigSO fishConfigSO;

    public T CreateFish<T>(
        FishType fishType,
        Vector3 spawnPoint,
        Quaternion rotation,
        Transform parent
    ) where T : BaseFish
    {
        return fishConfigSO.CreateFish<T>(
            fishType,
            spawnPoint,
            rotation,
            parent
        );
    }

    public string GetFishPoolID(FishType fishType)
    {
        return fishConfigSO.listFishConfigs
                           .FirstOrDefault(fishConfig => fishConfig.fishType == fishType)
                           ?.fishPrefabs.GetPoolID();
    }
}