using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

public abstract class LoadingJobExecutor
{
    public abstract UniTask Execute();
}

public abstract class LoadingJob : LoadingJobExecutor
{
    private readonly List<LoadingJob> _previousJobs;
    private readonly Action _onStartJob;
    private readonly Action _onFinishJob;
    public bool IsCompleted { get; private set; }

    protected LoadingJob(params LoadingJob[] previousJobs)
    {
        IsCompleted = false;
        _previousJobs = new List<LoadingJob>(previousJobs);
    }

    protected LoadingJob(IEnumerable<LoadingJob> previousJobs)
    {
        IsCompleted = false;
        _previousJobs = new List<LoadingJob>(previousJobs);
    }

    protected LoadingJob(
        Action onStartJob,
        Action onFinishJob,
        params LoadingJob[] previousJobs
    )
    {
        IsCompleted = false;
        _onStartJob = onStartJob;
        _onFinishJob = onFinishJob;
        _previousJobs = new List<LoadingJob>(previousJobs);
    }

    protected LoadingJob(
        Action onStartJob,
        Action onFinishJob,
        IEnumerable<LoadingJob> previousJobs
    )
    {
        IsCompleted = false;
        _onStartJob = onStartJob;
        _onFinishJob = onFinishJob;
        _previousJobs = new List<LoadingJob>(previousJobs);
    }

    public sealed override async UniTask Execute()
    {
        if (IsCompleted) return;
        IsCompleted = false;
        await UniTask.WaitUntil(() => _previousJobs?.All(job => job.IsCompleted) ?? true);
        _onStartJob?.Invoke();
        await Task();
        _onFinishJob?.Invoke();
        IsCompleted = true;
    }

    protected abstract UniTask Task();
}