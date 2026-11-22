using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;
    private readonly List<TowerWeapon> slowTowers = new List<TowerWeapon>();
    [SerializeField]
    private int gold = 10;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        transform.position = wayPoints[currentIndex].position;

        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            UpdateTravelTime();
            transform.Rotate(Vector3.forward, 360f * Time.deltaTime);

            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < Mathf.Max(0.02f, Time.deltaTime) * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    public void EnterSlowField(TowerWeapon tower)
    {
        if (!slowTowers.Contains(tower)) slowTowers.Add(tower);
    }

    public void ExitSlowField(TowerWeapon tower)
    {
        slowTowers.Remove(tower);
    }

    private void UpdateTravelTime()
    {
        float multiplier = 1f;
        for (int i = slowTowers.Count - 1; i >= 0; i--)
        {
            TowerWeapon tower = slowTowers[i];
            if (tower == null) slowTowers.RemoveAt(i);
            else multiplier *= 1f + tower.Slow;
        }
        movement2D.TravelTimeMultiplier = multiplier;
    }

    private void NextMoveTo()
    {
        if (currentIndex < wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            gold = 0;
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}

