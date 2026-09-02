using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private Image gaugeImage;
    [SerializeField] private float maxGauge = 100f;
    [SerializeField] private float damagePerHit = 8f; // 한 번 누를 때 깍이는 양

    [SerializeField] private CharacterHitAnim hitAnim; // 인스펙터에서 연결

    private float currentGauge;
    private bool isGaugeEmpty = false;

    private void Start()
    {
        currentGauge = maxGauge;
        gaugeImage.fillAmount = 1f; // 시작은 꽉 찬 상태 (100%)
    }

    private void Update()
    {
        KeySetup();
    }

    private void OnHit()
    {
        currentGauge -= damagePerHit;
        currentGauge = Mathf.Max(currentGauge, 0); // 음수로 내려가지 않게

        gaugeImage.fillAmount = currentGauge / maxGauge; // 0~1 비율로 변환

        // 여기서 캐릭터 타격 모션 트리거 호출 예정
        hitAnim.PlayHitMotion(); // 스페이스바 누를 때마다 타격 모션 재생(현재 이미지로 대체되어 있음)

        if (currentGauge <= 0)
        {
            isGaugeEmpty = true;
            GameManager.Instance.GameClear(); // 게이지 다 닳으면 즉시 클리어!
        }
    }

    private void KeySetup()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;
        if (isGaugeEmpty) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnHit();
        }
    }

}
