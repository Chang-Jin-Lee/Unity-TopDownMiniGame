using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{    
    [FormerlySerializedAs("PlayerAbilityTemplate")] [SerializeField] public PlayerAbilityData[] playerAbilityTemplate = new PlayerAbilityData[(int)eCharacterState.Max];
    [SerializeField] private GameObject[] PlayerModel = new GameObject[(int)eCharacterState.Max];
    [SerializeField] private GameObject Player;
    [FormerlySerializedAs("Gamestate")] [SerializeField] private GameState gameState;
    void Awake()
    {
        gameState = GameState.Instance;

        if (PlayerModel.Length > 0 && Player != null)
        {
            GameObject obj = Instantiate(PlayerModel[(int)gameState.curCharacterState], Vector3.zero,
                Quaternion.identity);
            Player.GetComponent<Player>().curCharacterModel = obj;
            Player.GetComponent<Player>().animatorController = obj.GetComponent<Animator>();
            Player.GetComponent<Player>().playerAbilityTemplate = playerAbilityTemplate[(int)gameState.curCharacterState];
            obj.transform.SetParent(Player.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
    }
}
