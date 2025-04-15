using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float moveSpeed = 5f; // 카메라가 이동하는 속도
    public Vector2 gridSize = new Vector2(10f, 10f); // 한 칸 크기 (맵 타일 크기)
    
    private Vector3 targetPosition; // 목표 위치
    private bool isMoving = false; // 이동 중인지 체크

    void Start()
    {
        if (player != null)
        {
            targetPosition = GetSnappedPosition(player.position);
            transform.position = targetPosition;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.position;
        Vector3 snappedPos = GetSnappedPosition(playerPos);

        if (snappedPos != targetPosition && !isMoving)
        {
            targetPosition = snappedPos;
            StartCoroutine(MoveCamera());
        }
    }

    Vector3 GetSnappedPosition(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize.x) * gridSize.x;
        float y = Mathf.Round(position.y / gridSize.y) * gridSize.y;
        return new Vector3(x, y, transform.position.z);
    }

    IEnumerator MoveCamera()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // 정확한 위치 보정
        isMoving = false;
    }
}
