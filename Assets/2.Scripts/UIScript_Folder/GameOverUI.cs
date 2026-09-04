using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [Header("연출 대상")]
    [SerializeField] private RectTransform gameOverText;

    [Header("위치 설정")]
    [SerializeField] private Vector2 startPos = new Vector2(0, 600f); // 하면 위 바깥
    [SerializeField] private Vector2 endPos = new Vector2(0, 0f); // 최종 도착 위치

    [Header("연출 시간")]
    [SerializeField] private float slideDuration = 1f;

    public IEnumerator SlideDown()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.anchoredPosition = startPos;

        float elapsed = 0f; //경과 시간

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slideDuration; // 0~1로 정규화된 진행률

            gameOverText.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            yield return null; // 다음 프레임 까지 대기 -> 매 프레임 조금씩 이동
        }

        gameOverText.anchoredPosition = endPos; // 오차 보정, 정확히 목표 위치로 고정
    }
}
