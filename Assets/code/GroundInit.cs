using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundInit : MonoBehaviour
{
    public int enemyCount;
    private int _currentCount;
    private float _creatWaitTime;
    public GameObject enemy;

    // 获取当前这个地面的边界点
    private Transform[] _child;
    // private Transform _transform;
    // private Transform _tempLeftDownPos;
    // private Transform _tempRightDownPos;
    
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(enemy.GetComponentInChildren<EnemyFather>().levelOffsetPos);
        // 将敌怪那边的偏移量赋值上当前的地面坐标 x = 地面中心点， y = 地表平面
        _child = gameObject.GetComponentsInChildren<Transform>();
        // 0 是 ground 本身
        // Debug.Log(transform.position); 地面坐标(-15.54, 0.16, 0.00)
        // Debug.Log(child[1].position);
        // Debug.Log(child[2].position);
        // TODO Instantiate 克隆一次。调用一次start方法. 这里调用Instantiate(transform) 就死循环了
        // TODO 解答： https://forum.unity.com/threads/making-a-new-transform-in-code.49277/
        // Debug.Log(enemy.GetComponentInChildren<EnemyFather>()._leftDownPos.position);
        // Debug.Log(enemy.GetComponentInChildren<EnemyFather>()._rightUpPos.position);
        // _tempLeftDownPos = new GameObject("tempLD--" + transform.root.name).transform;
        // _tempRightDownPos = new GameObject("tempRD--"  + transform.root.name).transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        // 根据数量生成敌人
        createEnemy();
    }


    // // 生成相应数量的敌人
    void createEnemy()
    {
        _creatWaitTime -= Time.deltaTime;
        if (_creatWaitTime <= 0 && _currentCount < enemyCount)
        {
            // Quaternion.identity:无旋转
            // 生成一个, 还需要手动再销毁一次
            var e = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y), quaternion.identity);
            
            e.GetComponentInChildren<EnemyFather>().currentName = transform.root.name;
            // 给每一个敌怪绑定边界
            // 左边界
            // e.GetComponentInChildren<EnemyFather>()._leftDownPos = _tempLeftDownPos;
            // e.GetComponentInChildren<EnemyFather>()._leftDownPos.position = new Vector3(_child[1].position.x, _child[1].position.y);
            e.GetComponentInChildren<EnemyFather>().limtLeftX = _child[1].position.x;
            e.GetComponentInChildren<EnemyFather>().limtY = _child[1].position.y;
            
            // 右边界
            // enemy.GetComponentInChildren<EnemyFather>()._rightUpPos = Instantiate(transform);
            // e.GetComponentInChildren<EnemyFather>()._rightUpPos = _tempRightDownPos;
            // e.GetComponentInChildren<EnemyFather>()._rightUpPos.position = new Vector3(_child[2].position.x, _child[2].position.y);
            e.GetComponentInChildren<EnemyFather>().limtRightX = _child[2].position.x;
            
            _currentCount++;
            _creatWaitTime = Random.Range(2, 6); // 随机
        }
    }


    // 提供给外部的
    public void updateEnemyCount(int count)
    {
        _currentCount += count;
        // Debug.Log(_currentCount);
    }
}