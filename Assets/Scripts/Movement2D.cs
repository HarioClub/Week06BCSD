using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public float TravelTimeMultiplier { private get; set; } = 1f;
    public float MoveSpeed => moveSpeed / TravelTimeMultiplier;

    private void Update()
    {
        transform.position += moveDirection * MoveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
