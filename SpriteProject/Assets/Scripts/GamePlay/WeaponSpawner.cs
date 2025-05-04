using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSpawner : MonoBehaviour
{
    // 캐릭터에 따라서 무기가 다른게 나오게 하기
    [SerializeField] private GameObject[] weaponModel = new GameObject[(int)eCharacterState.Max];
    [SerializeField] private GameObject player;
    [FormerlySerializedAs("gamestate")] [SerializeField] private GameState gameState;
    void Awake()
    {
        gameState = GameState.Instance;
    }
    private void Start()
    {
        Instantiate(weaponModel[(int)gameState.curCharacterState], player.transform.position + new Vector3(-3,1,-3),
            Quaternion.identity);
    }
}
