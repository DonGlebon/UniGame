using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    public float speed = 5f;
    public float rotationDelay = 1f;
    public float xOffset;
    public float castDistance = 1f;

    private Rigidbody2D rig2d;
    private SpriteRenderer spriteRenderer;
    private Vector2 deltaPosition;
  
    private float moveDiraction = 1f;
    private Animator animator;


    // Use this for initialization
    void Start()
    {
        rig2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        deltaPosition = Vector2.zero;
        StartCoroutine("Move");
    }
    
    IEnumerator Move()
    {
        for (; ; )
        {
            deltaPosition = Vector2.right * Time.deltaTime * speed * moveDiraction * Time.deltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position + Vector3.right * xOffset * moveDiraction, Vector2.down,castDistance);
            if(hit2D.collider == null || hit2D.transform.tag.Equals("Trap"))
            {
                moveDiraction *= -1f;
                animator.SetFloat("Speed", 0f);
                yield return new WaitForSeconds(rotationDelay);
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            animator.speed = 0.65f;
            animator.SetFloat("Speed", 1f);
            rig2d.position += deltaPosition;
            yield return new WaitForFixedUpdate();
        }
    }
}
