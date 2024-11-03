using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using static CONST;

public static class GameEditor
{
    // Play  Loading Scene  in any scene by ctrl +9
    [MenuItem("Tools/PlayFromStartupScene %9")]
    public static void PlayFromStartupScene()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }

        EditorPrefs.SetString("lastLoadedScenePath", SceneManager.GetActiveScene().path);
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
        EditorApplication.isPlaying = true;
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(
                "Assets/_Game/Scenes/" + sceneName + ".unity"
            );
        }
    }

    // Shift + 1  -> open Loading scene
    [MenuItem("Tools/OpenLoadingScene #1")]
    public static void OpenLoadingScene()
    {
        OpenScene(SCENE_LOADING);
    }

    // Shift + 2  -> open Main scene
    [MenuItem("Tools/OpenMainScene #2")]
    public static void OpenMainScene()
    {
        OpenScene(SCENE_MAIN);
    }
    
    // Shift + 3  -> open Home scene
    [MenuItem("Tools/OpenHomeScene #3")]
    public static void OpenHomeScene()
    {
        OpenScene(SCENE_GAMEPLAY);
    }
}