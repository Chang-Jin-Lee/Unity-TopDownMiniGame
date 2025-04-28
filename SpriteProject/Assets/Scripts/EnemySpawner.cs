using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject enemyPrefab;
    public float range = 40.0f;
    public int repeatCount = 2;
    public int maxCount = 100;
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, 1f);

        //OnDrawGizmos();
    }

    private void SpawnEnemy()
    {
        if(enemies.Count < maxCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                if (enemies.Count >= maxCount) return;
                float len = Random.Range(1, range);
                Vector2 dir = Random.insideUnitCircle.normalized * len;
                Vector3 randomDir = new Vector3(dir.x, 1, dir.y);
                GameObject obj = Instantiate(enemyPrefab, randomDir, Quaternion.identity);
                enemies.Add(obj);
            }
        }
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, range);
    }
}
