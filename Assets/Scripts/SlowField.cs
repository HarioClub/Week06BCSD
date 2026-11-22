using UnityEngine;

public class SlowField : MonoBehaviour
{
    private TowerWeapon tower;

    private void Awake()
    {
        tower = GetComponentInParent<TowerWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy)) enemy.EnterSlowField(tower);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy)) enemy.ExitSlowField(tower);
    }
}
