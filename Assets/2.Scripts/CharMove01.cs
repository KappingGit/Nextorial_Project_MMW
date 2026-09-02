using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove01 : MonoBehaviour
{
    // 기본적인 캐릭터 움직임, 다른 미니게임에서도 활용 될 수 있음

    private Rigidbody2D charRG;

    private float moveSpeed = 5.0f;

    private Vector2 moveDir;

    private void Awake()
    {
        charRG = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveController();
    }

    // 이곳에서 실제로 움직이는 코드를 넣어야지 떨리는 방지를 함
    private void FixedUpdate()
    {
        CharMove();
    }

    private void MoveController()
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

    private void CharMove()
    {
        charRG.velocity = moveDir.normalized * moveSpeed;
    }

}
