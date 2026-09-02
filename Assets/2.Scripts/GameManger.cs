using UnityEngine;
using System;

// 게임의 상태를 표시
public enum GameState
{
    Ready,     // 시작 전 대기
    Playing,   // 진행 중
    Clear,     // 클리어
    GameOver   // 실패
}

// 시간이 다 됐을 때 어떻게 처리할지(규칙을 정하기, 미니 게임마다 제한 시간이 도달했을 때의 결과가 달라지니깐)
public enum TimeUpBehavior
{
    Clear,    // 시간 종료 = 성공 (물풍선 게임) (주의 사항 : GameState와 잘 구분할 것!)
    GameOver  // 시간 종료 = 실패 (연타 게임)
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; } = GameState.Ready;

    [SerializeField] private float timeLimit = 10f;   // 제한시간 (초)
    /// <summary>
    /// 해당 미니게임이 제한시간 조건이 다를 때 설정 : 제한시간에 도달했을 때 클리어일 결우 Clear로 변경
    /// </summary>
    [SerializeField][Tooltip("제한시간에 도달했을 때 결과값 규칙 설정")] private TimeUpBehavior timeUpBehavior = TimeUpBehavior.Clear;
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

            if (timeUpBehavior == TimeUpBehavior.Clear)
            {
                GameClear();
            } 
            else
            {
                GameOver();
            }

        }
    }

    public void GameClear()
    {
        if (CurrentState != GameState.Playing) return; // 중복 호출 방지

        CurrentState = GameState.Clear;
        Debug.Log("게임 클리어!");
        OnGameClear?.Invoke();

        OverallGameManager.Instance.OnMiniGameClear(); // 게임 클리어를 OverallGameManager의 스크립트에서 호출
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing) return; // 중복 호출 방지(현재상태CurrentState가 플레이 중이라면 여기 조건문을 통과한다고 읽는게 좋다)
        // 즉, 플레이 중이 아니라면 이 함수는 리턴을 받고 나가진다(아래 코드는 실행이 안된다.)

        CurrentState = GameState.GameOver; // 현재 상태를 게임 오버로 처리
        Debug.Log("게임 오버!");
        OnGameOver?.Invoke();

        OverallGameManager.Instance.OnMiniGameFail();  // 게임 클리어를 OverallGameManager의 스크립트에서 호출
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }
}
