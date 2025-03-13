using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f; // 이동 속도
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 가져오기
    }

    void Update()
    {
        // WASD 입력 감지
        movement.x = Input.GetAxisRaw("Horizontal"); // A, D 또는 ←, →
        movement.y = Input.GetAxisRaw("Vertical");   // W, S 또는 ↑, ↓

        // 플레이어 회전 (마우스 방향)
        RotateTowardsMouse();
    }

    void FixedUpdate()
    {
        // Rigidbody2D를 이용해 부드럽게 이동
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    void RotateTowardsMouse()
    {
        // 1. 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D 게임이므로 Z 고정

        // 2. 방향 벡터 계산 (플레이어 → 마우스)
        Vector2 direction = (mousePos - transform.position).normalized;

        // 3. 방향을 각도로 변환
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. Rigidbody2D를 사용해 회전 적용 (더 부드러운 회전)
        rb.MoveRotation(angle-90);
        /*
        90도를 배는 이유는 좌표평면 위에서 X축을 기준으로 시작하기 때문이다.
        x축을 기준으로 몇 도 차이가 나는지 반환하기 때문에 마우스를 완전히 화살표의 오른쪽에 놓는다면, angle은 0을 가르킨다.
        하지만 우리 인간의 기준으로는 마우스가 화살표의 오른쪽에 있다면 화살표가 오른쪽으로 90도 회전해야 맞다.
        */

    }
}
