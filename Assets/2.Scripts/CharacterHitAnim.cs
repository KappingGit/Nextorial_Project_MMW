using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//던파 캐릭터 
public class CharacterHitAnim : MonoBehaviour
{
    [SerializeField] private GameObject idleImage; // 평상시 이미지
    [SerializeField] private GameObject hitImage;   // 타격 모션 이미지
    [SerializeField] private float hitDuration = 0.1f; // 타격 이미지가 보이는 시간

    private Coroutine hitCoroutine;

    public void PlayHitMotion()
    {
        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine); // 이미 재생 중이면 처음부터 다시

        hitCoroutine = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        idleImage.SetActive(false);
        hitImage.SetActive(true);

        yield return new WaitForSeconds(hitDuration);

        hitImage.SetActive(false);
        idleImage.SetActive(true);
    }
}
