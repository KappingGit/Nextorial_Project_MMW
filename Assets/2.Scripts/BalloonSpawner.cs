using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnInterval = 1.0f; // 몇 초마다 물풍선을 생성할지
    private bool canSpawn = true; // 물풍선을 스폰한다의 변수

    private void Start()
    {
        InvokeRepeating(nameof(SpawnBalloon), 1f, spawnInterval);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += StopSpawning;
        GameManager.Instance.OnGameClear += StopSpawning;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= StopSpawning;
        GameManager.Instance.OnGameClear -= StopSpawning;
    }

    private void SpawnBalloon()
    {
        if (!canSpawn) return; // 정지 상태면 생성 자체를 스킵

        Vector2Int? emptyCell = GridManager.Instance.GetRandomEmptyCell();

        if (emptyCell == null) return; // 빈 칸 없으면 이번엔 생성 스킵

        Vector2Int gridPos = emptyCell.Value;
        Vector2 worldPos = GridManager.Instance.GridToWorld(gridPos);

        GameObject balloon = Instantiate(balloonPrefab, worldPos, Quaternion.identity);
        GridManager.Instance.SetOccupied(gridPos, true);

        // 물풍선 스크립트에 자신의 그리드 좌표를 전달 (터질 때 칸을 다시 비우기 위해)
        Balloon balloonScript = balloon.GetComponent<Balloon>();
        balloonScript.Init(gridPos);
    }

    private void StopSpawning()
    {
        canSpawn = false;
        CancelInvoke(nameof(SpawnBalloon)); // 예약된 반복 생성 자체도 완전히 중단
    }
}
