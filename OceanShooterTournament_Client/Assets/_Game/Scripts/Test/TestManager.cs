using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestManager : MonoSingleton<TestManager>
{
    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnLogout;
    [SerializeField] private Slider slide;
    [SerializeField] private TextMeshPro userIDText;
    [SerializeField] private SpriteRenderer stateColor;

    public string userID;

    private void Start()
    {
        btnLogin.onClick.AddListener(() =>
        {
            ClientManager.instance.client.SendMessage("client;login");
        });

        btnLogout.onClick.AddListener(() =>
        {
            TouchLogOut();
        });
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            slide.value = progressValue;
            yield return null;  
        }
    }

    public void UpdateUserID(string userId)
    {
        userID = userId;
        userIDText.text = userId;
    }

    public void UpdateStateColor(bool active)
    {
        if(active)
        {
            stateColor.color = Color.green;
        }
        else
        {
            stateColor.color = Color.red;
        }
    }

    public void TouchLogOut()
    {
        string mess = userID + ";" + "logout";
        ClientManager.instance.client.SendMessage(mess);
    }

    private void OnApplicationQuit()
    {
        TouchLogOut();
        ClientManager.instance.client.OnApplicationQuit();
    }
}
