using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum OverallState
{
    Playing,
    AllClear,   // 모든 미니게임 클리어
    GameOver    // 목숨 소진
}

/// <summary>
/// 해당 스크립트는 게임 전체를 아우르는 매니저로 사용하는 스크립트이다, 여기에 게임 클리어, 게임 오버를 "보고"한다는 느낌으로 사용한다
/// </summary>
public class OverallGameManager : MonoBehaviour
{
    public static OverallGameManager Instance;

    public int maxLives = 4; // 목숨 값
    public int CurrentLives { get; private set; } //현재 목숨 : 프로퍼티로 해서 값은 가져오데 여기서 변환 안되게끔

    public OverallState CurrentOverallState { get; private set; } = OverallState.Playing;

    // 이벤트 함수, 이벤트 선언
    public event Action<int> OnLivesChanged;   // 목숨이 바뀔 때마다 (남은 목숨 전달)
    public event Action OnOverallGameOver;      // 목숨 0 -> 전체 게임오버
    public event Action OnOverallClear;         // 모든 미니게임 클리어
    
    private void Awake()
    {
        Instance = this;
        CurrentLives = maxLives;
    }

    // 미니게임이 실패했을 때 호출
    public void OnMiniGameFail()
    {
        if (CurrentOverallState != OverallState.Playing) return;

        CurrentLives--;
        OnLivesChanged?.Invoke(CurrentLives);

        if (CurrentLives <= 0)
        {
            CurrentOverallState = OverallState.GameOver; // 상태값을 변경, 현재 오버롤상태를 = 게임 오버 상태로
            OnOverallGameOver?.Invoke(); // 구독자들에게 능동적으로 알림을 보내는
            Debug.Log("전체 게임 오버! 목숨을 모두 소진했습니다.");
        }
        else
        {
            Debug.Log($"미니게임 실패! 남은 목숨: {CurrentLives}");
            // TODO: 다음 미니게임으로 전환 로직 (Additive Scene Loading 붙일 때 연결)
        }
    }

    // 미니게임이 성공했을 때 호출
    public void OnMiniGameClear()
    {
        if (CurrentOverallState != OverallState.Playing) return;

        Debug.Log("미니게임 성공!");
        // TODO: 다음 미니게임으로 전환, 혹은 마지막 미니게임이면 AllClear 처리
    }
}
