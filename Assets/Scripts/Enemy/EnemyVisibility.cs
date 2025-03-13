using UnityEngine;
using System.Collections;

public class EnemyVisibility : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeSpeed = 5f;
    private bool isVisible = false; // 현재 몬스터가 보이는 상태인지 체크

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetAlpha(0f); // 기본적으로 몬스터는 안 보이게 설정
    }
    void Update()
    {
        if (!isVisible) // 🔥 감지되지 않았다면 자동으로 숨김
        {
            SetAlpha(0f);
        }
        isVisible = false; // ✅ 다음 프레임에서 다시 감지될 때까지 초기화
    }

    public void SetVisible(bool state)
    {
        isVisible = state;
        if (isVisible)
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo(1f)); // 부드럽게 보이게 설정
        }
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        while (!Mathf.Approximately(spriteRenderer.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(spriteRenderer.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            SetAlpha(alpha);
            yield return null;
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
