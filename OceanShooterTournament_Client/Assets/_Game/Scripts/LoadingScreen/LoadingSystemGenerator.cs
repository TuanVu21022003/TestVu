using System.Collections.Generic;

public static class LoadingSystemGenerator
{
    public static List<LoadingJob> GenerateLoadingSystem(this LoadingScreen loadingScreen)
    {
        var listLoadingJobs = new List<LoadingJob>();

        var loadBaseGameResources = new LoadBaseGameResources(
            loadingScreen.baseResourcesObject,
            null,
            loadingScreen.UpdateProgress
        );
        listLoadingJobs.Add(loadBaseGameResources);

        var wait1Seconds = new WaitForSecondJob(
            1f,
            null,
            loadingScreen.UpdateProgress
        );
        listLoadingJobs.Add(wait1Seconds);

        var logIn = new LoginJob(
            null,
            loadingScreen.UpdateProgress,
            wait1Seconds,
            loadBaseGameResources
        );
        listLoadingJobs.Add(logIn);

        var loadMainScene = new LoadMainScene(
            listLoadingJobs,
            loadingScreen.UpdateProgress,
            null,
            loadBaseGameResources
        );
        listLoadingJobs.Add(loadMainScene);

        return listLoadingJobs;
    }
}