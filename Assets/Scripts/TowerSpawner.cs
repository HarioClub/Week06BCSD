using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private TowerAttackRange attackRange;

    private TowerTemplate selectedTower;
    private SpriteRenderer preview;

    public bool IsPlacing => selectedTower != null;

    public void SelectTower(TowerTemplate tower)
    {
        if (IsPlacing) return;
        if (tower.weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        selectedTower = tower;
        preview = new GameObject("Tower Preview").AddComponent<SpriteRenderer>();
        preview.transform.localScale = tower.towerPrefab.transform.localScale;
        preview.sprite = tower.weapon[0].sprite;
        preview.sortingOrder = 3;
        preview.gameObject.SetActive(false);
    }

    public void MovePreview(Vector3 position)
    {
        preview.transform.position = position;
        preview.gameObject.SetActive(true);
        attackRange.OnAttackRange(position, selectedTower.weapon[0].range);
    }

    public void CancelPlacement()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
            attackRange.OffAttackRange();
        }
        preview = null;
        selectedTower = null;
    }

    public void SpawnTower(Tile tile)
    {
        if (!IsPlacing) return;
        if (tile == null || tile.IsBuildTower)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        int cost = selectedTower.weapon[0].cost;
        if (playerGold.CurrentGold < cost)
        {
            systemTextViewer.PrintText(SystemType.Money);
            CancelPlacement();
            return;
        }

        Vector3 position = tile.transform.position + Vector3.back;
        GameObject clone = Instantiate(selectedTower.towerPrefab, position, Quaternion.identity);
        clone.GetComponent<TowerWeapon>().Setup(selectedTower, enemySpawner, playerGold, tile);
        tile.IsBuildTower = true;
        playerGold.CurrentGold -= cost;
        CancelPlacement();
    }
}
