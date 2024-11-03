using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public List<string> baseResourcesObject;
    public ProgressBar progressBar;
    private List<LoadingJob> _allLoadingSystem;

    private void Awake()
    {
        progressBar.Init();
        InitLoadingSystem();
        Loading().Forget();
    }

    private void InitLoadingSystem()
    {
        _allLoadingSystem = this.GenerateLoadingSystem();
    }

    private async UniTask Loading()
    {
        await UniTask.WhenAll(_allLoadingSystem.ConvertAll(job => job.Execute()));
    }

    public void UpdateProgress()
    {
        progressBar.UpdateTarget(1f / _allLoadingSystem.Count);
    }
}