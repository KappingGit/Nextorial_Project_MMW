using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance; // 다른 스크립트에서 쉽게 접근하기 위한 싱글톤

    public int gridWidth = 8; // 너비
    public int gridHeight = 8; // 높이
    public float cellSize = 1.0f;      // 한 칸의 실제 월드 크기(예시: 좌표 0과 1의 사이 간격을 뜻함)
    public Vector2 originPos = Vector2.zero; // 그리드 시작(왼쪽 아래) 월드 좌표, 기준점

    private bool[,] occupied; // 칸이 차있는지 여부

    private void Awake()
    {
        Instance = this;
        occupied = new bool[gridWidth, gridHeight];
    }

    /// <summary>
    /// 그리드 좌표 -> 월드 좌표 변환
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public Vector2 GridToWorld(Vector2Int gridPos) //gridPos는 월드 좌표로 변환, 그리드는 바둑과 같은 느낌, 이것을 전환 한다고 생각하면 됨
    {
        float x = originPos.x + gridPos.x * cellSize + cellSize * 0.5f; //gridPos.x * cellSize는 그 칸이 시작하는 지점, cellSize * 0.5f;는 칸의 중앙을 뜻한다
        float y = originPos.y + gridPos.y * cellSize + cellSize * 0.5f;
        return new Vector2(x, y);
    }

    /// <summary>
    /// 비어있는 칸 하나를 랜덤으로 반환 (없으면 null 처리 필요)
    /// </summary>
    /// <returns></returns>
    public Vector2Int? GetRandomEmptyCell()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>(); // 비어있는 칸을 리스트에 넣음

        for (int x = 0; x < gridWidth; x++) // 비어있는 칸을 차례로 확인
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (!occupied[x, y]) // 비어있는 칸(좌표)이 있다면
                    emptyCells.Add(new Vector2Int(x, y)); // 채운다
            }
        }

        if (emptyCells.Count == 0) return null; // 빈 칸이 하나도 없음

        int randomIndex = Random.Range(0, emptyCells.Count);
        return emptyCells[randomIndex];
    }

    /// <summary>
    /// 좌표를 사용중을 알리는 함수
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="value"></param>
    public void SetOccupied(Vector2Int gridPos, bool value)
    {
        occupied[gridPos.x, gridPos.y] = value;
    }

    public bool IsInsideGrid(Vector2Int pos) //물줄기 스크립트에서 사용됨
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    /// <summary>
    /// 그리드 눈으로 확인용
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(originPos.x + x * cellSize, originPos.y, 0);
            Vector3 end = new Vector3(originPos.x + x * cellSize, originPos.y + gridHeight * cellSize, 0);
            Gizmos.DrawLine(start, end); // 세로선
        }

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = new Vector3(originPos.x, originPos.y + y * cellSize, 0);
            Vector3 end = new Vector3(originPos.x + gridWidth * cellSize, originPos.y + y * cellSize, 0);
            Gizmos.DrawLine(start, end); // 가로선
        }
    }

}
