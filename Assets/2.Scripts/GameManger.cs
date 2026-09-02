using UnityEngine;
using System;

public enum GameState
{
    Ready,     // 시작 전 대기
    Playing,   // 진행 중
    Clear,     // 클리어
    GameOver   // 실패
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; } = GameState.Ready;

    public float timeLimit = 20f;   // 제한시간 (초)
    private float currentTime;

    // 다른 스크립트가 상태 변화를 구독할 수 있도록 이벤트로 알림
    public event Action OnGameClear;
    public event Action OnGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        currentTime = timeLimit;
        CurrentState = GameState.Playing;
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing) return; // 진행 중이 아니면 아무것도 안 함

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            GameClear();
        }
    }

    public void GameClear()
    {
        if (CurrentState != GameState.Playing) return; // 중복 호출 방지

        CurrentState = GameState.Clear;
        Debug.Log("게임 클리어! 생존 성공");
        OnGameClear?.Invoke();
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing) return; // 중복 호출 방지

        CurrentState = GameState.GameOver;
        Debug.Log("게임 오버! 물줄기에 맞음");
        OnGameOver?.Invoke();
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }
}
