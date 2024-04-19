using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hzz_RandomMovement : MonoBehaviour
{
    public float movementSpeed = 0.2f;
    public float moveThreshold = 0.1f;
    public float directionChangeInterval = 8f;

    private Animator animator;
    private Vector3 movementDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GenerateRandomMovementDirection();

        // 在directionChangeInterval秒后开始，每隔directionChangeInterval秒调用一次ChangeMovementDirection方法
        InvokeRepeating("ChangeMovementDirection", directionChangeInterval, directionChangeInterval);
    }

    private void Update()
    {
        MoveCharacter();
        UpdateAnimator();
    }

    private void MoveCharacter()
    {
        Vector3 movement = movementDirection.normalized * movementSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void GenerateRandomMovementDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        movementDirection = new Vector3(randomX, randomY, 0f).normalized;
    }

    private void UpdateAnimator()
    {
        bool isMoving = movementDirection.magnitude > moveThreshold;
        animator.SetBool("Move", isMoving);
        if (movementDirection != Vector3.zero)
        {
            transform.localScale = new Vector3(-Mathf.Sign(movementDirection.x), 1f, 1f);
        }
    }

    private void ChangeMovementDirection()
    {
        GenerateRandomMovementDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果发生碰撞，立即改变移动方向
        GenerateRandomMovementDirection();
    }
}

