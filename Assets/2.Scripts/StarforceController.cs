using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메이플 스타포스 미니 게임
public class StarforceController : MonoBehaviour
{
    public static StarforceController Instance; // 싱글톤 패턴 추가

    [Header("바 UI 연결")]
    [SerializeField] private RectTransform cursorRect;

    [Header("이동 범위 설정")]
    [SerializeField] private float leftEdgeX = -190f;  // 커서 왼쪽 끝 x좌표
    [SerializeField] private float rightEdgeX = 190f;   // 커서 오른쪽 끝 x좌표

    [Header("게임 설정")]
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField][Range(0f, 1f)] private float successZoneMin = 0.4f;
    [SerializeField][Range(0f, 1f)] private float successZoneMax = 0.6f;
    [SerializeField] private int targetSuccessCount = 4;

    private int currentSuccessCount = 0;
    private float moveRange; // 이동 가능한 전체 거리

    private void Awake() // 싱글톤 용 추가
    {
        Instance = this;
    }

    private void Start()
    {
        moveRange = rightEdgeX - leftEdgeX; // 왼쪽~오른쪽 사이 거리 자동 계산
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        MoveCursor();
        CheckInput();
    }

    private void MoveCursor()
    {
        float pingPongValue = Mathf.PingPong(Time.time * moveSpeed, moveRange); // 0 ~ moveRange
        float actualX = leftEdgeX + pingPongValue; // 왼쪽 끝 기준으로 실제 좌표 계산

        Vector2 pos = cursorRect.anchoredPosition;
        pos.x = actualX;
        cursorRect.anchoredPosition = pos;
    }

    private void CheckInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        // 현재 커서 위치를 0~1 비율로 변환
        float currentRatio = (cursorRect.anchoredPosition.x - leftEdgeX) / moveRange;

        if (currentRatio >= successZoneMin && currentRatio <= successZoneMax)
            OnSuccess();
        else
            OnFail();
    }

    public event System.Action<int> OnSuccessCountChanged;


    private void OnSuccess()
    {
        currentSuccessCount++;

        OnSuccessCountChanged?.Invoke(currentSuccessCount); // Starforce_Star에서 이벤트로 알림

        Debug.Log($"강화 성공! ({currentSuccessCount}/{targetSuccessCount})");

        if (currentSuccessCount >= targetSuccessCount)
            GameManager.Instance.GameClear();
    }

    private void OnFail()
    {
        Debug.Log("타이밍 실패, 다시 시도하세요");
    }
}
