using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class LoadBaseGameResources : LoadingJob
{
    private List<string> _baseResourcesObjectName;

    public LoadBaseGameResources(
        List<string> baseResourcesObjectName,
        params LoadingJob[] previousJobs
    ) : base(previousJobs)
    {
        _baseResourcesObjectName = baseResourcesObjectName;
    }

    public LoadBaseGameResources(
        List<string> baseResourcesObjectName,
        Action onStartJob,
        Action onFinishJob,
        params LoadingJob[] previousJobs
    ) : base(onStartJob, onFinishJob, previousJobs)
    {
        _baseResourcesObjectName = baseResourcesObjectName;
    }

    protected override async UniTask Task()
    {
        await UniTask.WhenAll(_baseResourcesObjectName.ConvertAll(LoadObject));
    }
    
    private async UniTask<GameObject> LoadObject(string path)
    {
        Debug.Log($"Loading Task object: {path}");
        var resource = await Resources.LoadAsync<GameObject>(path);
        var objectRef = Object.Instantiate(resource) as GameObject;
        return objectRef;
    }
}