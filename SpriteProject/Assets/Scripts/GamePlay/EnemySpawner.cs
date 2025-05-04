using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> enemyPrefabs;
    public float range = 40.0f;
    public int repeatCount = 2;
    public int maxCount = 100;
    public Transform playerTransform;
    [SerializeField] private GameState gameState;
    private void Awake()
    {
        gameState = GameState.Instance;
    }
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, 1f);
    }
    private void SpawnEnemy()
    {
        if(enemies.Count < maxCount)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0,enemyPrefabs.Count)];
            for (int i = 0; i < repeatCount; i++)
            {
                if (enemies.Count >= maxCount) return;
                float len = Random.Range(1, range);
                Vector2 dir = Random.insideUnitCircle.normalized * len;
                Vector3 randomDir = new Vector3(dir.x, 1, dir.y);
                GameObject obj = Instantiate(enemyPrefab, randomDir, Quaternion.identity);
                obj.GetComponent<Enemy>().playerTransform = playerTransform;
                enemies.Add(obj);
                gameState.EnemyCount += i;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, range);
    }
}
