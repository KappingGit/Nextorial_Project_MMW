using UnityEngine;

[ExecuteAlways] // 에디터에서 플레이 안 해도 즉시 반영되게 함
public class SuccessZoneVisualizer : MonoBehaviour
{
    [SerializeField] private RectTransform barRect;
    [SerializeField] private RectTransform successZoneRect; // 성공 구간을 표시할 이미지
    [SerializeField][Range(0f, 1f)] private float successZoneMin = 0.4f;
    [SerializeField][Range(0f, 1f)] private float successZoneMax = 0.6f;

    private void Update()
    {
        if (barRect == null || successZoneRect == null) return;

        float barWidth = barRect.rect.width;

        float startX = (barWidth * successZoneMin) - (barWidth * 0.5f);
        float endX = barWidth * successZoneMax;
        float zoneWidth = endX - startX;

        // 성공 구간 이미지의 위치와 크기를 계산값에 맞춰 자동 조절
        successZoneRect.anchoredPosition = new Vector2(startX + zoneWidth * 0.5f, successZoneRect.anchoredPosition.y);
        successZoneRect.sizeDelta = new Vector2(zoneWidth, successZoneRect.sizeDelta.y);
    }
}