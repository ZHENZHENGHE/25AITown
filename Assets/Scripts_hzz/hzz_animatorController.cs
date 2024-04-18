using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hzz_animatorController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public float moveSpeed = 5f; //定义移动速度
    public float movementSpeed = 0.2f; //定义移动速度系数
    private bool _isRunning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 获取WSAD键的输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 计算移动向量
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized * movementSpeed;
        // 更新角色位置
        transform.position += movement * moveSpeed * Time.deltaTime;

        // 检查是否按下了 R 键
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isRunning = true;
        }
        // 检查是否抬起了 R 键
        if (Input.GetKeyUp(KeyCode.R))
        {
            _isRunning = false;
        }

        // 根据移动方向和运行状态设置动画参数
        float runNumber = (_isRunning) ? 1f : 0.5f;
        animator.SetFloat("RunNumber", runNumber); // 设置混合树状态

        animator.SetBool("Move", movement.magnitude > 0); // 设置移动状态
        movementSpeed = (_isRunning) ? 0.22f : 0.2f; // 设置移动速度

        // 根据移动方向设置角色左右朝向
        if (movement != Vector3.zero)
        {
            transform.localScale = new Vector3(-Mathf.Sign(moveHorizontal), 1, 1); // 根据水平移动方向设置朝向
        }
    }
}
