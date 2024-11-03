using Core.StateMachine;
using UnityEngine;

public class CanvasMenuHome : MonoSingleton<CanvasMenuHome>
{
    [SerializeField] private HomeMenuUI _homeMenuUI;
    [SerializeField] private FindingScreen _findingScreen;
    private QueueScreenManager _queueScreenManager = new();

    private void OnEnable()
    {
        GameStateManager.OnGameStateChanged += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Loading:
                break;
            case GameState.Home:
                ChangeToHome();
                break;
            case GameState.Settings:
                break;
            case GameState.FindingScreen:
                ChangeToFindingScreen();
                break;
            case GameState.WaitingRoom:
                break;
            case GameState.Playing:
                break;
            default:
                return;
        }
    }

    public void ChangeToHome()
    {
        _queueScreenManager.ShowPopup(_homeMenuUI);
    }

    public void ChangeToFindingScreen()
    {
        _queueScreenManager.ShowPopup(_findingScreen);
    }
}