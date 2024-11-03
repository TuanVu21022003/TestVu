using System;
using System.Collections.Generic;
using System.Linq;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CONST;

public class LoadMainScene : LoadingJob
{
    private readonly List<LoadingJob> _waitingJobs;

    public LoadMainScene(
        IEnumerable<LoadingJob> waitingJobs,
        Action onStartJob,
        Action onFinishJob,
        params LoadingJob[] previousJobs
    ) : base(onStartJob, onFinishJob, previousJobs)
    {
        _waitingJobs = new List<LoadingJob>(waitingJobs);
    }

    protected override async UniTask Task()
    {
        Debug.Log("Loading Task Main Scene");
        var sceneAsync = SceneManager.LoadSceneAsync(SCENE_MAIN);
        if (sceneAsync != null) sceneAsync.allowSceneActivation = false;
        await UniTask.WaitUntil(() => _waitingJobs?.All(job => job.IsCompleted) ?? true);
        if (sceneAsync != null && GameStateManager.instance.ChangeGameState(GameState.Home))
        {
            sceneAsync.allowSceneActivation = true;
        }
    }
}