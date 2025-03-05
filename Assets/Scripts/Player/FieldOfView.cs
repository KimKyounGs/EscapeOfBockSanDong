using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    [Header("시야 설정")]
    public float viewRadius = 5f;  // 시야 범위
    [Range(0, 360)]
    public float viewAngle = 90f;  // 시야 각도 (부채꼴)
    public int rayCount = 50;      // 부채꼴을 이루는 레이 개수
    public LayerMask obstacleMask; // 장애물(벽) 감지

    [Header("메시 렌더링")]
    public MeshFilter viewMeshFilter;

    private Mesh viewMesh;
    private HashSet<GameObject> seenObjects = new HashSet<GameObject>(); // 본 적 있는 물체 저장


    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        // ✅ MeshRenderer의 Material을 반투명하게 설정
        MeshRenderer meshRenderer = viewMeshFilter.GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = new Color(0, 0, 0, 0.8f); // 반투명 검은색 (시야 마스크)
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
        UpdateObjectsInView();
    }

    void UpdateObjectsInView()
    {
        Collider2D[] objectsInView = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        foreach (Collider2D obj in objectsInView)
        {
            Vector3 dirToObj = (obj.transform.position - transform.position).normalized;
            float angleToObj = Mathf.Atan2(dirToObj.y, dirToObj.x) * Mathf.Rad2Deg;

            // 부채꼴 시야 안에 있는지 확인
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, angleToObj)) < viewAngle / 2)
            {
                // 벽이 가리고 있는지 Raycast로 확인
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToObj, viewRadius, obstacleMask);
                if (hit.collider == null || hit.collider.gameObject == obj.gameObject)
                {
                    // 시야 안에 있는 물체: 원래 상태로 표시
                    SetObjectAlpha(obj.gameObject, 1f);
                    seenObjects.Add(obj.gameObject);
                }
                else
                {
                    // 벽이 가려서 보이지 않으면 흐리게
                    if (seenObjects.Contains(obj.gameObject))
                    {
                        SetObjectAlpha(obj.gameObject, 0.3f);
                    }
                    else
                    {
                        obj.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // 시야 밖이지만 이전에 본 적 있다면 흐리게
                if (seenObjects.Contains(obj.gameObject))
                {
                    SetObjectAlpha(obj.gameObject, 0.3f);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }

    void SetObjectAlpha(GameObject obj, float alpha)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = rayCount;
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount]; // vertices[]: 메시의 모든 점을 저장하는 배열
        int[] triangles = new int[(vertexCount - 2) * 3]; // triangles[]: 삼각형(정점)을 이루는 점들의 인덱스를 저장하는 배열

        vertices[0] = Vector3.zero; // 메시의 첫 번째 꼭짓점은 중심점 (플레이어 위치)
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]); // transform.InverseTransformPoint(viewPoints[i]): → 월드 좌표를 로컬 좌표로 변환해서 저장

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
                /*
                triangles[0] = 0, 1, 2;     
                triangles[3] = 0, 2, 3;
                triangles[6] = 0, 3, 4; (이런 식으로 시야 범위를 채워가는 방식)

                */
            }
        }

        viewMesh.Clear(); // 기존 메시를 지우고 새로 생성
        viewMesh.vertices = vertices; 
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals(); // 메시의 법선(Normal) 재계산 (광원/렌더링 최적화)
    }

ViewCastInfo ViewCast(float globalAngle)
{
    Vector3 dir = DirFromAngle(globalAngle, true);
    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, viewRadius, obstacleMask);

    if (hits.Length > 0)
    {
        // 가장 가까운 충돌 지점 찾기
        RaycastHit2D closestHit = hits[0];
        foreach (var hit in hits)
        {
            if (hit.distance < closestHit.distance)
            {
                closestHit = hit;
            }
        }

        // ✅ 디버깅용 초록색 레이 추가
        Debug.DrawRay(transform.position, dir * closestHit.distance, Color.green, 0.1f);
        Debug.Log($"Ray Hit: {closestHit.collider.name}, Distance: {closestHit.distance}");

        return new ViewCastInfo(true, closestHit.point, closestHit.distance, globalAngle);
    }
    else
    {
        // 장애물이 없으면 최대 거리까지 시야 표시
        Debug.DrawRay(transform.position, dir * viewRadius, Color.green, 0.1f);

        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }
}

    // 각도를 방향 벡터(Vector3)로 변환하는 역할
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z; // 플레이어 회전 적용
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }
}
