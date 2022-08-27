using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour {
    private PolygonCollider2D[] _polygonCollider2D;

    // Start is called before the first frame update
    void Start() {
        _polygonCollider2D = GetComponentsInChildren<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update() {
    }


    // 攻击动画
    void HitStart(int index) {
        _polygonCollider2D[index].enabled = true;
    }

    void HitEnd(int index) {
        _polygonCollider2D[index].enabled = false;
    }
}