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
        // 마우스 위치 가져오기 (월드 좌표)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // 플레이어 -> 마우스 방향 벡터
        Vector2 direction = (mousePos - transform.position).normalized;

        // 방향을 각도로 변환
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 플레이어 회전 적용
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
