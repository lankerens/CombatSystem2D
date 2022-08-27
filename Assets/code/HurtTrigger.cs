using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtTrigger : MonoBehaviour {
    // 伤害
    public int damage;

    // 顿帧时间
    public float duration;
    // 震动时间
    public float shakeTime;
    // 震动强度
    public float strength;
    
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    // 触碰到了就计算伤害
    void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log(other.Equals(other.GetComponent<BoxCollider2D>()));
        // 是否是敌人对象 并且是个触发器组件
        if (other.gameObject.CompareTag("Enemy") && other.isTrigger) {
            // 伤害计算
            other.GetComponent<EnemyFather>().BeHurt(damage);
            // 触发打击感
            AttackSense.Instance.HitPause(duration);
            AttackSense.Instance.CameraShake(shakeTime, strength);
        }
    }
}