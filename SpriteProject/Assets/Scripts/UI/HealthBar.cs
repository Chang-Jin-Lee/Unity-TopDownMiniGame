using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    //[SerializeField] private Transform cameraTransform;

    void LateUpdate()
    {
        // 카메라를 바라보도록 회전
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void SetHP(float current, float max)
    {
        hpSlider.value = current / max;
    }
}
