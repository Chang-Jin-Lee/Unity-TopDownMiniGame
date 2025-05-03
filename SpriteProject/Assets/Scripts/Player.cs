using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IPlayerAbility
{
    //[SerializeField] private string abilityTemplatePath = "Player_Midori_Ability_Data";
    [FormerlySerializedAs("PlayerAbilityTemplate")] [SerializeField] public PlayerAbilityData playerAbilityTemplate;

    [FormerlySerializedAs("Weapon")] public Weapon gunRef;
    [FormerlySerializedAs("CharacterModel")] public GameObject curCharacterModel;
    [FormerlySerializedAs("Animation")] public Animator animatorController;
    // About Animation
    public string anim_cur = "Idle";
    
    // Weapon에게 전달할 destination Position
    private Vector3 mousePosition = Vector3.zero;

    // About Ability
    public float health = 100.0f;
    public float moveSpeed = 5.0f;
    public float moveWalkSpeed = 5.0f;
    public float moveDashSpeed = 10.0f;

    public float Health => health;
    public float MoveWalkSpeed => moveWalkSpeed;
    public float MoveDashSpeed => moveDashSpeed;

    // About Weapon
    private float lastFireTime = 0f; // 마지막 발사 시각 저장

    public void Death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }
    public void Heal(float amount)
    {
        health += amount;
    }

    enum eAnimationState
    {
        idle_hand,
        idle_rifle,
        walk,
        dash,
        attack_hand,
        attack_rifle,
        hit,
        die,
    };
    eAnimationState state_cur;

    /* About Combat 
     */
    enum eCombatState
    {
        unArmed,
        rifle,
    }
    eCombatState combat_state_cur;
    void Start()
    {
        combatStart();
        SetAbilities();
    }

    public void SetAbilities()
    {
        //abilityTemplate = Resources.Load<AbilityData>(abilityTemplatePath);
        moveWalkSpeed = playerAbilityTemplate.moveWalkSpeed;
        moveDashSpeed = playerAbilityTemplate.moveDashSpeed;
        health = playerAbilityTemplate.health;
    }

    void Update()
    {
        Input_Update();
        AdjustState();
    }

    private void FixedUpdate()
    {
        RayCastMousePosition();
    }

    void Input_Update()
    {
        if (anim_cur.Contains("Attack")) // 공격하는 동안에는 공격만하기, 움직일 수는 있음
        {
            WASDmovement();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            DetachWeapon();
        }

        if (Input.GetKey(KeyCode.W) ||
         Input.GetKey(KeyCode.A) ||
         Input.GetKey(KeyCode.S) ||
         Input.GetKey(KeyCode.D)
         )
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                state_cur = eAnimationState.dash;
                moveSpeed = MoveDashSpeed;
            }
            else
            {
                state_cur = eAnimationState.walk;
                moveSpeed = MoveWalkSpeed;
            }

            WASDmovement();

            if (Input.GetMouseButton(0))
            {
                state_cur = GetCombatState();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            state_cur = GetCombatState();
        }
        else
        {
            state_cur = GetIdleState();
        }
    }

    void WASDmovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
    }

    private eAnimationState GetCombatState()
    {
        switch (combat_state_cur)
        {
            case eCombatState.unArmed:
                return eAnimationState.attack_hand;
            case eCombatState.rifle:
                return eAnimationState.attack_rifle;
        }

        return eAnimationState.attack_hand;
    }

    private eAnimationState GetIdleState()
    {
        switch (combat_state_cur)
        {
            case eCombatState.unArmed:
                return eAnimationState.idle_hand;
            case eCombatState.rifle:
                return eAnimationState.idle_rifle;
        }

        return eAnimationState.idle_hand;
    }

    public void AdjustState()
    {
        switch (state_cur)
        {
            case eAnimationState.idle_hand:
                SetAnimation("Idle_Hand");
                break;
            case eAnimationState.idle_rifle:
                SetAnimation("Idle_Rifle");
                break;
            case eAnimationState.walk:
                SetAnimation("Walk");
                break;
            case eAnimationState.dash:
                SetAnimation("Dash");
                break;
            case eAnimationState.attack_hand:
                Attack_Hand();
                break;
            case eAnimationState.attack_rifle:
                Attack_Rifle();
                break;
            case eAnimationState.hit:
                //Attack_Rifle();
                break;
        }
    }

    void Attack_Hand()
    {
        SetAnimation("Attack_Hand", "Idle_Hand");
    }

    void Attack_Rifle()
    {
        SetAnimation("Attack_Rifle", "Idle_Rifle");

        if (Time.time - lastFireTime > 1f / gunRef.fireRate)
        {
            gunRef.Shoot(mousePosition);
            lastFireTime = Time.time;
        }

        //Debug.DrawLine(gunRef.transform.position, mousePosition, Color.red);
    }

    void SetAnimation(string anim, string next = "")
    {
        if (anim_cur == anim) return;
        anim_cur = anim;
        animatorController.Play(anim_cur);

        if (next != "") StartCoroutine(SetAnimationNext(next));
    }

    IEnumerator SetAnimationNext(string anim)
    {
        yield return null;  // 애니메이션 이전 플레이 길이를 안가져오게 1프레임 기다리기 
        float delay = animatorController.GetCurrentAnimatorClipInfo(0)[0].clip.length;    // 애니메이션 길이 가져와서 기다리게 하기
        print(anim_cur);
        print(delay);
        yield return new WaitForSeconds(delay);
        SetAnimation(anim);
    }

    void RayCastMousePosition()
    {
        UnityEngine.RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = ~(1 << LayerMask.NameToLayer("Bullet")); // Bullet 레이어만 제외
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 500.0f, layerMask))
        {
            //print(hit.transform.gameObject.name);
            mousePosition = hit.point;
            Vector3 xzPosition = new Vector3(mousePosition.x, 0, mousePosition.z);
            curCharacterModel.transform.LookAt(xzPosition);   //특정 위치 바라보기
        }
    }

    public void EquipWeapon(GameObject Gunobj)
    {
        GameObject obj = Instantiate(Gunobj);
        obj.transform.position = Vector3.zero; // 위치 초기화
        obj.transform.rotation = Quaternion.identity; // 회전 초기화
        obj.GetComponent<BoxCollider>().enabled = false;
        gunRef = obj.GetComponent<Weapon>();
        StartCoroutine(DelayedEquip(obj));
        //print("Weapon Spawn");
    }
    private IEnumerator DelayedEquip(GameObject obj)
    {
        Transform targetTransform = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "LoweArm.R");
        obj.transform.SetParent(targetTransform, false);

        // 확실하게 Parent 설정 완료될 때까지 기다린다
        while (obj.transform.parent != targetTransform)
        {
            yield return null;
        }

        obj.transform.localPosition = new Vector3(-0.188999996f, -0.0250000004f, 0.00499999989f);
        obj.transform.localRotation = new Quaternion(0.61122942f, 0.479516983f, -0.403191835f, 0.483630598f);
        combat_state_cur = eCombatState.rifle;
    }

    void DetachWeapon()
    {
        combat_state_cur = eCombatState.unArmed;
        Transform targetTransform = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.Contains("Weapon"));
        gunRef = null;
        if (targetTransform)
        {
            targetTransform.GetComponent<BoxCollider>().enabled = true;
            targetTransform.SetParent(null, false);
            targetTransform.position = transform.position + curCharacterModel.transform.forward + transform.up;
            targetTransform.rotation = Quaternion.identity;
            //targetTransform.localPosition = transform.position + CharacterModel.transform.forward + transform.up * 2;
            //targetTransform.localRotation = new Quaternion(-0.707106829f, 0f, 0f, 0.707106829f);
            //SetCollisionEnableNext(targetTransform);
        }
    }

    IEnumerator SetCollisionEnableNext(Transform targetTransform)
    {
        yield return null;  // 1프레임 기다리기 
        float delay = 0.5f;    // 0.5초 후에 아이템 먹을 수 있음.
        yield return new WaitForSeconds(delay);
        targetTransform.GetComponent<BoxCollider>().enabled = true;
    }

    void combatStart()
    {
        combat_state_cur = eCombatState.unArmed;
    }
}
