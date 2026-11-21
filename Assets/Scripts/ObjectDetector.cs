using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private TowerSpawner towerSpawner;
    [SerializeField] private TowerDataViewer towerDataViewer;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        towerDataViewer.OffPanel();
    }

    private void Update()
    {
        if (WaveSystem.Instance.IsFinished)
        {
            if (towerSpawner.IsPlacing) towerSpawner.CancelPlacement();
            if (towerDataViewer.gameObject.activeSelf) towerDataViewer.OffPanel();
            return;
        }
        Mouse mouse = Mouse.current;
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true || mouse?.rightButton.wasPressedThisFrame == true)
        {
            towerSpawner.CancelPlacement();
            towerDataViewer.OffPanel();
            return;
        }

        if (mouse == null) return;
        bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
        if (towerSpawner.IsPlacing)
        {
            Plane towerPlane = new Plane(Vector3.forward, Vector3.back);
            if (towerPlane.Raycast(ray, out float distance))
                towerSpawner.MovePreview(ray.GetPoint(distance));
        }

        if (overUI || !mouse.leftButton.wasPressedThisFrame) return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (towerSpawner.IsPlacing)
            {
                towerSpawner.SpawnTower(hit.transform.GetComponent<Tile>());
                return;
            }

            if (hit.transform.CompareTag("Tower"))
            {
                towerDataViewer.OnPanel(hit.transform);
                return;
            }
        }

        towerDataViewer.OffPanel();
    }

    public void SelectTower(TowerTemplate tower)
    {
        if (WaveSystem.Instance.IsFinished) return;
        towerDataViewer.OffPanel();
        towerSpawner.SelectTower(tower);
    }
}
