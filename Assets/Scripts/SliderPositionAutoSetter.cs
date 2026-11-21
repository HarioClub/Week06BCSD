using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = Vector3.down * 0.4f;
    private Transform targetTransform;
    private RectTransform rectTransform;
    private Camera mainCamera;

    public void Setup(Transform target)
    {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        rectTransform.position = mainCamera.WorldToScreenPoint(targetTransform.position + offset);
    }
}
