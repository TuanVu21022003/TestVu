using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoginJob : LoadingJob
{
    public LoginJob(
        Action onStartJob,
        Action onFinishJob,
        params LoadingJob[] previousJobs
    ) : base(onStartJob, onFinishJob, previousJobs) { }

    protected override async UniTask Task()
    {
        Debug.Log("Loading Task login popup");
        PopupManager.instance.ShowPopup(
            PopupType.Login,
            initPopup: popup => popup.transform.localPosition = Vector3.zero
        );
        await UniTask.WaitUntil(() => AccountManager.instance.IsLoggedIn);
    }
}