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
    
    // Weapon���� ������ destination Position
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
    private float lastFireTime = 0f; // ������ �߻� �ð� ����

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
        if (anim_cur.Contains("Attack")) // �����ϴ� ���ȿ��� ���ݸ��ϱ�, ������ ���� ����
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
        yield return null;  // �ִϸ��̼� ���� �÷��� ���̸� �Ȱ������� 1������ ��ٸ��� 
        float delay = animatorController.GetCurrentAnimatorClipInfo(0)[0].clip.length;    // �ִϸ��̼� ���� �����ͼ� ��ٸ��� �ϱ�
        print(anim_cur);
        print(delay);
        yield return new WaitForSeconds(delay);
        SetAnimation(anim);
    }

    void RayCastMousePosition()
    {
        UnityEngine.RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = ~(1 << LayerMask.NameToLayer("Bullet")); // Bullet ���̾ ����
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 500.0f, layerMask))
        {
            //print(hit.transform.gameObject.name);
            mousePosition = hit.point;
            Vector3 xzPosition = new Vector3(mousePosition.x, 0, mousePosition.z);
            curCharacterModel.transform.LookAt(xzPosition);   //Ư�� ��ġ �ٶ󺸱�
        }
    }

    public void EquipWeapon(GameObject Gunobj)
    {
        GameObject obj = Instantiate(Gunobj);
        obj.transform.position = Vector3.zero; // ��ġ �ʱ�ȭ
        obj.transform.rotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
        obj.GetComponent<BoxCollider>().enabled = false;
        gunRef = obj.GetComponent<Weapon>();
        StartCoroutine(DelayedEquip(obj));
        //print("Weapon Spawn");
    }
    private IEnumerator DelayedEquip(GameObject obj)
    {
        Transform targetTransform = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "LoweArm.R");
        obj.transform.SetParent(targetTransform, false);

        // Ȯ���ϰ� Parent ���� �Ϸ�� ������ ��ٸ���
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
        yield return null;  // 1������ ��ٸ��� 
        float delay = 0.5f;    // 0.5�� �Ŀ� ������ ���� �� ����.
        yield return new WaitForSeconds(delay);
        targetTransform.GetComponent<BoxCollider>().enabled = true;
    }

    void combatStart()
    {
        combat_state_cur = eCombatState.unArmed;
    }
}
