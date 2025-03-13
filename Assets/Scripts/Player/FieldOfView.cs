using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    [Header("시야 설정")]
    public float viewRadius = 5f;  // 시야 범위
    [Range(0, 360)]
    public float viewAngle = 90f;  // 시야 각도 (부채꼴)
    public int rayCount = 50;      // 부채꼴을 이루는 레이 개수

    public LayerMask wall; // 벽 감지 레이어
    public LayerMask enemyMask;    // 감지할 적(몬스터) 레이어
    private void LateUpdate()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        int stepCount = rayCount;
        float stepAngleSize = viewAngle / stepCount;

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            Vector3 dir = DirFromAngle(angle+90, true);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, enemyMask | wall);

            if (hit.collider != null)
            {
                // 디버깅용 초록색 레이 (충돌된 지점까지)
                Debug.DrawRay(transform.position, dir * hit.distance, Color.green, 0.1f);

                // 벽을 먼저 맞았는지 확인
                if (((1 << hit.collider.gameObject.layer) & wall) != 0)
                {
                    // 벽을 만나면 더 이상 Raycast 진행 안 함
                    continue;
                }

                if (((1 << hit.collider.gameObject.layer) & enemyMask) != 0)
                {
                    // 적 감지 → 보이게 설정
                    EnemyVisibility monster = hit.collider.GetComponent<EnemyVisibility>();
                    if (monster != null)
                    {
                        monster.SetVisible(true);
                    }
                }
            }
            else
            {
                // 디버깅용 초록색 레이 (최대 거리까지)
                Debug.DrawRay(transform.position, dir * viewRadius, Color.green, 0.1f);
            }

 
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
