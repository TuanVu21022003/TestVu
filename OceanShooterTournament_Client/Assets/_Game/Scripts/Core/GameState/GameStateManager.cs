using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Core.StateMachine
{
    public enum GameState
    {
        Loading,
        Home,
        Settings,
        FindingScreen,
        WaitingRoom,
        LoadingGamePlay,
        Playing,
    }

    public class GameStateManager : MonoSingletonDontDestroyOnLoad<GameStateManager>
    {
        private const float StateLockedTime = 0.4f;

        private static readonly Dictionary<GameState, List<GameState>>
                StatesMapFromKeyToValue =
                        new()
                        {
                            {
                                GameState.Home,
                                new List<GameState>
                                {
                                    GameState.FindingScreen,
                                    GameState.Settings,
                                }
                            },
                            {
                                GameState.Settings,
                                new List<GameState>
                                {
                                    GameState.Home,
                                }
                            },
                            {
                                GameState.Playing,
                                new List<GameState>
                                {
                                    GameState.Home
                                }
                            },
                            {
                                GameState.Loading,
                                new List<GameState>
                                {
                                    GameState.Home,
                                }
                            },
                            {
                                GameState.FindingScreen,
                                new List<GameState>
                                {
                                    GameState.Home,
                                    GameState.WaitingRoom,
                                }
                            },
                            {
                                GameState.WaitingRoom,
                                new List<GameState>
                                {
                                    GameState.FindingScreen,
                                    GameState.LoadingGamePlay,
                                }
                            },
                            {
                                GameState.LoadingGamePlay,
                                new List<GameState>
                                {
                                    GameState.Playing,
                                }
                            }
                        };

        public static event Action<GameState> OnGameStateChanged;
        private bool stateLocked;
        private bool pausedByAppSuspended;
        public GameState CurrentState { get; private set; } = GameState.Loading;

        public GameState LastState { get; private set; } = GameState.Loading;

        public bool ChangeGameState
        (
            GameState nextGameState,
            bool force = false,
            Action onStart = null
        )
        {
            if (stateLocked && !force)
            {
                return false;
            }

            if (!StatesMapFromKeyToValue[CurrentState].Contains(nextGameState))
            {
                return false;
            }

            if (CurrentState == nextGameState)
            {
                return false;
            }

            onStart?.Invoke();
            LastState = CurrentState;
            CurrentState = nextGameState;
            StartCoroutine(ResetStateLockInTime(StateLockedTime));
            if (nextGameState == GameState.Home)
            {
                stateLocked = true;
            }

            OnGameStateChanged?.Invoke(nextGameState);
            return true;
        }

        IEnumerator ResetStateLockInTime(float sec)
        {
            yield return new WaitForSecondsRealtime(sec);
            stateLocked = false;
        }
    }
}