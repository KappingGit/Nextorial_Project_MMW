using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Starforce_Star : MonoBehaviour
{
    [SerializeField] GameObject[] star; // 강화 성공 했을 때 뜨는 별

    private void OnEnable()
    {
        StarforceController.Instance.OnSuccessCountChanged += SuccessStar;
    }

    private void OnDisable()
    {
        StarforceController.Instance.OnSuccessCountChanged -= SuccessStar;
    }

    private void Start()
    {
        SuccessStar(0); // 시작 시 0개 성공 상태로 초기화 (별 전부 꺼짐)
    }

    private void SuccessStar(int currentStar)
    {
        for (int i = 0; i< star.Length; i++)
        {
            star[i].SetActive(i < currentStar);
        }
    }
}
