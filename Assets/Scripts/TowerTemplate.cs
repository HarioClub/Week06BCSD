using UnityEngine;

public enum TowerType { Cannon, Laser, Slow, Buff }

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public TowerType towerType;
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;
        public float damage;
        public float slow;
        public float buff;
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
