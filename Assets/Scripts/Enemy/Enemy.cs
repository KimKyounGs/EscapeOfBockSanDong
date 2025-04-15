using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Enemy : MonoBehaviour
{   
    [Header("몬스터 설정")]
    [SerializeField] private float moveSpeed = 2f; // 몬스터 이동 속도
    [SerializeField] private float detectionRange = 5f; // 플레이어 감지 거리
    [SerializeField] private bool isChasing = false; // 현재 쫒고 있는가
    [SerializeField] private float fadeSpeed = 5f; // 없어지는 속도

    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange) // 플레이어가 감지 범위 안에 들어옴
        {
            if (!isChasing) 
            {
                isChasing = true;
            }
            ChasePlayer();
        }
        else 
        {
            if (isChasing) 
            {
                isChasing = false;
            }
        }
    }

    void ChasePlayer()
    {
        SoundManager.instance.SoundScream();
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
