using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSense : MonoBehaviour {
    private static AttackSense instance;

    public static AttackSense Instance {
        get {
            if (instance == null) {
                instance = Transform.FindObjectOfType<AttackSense>();
            }

            return instance;
        }
    }

    // 相机抖动
    private bool isShake;

    /**
     * 给外部调用
     */
    public void HitPause(float duration) {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(float duration) {
        float pauseTime = duration / 60f;
        // 暂停
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        // 开始
        Time.timeScale = 1;
    }

    /**
     *  给外部调用
     */
    public void CameraShake(float duration, float strength) {
        if (!isShake) {
            StartCoroutine(Shake(duration, strength));
        }
    }


    /**
     * strength 震动强度
     */
    IEnumerator Shake(float duration, float strength) {
        isShake = true;
        // 获取相机的位置
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.position;

        while (duration > 0) {
            // 生成球体范围的一个随机坐标
            camera.position = Random.insideUnitSphere * strength + startPosition;
            duration -= Time.deltaTime;
            yield return null;
        }

        camera.position = startPosition;
        isShake = false;
    }
}