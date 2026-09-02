using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove01 : MonoBehaviour
{
    // 기본적인 캐릭터 움직임, 다른 미니게임에서도 활용 될 수 있음

    private Rigidbody2D charRG;

    private float moveSpeed = 5.0f;

    private Vector2 moveDir;

    private bool canMove = true; // 캐릭터 조작 가능여부

    private void Awake()
    {
        charRG = GetComponent<Rigidbody2D>();
    }

    // 유니티에서 자동으로 호출해주는 함수들
    private void OnEnable() // 이 오브젝트가 활성화될 때 한 번 실행 (보통 게임 시작 시)
    {
        GameManager.Instance.OnGameOver += StopMoving; //"게임 오버 이벤트가 발생하면~" StopMoving()함수를 실행시켜줘
        GameManager.Instance.OnGameClear += StopMoving;
    }

    private void OnDisable() // 이 오브젝트가 비활성화되거나 파괴될 때 한 번 실행
    {
        //중요 : 존재하지 않는 오브젝트의 함수를 호출하려다 에러가 날 수 있어요. +=로 등록했으면 -=로 꼭 짝을 맞춰 해제하는 게 안전한 습관
        GameManager.Instance.OnGameOver -= StopMoving; 
        GameManager.Instance.OnGameClear -= StopMoving;
    }

    private void Update()
    {
        if (!canMove) return; // 정지 상태면 입력 자체를 안 받음
        MoveController();
    }

    // 이곳에서 실제로 움직이는 코드를 넣어야지 떨리는 방지를 함
    private void FixedUpdate()
    {
        CharMove();
        //ClampPositionInsideGrid();
    }

    private void MoveController() // 조작 함수
    {
        float inputH = Input.GetAxisRaw("Horizontal"); // 왼쪽/오른쪽: -1, 0, 1
        float inputV = Input.GetAxisRaw("Vertical");   // 아래/위: -1, 0, 1

        if (inputV != 0)
            moveDir = new Vector2(0, inputV);   // 상하 입력이 있으면 상하만
        else if (inputH != 0)
            moveDir = new Vector2(inputH, 0);   // 없으면 좌우
        else
            moveDir = Vector2.zero;  // 멈춤상태

    }

    private void CharMove() // 움직임 함수 (속도가 여기에 사용됨)
    {
        charRG.velocity = moveDir.normalized * moveSpeed;
    }

    private void StopMoving() // 정지 코드
    {
        canMove = false;
        moveDir = Vector2.zero; // 남아있던 이동값도 즉시 제거
    }

    //private void ClampPositionInsideGrid()
    //{
    //    // (물리적 벽으로 대체함)
    //}

}
