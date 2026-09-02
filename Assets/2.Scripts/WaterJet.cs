using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet : MonoBehaviour
{
    public float duration = 0.5f; // 물줄기가 얼마나 오래 유지될지

    private void Start()
    {
        Destroy(gameObject, duration); // 일정 시간 후 자동 제거
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("캐릭터가 물줄기에 맞았습니다!");
            GameManager.Instance.GameOver();
        }
    }
}
