using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // 移动需要的速度
    public float speed;

    // 获取刚体组件
    private Rigidbody2D rb2;
    private Animator _animation;

    // 跳跃
    public float jump;

    // 获取与地面接触的方形碰撞检测
    private BoxCollider2D bc2;
    private bool _isTouchingLayers;

    // 二段跳的速度
    public float doubleJump;
    private bool _canDoubleJump;


    // Start is called before the first frame update
    void Start() {
        // 在start这里进行获取
        rb2 = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        bc2 = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        Flip();
        Run();
        Jump();
        ChangeMovement();
        AttackTrigger();
    }


    // 攻击动画
    void AttackTrigger() {
        var aniSate = _animation.GetCurrentAnimatorStateInfo(0);
        var attack = Input.GetButtonDown("Attack");
        if (attack) {
            if (aniSate.IsName("attack1") && aniSate.normalizedTime > 0.70f) {
                _animation.SetInteger("AttackType", 2);
                _animation.SetTrigger("AttackTrigger");
            }
            else if (aniSate.IsName("attack2") && aniSate.normalizedTime > 0.70f) {
                _animation.SetInteger("AttackType", 3);
                _animation.SetTrigger("AttackTrigger");
            }
            else if (!((aniSate.IsName("attack3") && aniSate.normalizedTime < 0.70f)
                       || (aniSate.IsName("attack2") && aniSate.normalizedTime < 0.70f)
                       || (aniSate.IsName("attack1") && aniSate.normalizedTime < 1.00f))) {
                _animation.SetInteger("AttackType", 1);
                _animation.SetTrigger("AttackTrigger");
            }
        }
        // else
        // {
        //     if ((aniSate.IsName("attack3")
        //          || aniSate.IsName("attack2")
        //          || aniSate.IsName("attack1")) && aniSate.normalizedTime > 1.00f)
        //     {
        //         _animation.SetInteger("AttackType", 0);
        //     }
        // }
    }

    // 控制动作的动画
    void ChangeMovement() {
        if (_isTouchingLayers) {
            // 设置站地面上的动画
            _animation.SetBool("stand", true);
            _animation.SetBool("fall", false);
            _animation.SetBool("doubleFall", false);
        }
        else {
            // 播放第一段跳跃动画
            if (_animation.GetBool("jump")) {
                // 播放下落动画
                if (rb2.velocity.y < 0.08f) {
                    _animation.SetBool("jump", false);
                    _animation.SetBool("fall", true);
                }
                else {
                    // 播放上升动画, 另一个条件已经 jump == true
                    _animation.SetBool("stand", false);
                }
            }
            else if (_animation.GetBool("doubleJump")) {
                // 播放二段跳动画
                if (rb2.velocity.y < 0.08f) {
                    // 播放二段跳的下落动画
                    _animation.SetBool("doubleJump", false);
                    _animation.SetBool("doubleFall", true);
                }
            }
        }
    }


    // 跳跃
    void Jump() {
        var down = Input.GetButtonDown("Jump");
        _isTouchingLayers = bc2.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (down && _isTouchingLayers) {
            // 设置动画条件
            _animation.SetBool("jump", true);
            var vector2 = new Vector2(0.0f, jump);
            rb2.velocity = Vector2.up * vector2;
            _canDoubleJump = true;
        }
        else if (down && _canDoubleJump) {
            // 设置动画条件
            _animation.SetBool("jump", false);
            _animation.SetBool("doubleJump", true);
            // 可以二段跳
            var v = new Vector2(0.0f, doubleJump);
            // 单位向量 * v
            rb2.velocity = Vector2.up * v;
            _canDoubleJump = false;
        }
    }


    // 图片反转（主要是人物左右的朝向）
    void Flip() {
        var moveDir = Input.GetAxis("Horizontal");
        if (moveDir > 0.1f) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveDir < -0.1f) {
            // 反转
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Run() {
        // Horizontal 水平的
        var moveDir = Input.GetAxis("Horizontal");

        // Debug.Log(rb2.velocity.y);
        var vector2 = new Vector2(moveDir * speed, rb2.velocity.y);
        rb2.velocity = vector2;

        // Mathf.Epsilon; A tiny floating point value (Read Only).
        var isRun = Mathf.Abs(vector2.x) > Mathf.Epsilon;
        // 是否奔跑状态
        _animation.SetBool("run", isRun);
    }
}