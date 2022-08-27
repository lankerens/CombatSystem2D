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
}