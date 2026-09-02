using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] heart; // 하트 오브젝트를 순서대로 넣기

    private void OnEnable()
    {
        OverallGameManager.Instance.OnLivesChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        OverallGameManager.Instance.OnLivesChanged -= UpdateHearts;
    }

    private void Start()
    {
        // 게임 시작시 초기 목숨 상태를 바로 반영해두기
        UpdateHearts(OverallGameManager.Instance.CurrentLives); //Start()에서 한 번 더 UpdateHearts 호출하는 이유: OnLivesChanged 이벤트는 목숨이 "변화"할 때만 발동
    }

    private void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < heart.Length; i++)
        {
            heart[i].SetActive(i < currentLives);
        }
    }
}
