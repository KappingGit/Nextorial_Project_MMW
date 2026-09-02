using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private Vector2Int gridPos;
    public float timeToExplode = 2.0f; // 터지는 경과 시간

    public GameObject waterJetPrefab; // 물줄기 프리팹
    public int explosionRange = 2; // 물줄기가 몇 칸 뻗어나가나

    // 물줄기가 터질 때 상하좌우 4방향을 미리 정의
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),  // 위
        new Vector2Int(0, -1), // 아래
        new Vector2Int(-1, 0), // 왼쪽
        new Vector2Int(1, 0)   // 오른쪽
    };

    public void Init(Vector2Int pos)
    {
        gridPos = pos;
        // "timeToExplode초 뒤에 Explode() 실행해줘"
        Invoke(nameof(Explode), timeToExplode); //*nameof(Explode)**는 "진짜 존재하는 Explode라는 함수/변수의 이름을 문자열로 뽑아줘"라는 뜻,컴파일 에러로 바로 알려줌
    }

    /// <summary>
    /// 물풍선이 터지는
    /// </summary>
    private void Explode()
    {
        //물줄기 관련
        // 물풍선 자기 자신 위치에도 물줄기 생성
        SpawnWaterJet(gridPos);

        // 4방향으로 사거리만큼 뻗어나가며 물줄기 생성 : [왜 for 대신 foreach를 썼나: directions 배열은 "몇 번째"가 중요한 게 아니라 "이 안의 모든 방향"이 중요]
        foreach (Vector2Int dir in directions) //directions는 4방향 배열을 의미한다, "이 배열 안에 있는 걸 하나씩"이라는 뜻
        {
            for (int i = 1; i <= explosionRange; i++)
            {
                Vector2Int targetPos = gridPos + dir * i; // 방향 * 몇 칸째

                // 그리드 범위를 벗어나면 그 방향은 더 이상 진행하지 않음
                if (!GridManager.Instance.IsInsideGrid(targetPos))
                    break;

                SpawnWaterJet(targetPos);
            }
        }

        //
        GridManager.Instance.SetOccupied(gridPos, false); // 칸 다시 비우기
        Destroy(gameObject);
    }

    // 물줄기 관련
    private void SpawnWaterJet(Vector2Int pos)
    {
        Vector2 worldPos = GridManager.Instance.GridToWorld(pos);
        Instantiate(waterJetPrefab, worldPos, Quaternion.identity);
    }

}
