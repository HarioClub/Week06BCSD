using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private Transform laserHit;
    [SerializeField] private CircleCollider2D slowArea;

    private TowerTemplate towerTemplate;
    private EnemySpawner enemySpawner;
    private PlayerGold playerGold;
    private Tile ownerTile;
    private SpriteRenderer spriteRenderer;
    private EnemyHP target;
    private Collider2D targetShape;
    private int level;
    private float shotTimer;

    private TowerTemplate.Weapon Weapon => towerTemplate.weapon[level];
    public TowerType Type => towerTemplate.towerType;
    public Sprite TowerSprite => Weapon.sprite;
    public float Damage => Weapon.damage;
    public float Slow => Weapon.slow;
    public float Rate => Weapon.rate;
    public float Range => Weapon.range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;

    public void Setup(TowerTemplate tower, EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        towerTemplate = tower;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyLevel();
    }

    private void Update()
    {
        if (Type == TowerType.Slow) return;
        if (target == null || target.CurrentHP <= 0 || !InRange(target.transform.position))
        {
            target = FindClosestEnemy();
            targetShape = target != null ? target.GetComponent<Collider2D>() : null;
            shotTimer = Rate;
        }

        if (Type == TowerType.Laser) laser.gameObject.SetActive(target != null);
        if (target == null) return;

        Vector2 direction = target.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        if (Type == TowerType.Laser)
        {
            Vector2 hit = targetShape.ClosestPoint(spawnPoint.position);
            laser.SetPosition(0, spawnPoint.position);
            laser.SetPosition(1, new Vector3(hit.x, hit.y, transform.position.z));
            laserHit.position = hit;
            target.TakeDamage(Damage * Time.deltaTime);
        }
        else
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer > 0) return;
            GameObject shot = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            shot.GetComponent<Projectile>().Setup(target.transform, Damage);
            shotTimer = Rate;
        }
    }

    public bool InRange(Vector2 position)
    {
        return Vector2.Distance(position, transform.position) <= Range;
    }

    private EnemyHP FindClosestEnemy()
    {
        Enemy closest = null;
        float nearest = Range;
        foreach (Enemy enemy in enemySpawner.EnemyList)
        {
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance > nearest) continue;
            nearest = distance;
            closest = enemy;
        }
        return closest != null ? closest.GetComponent<EnemyHP>() : null;
    }

    public bool Upgrade()
    {
        if (Level >= MaxLevel || playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
            return false;

        level++;
        playerGold.CurrentGold -= Weapon.cost;
        ApplyLevel();
        return true;
    }

    private void ApplyLevel()
    {
        spriteRenderer.sprite = TowerSprite;
        if (Type == TowerType.Laser)
        {
            laser.startWidth = 0.05f * Level;
            laser.endWidth = 0.05f;
        }
        if (Type == TowerType.Slow)
            slowArea.radius = Range / slowArea.transform.lossyScale.x;
    }

    public void Sell()
    {
        playerGold.CurrentGold += Weapon.sell;
        ownerTile.IsBuildTower = false;
        Destroy(gameObject);
    }
}
