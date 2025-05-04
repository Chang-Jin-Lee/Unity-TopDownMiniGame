using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IPlayerAbility
{
    [FormerlySerializedAs("enemyAbilityTemplate")] [SerializeField] public PlayerAbilityData enemyAbilityTemplate;
    public Transform playerTransform;
    private NavMeshAgent agent;
    public GameState gameState;
    public HealthBar healthBar;
    
    // About Ability
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float moveSpeed = 5.0f;
    public float moveWalkSpeed = 5.0f;
    public float moveDashSpeed = 10.0f;
    
    public float Health => health;
    public float MoveWalkSpeed => moveWalkSpeed;
    public float MoveDashSpeed => moveDashSpeed;

    private void Awake()
    {
        gameState = GameState.Instance;
    }

    public void Death()
    {
        gameState.KillCount++;
        gameState.EnemyCount--;
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHP(health, maxHealth);
        if (health <= 0)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
    }
    
    public void SetAbilities()
    {
        moveWalkSpeed = enemyAbilityTemplate.moveWalkSpeed;
        moveDashSpeed = enemyAbilityTemplate.moveDashSpeed;
        health = enemyAbilityTemplate.health;
        maxHealth = enemyAbilityTemplate.health;
        agent.speed = moveSpeed;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetAbilities();
        healthBar = transform.GetComponentInChildren<HealthBar>();
    }

    private void FixedUpdate()
    {
        agent.SetDestination(playerTransform.position);
    }
}
