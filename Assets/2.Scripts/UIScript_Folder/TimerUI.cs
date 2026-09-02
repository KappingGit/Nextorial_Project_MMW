using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return; // 게임 중이 아니라면 여기 함수 실행하지 마라 뜻

        float remaining = GameManager.Instance.GetRemainingTime();
        int displaySeconds = Mathf.FloorToInt(remaining); // 소수점의 숫자를 올림으로 표시

        timerText.text = displaySeconds.ToString();
    }
}
