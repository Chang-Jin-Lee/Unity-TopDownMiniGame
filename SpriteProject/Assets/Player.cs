using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public GameObject CharacterModel;
    //public SpriteRenderer sr;

    private bool bAttacking = false;
    public string anim_cur = "Idle";
    public float moveSpeed = 5.0f;
    enum eAnimationState
    {
        idle,
        walk,
        attack,
        hit,
        die,
    };

    eAnimationState state_cur;

    void Start()
    {

        //animator = GetComponent<Animator>();
        //animator.speed = 2.0f;  //스피드변경

        //Sprite 얻어오기
        //sr = GetComponent<SpriteRenderer>();

        //에니메이션 state
        //AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        //state.length
        //state.normalizedTime  //경과시간 0 ~ 1.0f

        //에니메이션 클립 정보 얻어오기
        //AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0); //anim.layerCount
        //for (int i = 0; i < clips.Length; i++)
        //{
        //    print("clip:" + clips[i].clip.name + " length:" + clips[i].clip.length);
        //}

    }

    void Update()
    {
        Input_Update();
        AdjustState();
    }

    void Input_Update()
    {
        //if (state_cur == eAnimationState.attack) return;    // 공격하는 동안에는 공격만하기 
        if (anim_cur == "Attack") return;

        if (Input.GetKey(KeyCode.UpArrow) ||
             Input.GetKey(KeyCode.DownArrow) ||
             Input.GetKey(KeyCode.RightArrow) ||
             Input.GetKey(KeyCode.LeftArrow)
             )
        {
            state_cur = eAnimationState.walk;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += transform.up * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= transform.up * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                CharacterModel.transform.rotation = Quaternion.Euler(0,100,0);
                transform.position += transform.right * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                CharacterModel.transform.rotation = Quaternion.Euler(0, -100, 0);
                transform.position -= transform.right * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bAttacking = true;
                state_cur = eAnimationState.attack;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state_cur = eAnimationState.attack;
        }
        else
        {
            state_cur = eAnimationState.idle;
        }
    }

    public void AdjustState()
    {
        switch (state_cur)
        {
            case eAnimationState.idle:
                SetAnimation("Idle");
                break;
            case eAnimationState.walk:
                SetAnimation("Walk");
                break;
            case eAnimationState.attack:
                Attack();
                break;
            case eAnimationState.hit:

                break;
        }
    }

    void Attack()
    {
        SetAnimation("Attack", "Idle");
        //Instantiate(AttackSound, transform);
        //Instantiate(AttackFX, transform.position, transform.rotation);
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
        float delay = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //float delay = animator.runtimeAnimatorController.animationClips[0].length;
        //float delay = GetAnimationDuration(anim);
        print(anim_cur);
        print(delay);
        yield return new WaitForSeconds(delay); // 
        SetAnimation(anim);
    }

    float GetAnimationDuration(string animName)
    {
        var controller = animator.runtimeAnimatorController;
        if (controller == null)
            return 0f;

        foreach (var clip in controller.animationClips)
        {
            if (clip.name == animName)
            {
                return clip.length;
            }
        }

        return 0f;
    }

    //float GetAnimationDuration(string anim)
    //{
    //    AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
    //    for (int i = 0; i < clips.Length; i++)
    //    {
    //        if (clips[i].name == anim) return clips[i].length;
    //    }
    //
    //    return 0;
    //}
}
