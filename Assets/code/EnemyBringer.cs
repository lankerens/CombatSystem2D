using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBringer : EnemyFather {
    // public GameObject ParantObj;

    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        base.Update();
        attack();
    }


    // TODO 还可以优化我觉得
    // 受伤动画
    protected override void hurtAnimator() {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("hurt") && stateInfo.normalizedTime > 0.90f) {
            _animator.SetBool("ishurt", false);
            isHurt = false;
        }
        else if (isHurt && !stateInfo.IsName("death") && !stateInfo.IsName("hurt")) {
            // 播放受击动画
            _animator.SetBool("ishurt", true);
            _animator.SetTrigger("hurt");
        }
    }

    protected override void myDestroy() {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (life <= 0) {
            if (!stateInfo.IsName("death")) {
                _animator.SetTrigger("death");
                // 敌人数量-1
                // GameObject.Find("Ground").GetComponent<GroundInit>().updateEnemyCount(-1);
                // Debug.Log(currentName);
                GameObject.Find(currentName).GetComponentInChildren<GroundInit>().updateEnemyCount(-1);

                // TODO 注意这里 transform.root 不能为 null
                // Unity中将下行脚本指定给任意子对象，可以销毁该对象的根父级。
                Destroy(transform.root.gameObject, 0.83f);
                // Destroy(ParantObj, 0.83f);
            }
        }
    }

    /**
     * 攻击动画
     */
    void attack() {
        // Debug.Log(isAttack);
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        if (!isAttack && (Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position)) < 3f) {
            // 这个一次激活就可以. 
            _animator.SetTrigger("attackTrigger");
            // 布尔值需要持续激活
            // _animator.SetBool("attack", true);
            Run(false);
            // 朝向player 根据原图来判断的，原图敌怪是朝左， 所以当用户在敌怪左边，要为true 才是朝向player，反之则false
            Flip(transform.position.x - FindObjectOfType<PlayerController>().transform.position.x > 0);
            isAttack = true;
        } else if ((Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position)) > 3.2f && state.normalizedTime > 1f) {
            // 距离离开了并且动画播放完毕
            isAttack = false;
        }
    }
}