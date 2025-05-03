using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour, IWeaponAbility
{
    private Rigidbody rb;
    [SerializeField] private Bullet[] bulletPrefabs;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private WeaponAbiliyData weaponAbiliyData;
    [FormerlySerializedAs("Attack_RifleFXs")] public GameObject[] Attack_RifleFXs;
    
    public float fireRate = 1.0f;

    public Vector3 GetSpawnPoint => bulletSpawnPoint.transform.position;

    void Start()
    {
        fireRate = weaponAbiliyData.fireRate;
        foreach (var bulletPrefab in bulletPrefabs)
        {
            bulletPrefab.SetBulletAbility(weaponAbiliyData);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DelayedEquip(player));
        }
    }

    private IEnumerator DelayedEquip(Player player)
    {
        yield return null; // 1프레임 기다리기
        player.EquipWeapon(gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position - transform.up * 300, Color.green);
    }

    public void Shoot(Vector3 desPos)
    {
        Debug.DrawLine(bulletSpawnPoint.transform.position, desPos, Color.green);

        Vector3 dir = (desPos - bulletSpawnPoint.transform.position).normalized;
        Quaternion correction = Quaternion.FromToRotation(-transform.up, dir);
        transform.rotation = correction * transform.rotation;

        var bulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
        Bullet spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        spawnedBullet.SetBulletAbility(weaponAbiliyData);
        spawnedBullet.SetBulletDirection(bulletSpawnPoint.transform.position, desPos);
        
        GameObject AttackFX = Attack_RifleFXs[Random.Range(0, Attack_RifleFXs.Length)];
        Vector3 gunspawnPosition = GetSpawnPoint;
        AttackFX.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f); // change its local scale in x y z format
        Instantiate(AttackFX, gunspawnPosition, transform.rotation);
    }
}
