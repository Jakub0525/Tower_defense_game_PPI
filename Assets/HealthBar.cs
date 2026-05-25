using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }

    public void UpdateBar(int currentHP, int maxHP)
    {
        fillImage.fillAmount = (float)currentHP / maxHP;
    }
}