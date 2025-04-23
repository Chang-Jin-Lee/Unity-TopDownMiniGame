using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IAbility
{
    [SerializeField] private AbilityData abilityTemplate;
    
    public Animator animator;
    public GameObject CharacterModel;
    public Weapon gunRef;
    
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
    
    // About Animation
    public string anim_cur = "Idle";
    
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
    public GameObject[] Attack_RifleFXs;
    //public SpriteRenderer sr;
    
    
    void Start()
    {
        combatStart();
        SetAbilities();
    }

    public void SetAbilities()
    {
        moveWalkSpeed = abilityTemplate.moveWalkSpeed;
        moveDashSpeed = abilityTemplate.moveDashSpeed;
        health = abilityTemplate.health;
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
        
        gunRef.Shoot(mousePosition);
        
        Debug.DrawLine(gunRef.transform.position, mousePosition, Color.red);
        
        GameObject AttackFX = Attack_RifleFXs[Random.Range(0, Attack_RifleFXs.Length)];
        Instantiate(AttackFX, transform.position + CharacterModel.transform.forward * 2, transform.rotation);
    }

    void SetAnimation(string anim, string next = "")
    {
        if (anim_cur == anim) return;
        anim_cur = anim;
        animator.Play(anim_cur);

        if (next != "") StartCoroutine(SetAnimationNext(next));
    }

    IEnumerator SetAnimationNext(string anim)
    {
        yield return null;  // 애니메이션 이전 플레이 길이를 안가져오게 1프레임 기다리기 
        float delay = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;    // 애니메이션 길이 가져와서 기다리게 하기
        print(anim_cur);
        print(delay);
        yield return new WaitForSeconds(delay);
        SetAnimation(anim);
    }

    void RayCastMousePosition()
    {
        UnityEngine.RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 1000, out hit))
        {
            //print(hit.transform.gameObject.name);
            mousePosition = hit.point;
            CharacterModel.transform.LookAt(hit.point);   //특정 위치 바라보기
        }
    }

    public void EquipWeapon(GameObject Gunobj)
    {
        GameObject obj = Instantiate(Gunobj);
        gunRef = obj.GetComponent<Weapon>();
        obj.GetComponent<BoxCollider>().enabled = false;
        Transform targetTransform = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "LoweArm.R");
        obj.transform.SetParent(targetTransform, false);
        obj.transform.localPosition = new Vector3(-0.188999996f,-0.0250000004f,0.00499999989f);
        obj.transform.localRotation = new Quaternion(0.61122942f,0.479516983f,-0.403191835f,0.483630598f);
        combat_state_cur = eCombatState.rifle;
        print("Weapon Spawn");
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
            targetTransform.position = transform.position + CharacterModel.transform.forward + transform.up;
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
