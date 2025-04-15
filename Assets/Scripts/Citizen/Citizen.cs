using System;
using UnityEngine;
using UnityEngine.UI;

public class Citizen : MonoBehaviour
{
    [Header("시민 정보")]
    [SerializeField] private bool isRescued;
    
    [Header("UI 요소")]
    private Slider rescueSlider;
    
    private void Awake()
    {
        // 자식 오브젝트에서 Slider 자동 참조
        rescueSlider = GetComponentInChildren<Slider>();
        if (rescueSlider != null)
        {
            rescueSlider.gameObject.SetActive(false); // 처음엔 안 보이게
            rescueSlider.maxValue = 1f;
        }
    }
    public void SetRescueProgress(float value)
    {
        if (rescueSlider == null) return;
        
        rescueSlider.gameObject.SetActive(true); // 플레이어가 가까이 오면 보이게
        rescueSlider.value = value;

        if (value <= 0f || value >= 1f)
        {
            rescueSlider.gameObject.SetActive(false); // 슬라이더가 0% 또는 100%일 때 숨기기
        }
    }

    public void Rescue()
    {
        if (isRescued) return ;

        isRescued = true;
        Debug.Log("시민 구출");
    }
}
