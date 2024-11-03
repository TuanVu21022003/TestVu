using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaitForSecondJob : LoadingJob
{
    private float _secondTime;

    public WaitForSecondJob(
        float secondTime,
        params LoadingJob[] previousJobs
    ) : base(previousJobs)
    {
        _secondTime = secondTime;
    }

    public WaitForSecondJob(
        float secondTime,
        Action onStartJob,
        Action onFinishJob,
        params LoadingJob[] previousJobs
    ) : base(onStartJob, onFinishJob, previousJobs)
    {
        _secondTime = secondTime;
    }

    protected override UniTask Task()
    {
        Debug.Log($"Loading Task wait for {_secondTime} seconds");
        return UniTask.Delay((int)(_secondTime * 1000));
    }
}