using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnInterval = 1.0f; // 몇 초마다 물풍선을 생성할지

    private void Awake()
    {
        InvokeRepeating(nameof(SpawnBalloon), 1f, spawnInterval);
    }

    private void SpawnBalloon()
    {
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
}
