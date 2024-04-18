using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hzz_Camera : MonoBehaviour
{
    public Transform target; // 要跟随的目标（例如人物）

    public float smoothSpeed = 0.125f; // 平滑移动的速度

    private Vector3 offset; // 相机与目标的初始偏移量

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position; // 计算初始偏移量
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = target.position + offset; // 计算目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // 使用插值平滑移动
        transform.position = smoothedPosition; // 更新相机位置
    }
}
