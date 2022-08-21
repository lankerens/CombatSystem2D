using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float reachTime;
    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        // 玩家还活着
        if (player != null && transform.position != player.position)
        {
            //在两个点之间进行线性插值。
            transform.position = Vector3.Lerp(transform.position, player.position, reachTime);
        }
    }
}
