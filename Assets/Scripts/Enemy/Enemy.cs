using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Enemy : MonoBehaviour
{
    public float detectionRange = 5f; // 플레이어 감지 거리
    public float moveSpeed = 2f; // 몬스터 이동 속도
    public Transform player;
    private bool isChasing = false;
    private SpriteRenderer spriteRenderer;
    private float fadeSpeed = 5f; // 없어지는 속도

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange) // 🎯 플레이어가 감지 범위 안에 들어옴
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
