﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerContoller
{
    [Serializable]
    public abstract class PhysicalBody : MonoBehaviour
    {
        public float shellRadius = 0.01f;
        public float groundNormale = 0.7f;
        public float minMoveDistance = 0.01f;
        public float speed = 5f;
        public float jumpForce = 10f;
        public float angleSpeed = 0.15f;

        public UnityEvent onCoinPickUp;
        public UnityEvent<Transform> onTrapEntered;


        [Serializable]
        private enum Gravity
        {
            GROUND = 20,
            AIR = 2
        }


        protected abstract bool Jump();

        private Gravity gravity;
        protected bool onGround = false;
        private Vector2 velocity;
        private float inputSpeed = 0;

        private Vector2 framePosition;

        private RaycastHit2D[] hits = new RaycastHit2D[8];
        private RaycastHit2D hit = new RaycastHit2D();
        private ContactFilter2D generalFilter = new ContactFilter2D();
        private ContactFilter2D enviromentFilter = new ContactFilter2D();
        private Rigidbody2D rig2d;

        private void Start()
        {
            rig2d = GetComponent<Rigidbody2D>();
            velocity = Vector2.zero;
            generalFilter.layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
            generalFilter.useTriggers = false;
            generalFilter.useLayerMask = true;
            enviromentFilter = new ContactFilter2D();
            enviromentFilter.SetLayerMask(LayerMask.GetMask("Moveble"));
            enviromentFilter.useTriggers = false;
            enviromentFilter.useLayerMask = true;
            gravity = Gravity.AIR;
        }

        private void FixedUpdate()
        {
            velocity.x = 0;
            framePosition = rig2d.position;
            HorizontalInput();
            if (onGround)
            {
                velocity.y = 0;
                gravity = Gravity.GROUND;
            }
            else
                gravity = Gravity.AIR;
            if (Jump())
            {
                velocity.y = jumpForce;
                onGround = false;
                gravity = Gravity.AIR;
            }
            velocity += Physics2D.gravity * Time.deltaTime * (int)gravity;
            Vector2 deltaPosition = velocity * Time.deltaTime;

            MoveY(Vector2.up * deltaPosition.y);
            if (inputSpeed != 0)
                MoveX(Vector2.right * deltaPosition.x);
        }

        private void HorizontalInput()
        {
            inputSpeed = Input.GetAxis("Horizontal");
            velocity.x = inputSpeed * speed;
        }

        private void MoveX(Vector2 move)
        {
            float moveDistance = move.magnitude;
            if (moveDistance > minMoveDistance)
            {
                int hitCount = rig2d.Cast(move, enviromentFilter, hits, move.magnitude + shellRadius);
                if (hitCount > 0)
                {
                    hit = hits[0];
                    if (!hit.transform.CompareTag("Platform"))
                        move = CalculateGroundVector(move, ref moveDistance, hit);
                }
                rig2d.position += move.normalized * moveDistance;
                //  velocity.x = 0;
            }
        }

        private void MoveY(Vector2 move)
        {
            onGround = false;
            float moveDistance = move.magnitude;
            int hitCount = rig2d.Cast(move, generalFilter, hits, moveDistance + shellRadius);
            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    hit = hits[i];
                    switch (hit.transform.tag)
                    {
                        case "Coin":
                            {
                                break;
                            }
                        case "Trap":
                            {
                                break;
                            }
                        default:
                            {
                                Debug.Log(hit.normal);
                                if (hit.normal.y <= 0 && hit.transform.tag != "Platform" && hit.distance != 0)
                                {
                                    moveDistance = hit.distance - shellRadius;
                                    velocity.y = 0;
                                    break;
                                }
                                if (hit.distance != 0 && hit.normal.y > 0.5f)
                                {
                                    moveDistance = (hit.normal.x == -1f) ? 0 : hit.distance - shellRadius;
                                    onGround = true;
                                }
                                else if (hit.transform.tag != "Platform" && hit.distance > shellRadius)
                                {

                                    move.x += hit.normal.x;
                                    onGround = false;

                                }
                                break;
                            }
                    }

                }
            }
            Vector2 deltaPosition = move.normalized * moveDistance;
            rig2d.position += deltaPosition;
        }


        private float NormalToAngle(Vector2 normal)
        {
            int offset = 0;
            if (normal.x == 0 && normal.y == 0)
                return -1;
            if (normal.x >= 0 && normal.y <= 0)
                offset = 90;
            else if (normal.x >= 0 && normal.y >= 0)
                offset = 180;
            else if (normal.x <= 0 && normal.y >= 0)
                offset = 270;
            return offset + Vector2.Angle(new Vector2(1, 0), new Vector2(Mathf.Abs(normal.x), Mathf.Abs(normal.y)));
        }

        private Vector2 CalculateGroundVector(Vector2 move, ref float moveDistance, RaycastHit2D hit)
        {
            Vector2 normal = hit.normal;
            Vector2 groundNormal = new Vector2(normal.y, -normal.x);
            if (normal.x >= 0 && normal.x != 1f && normal.y >= 0)
            {
                return move = -moveDistance * groundNormal.normalized * angleSpeed;
            }
            else if (normal.x <= 0 && normal.x != -1f && normal.y >= 0)
            {
                return move = moveDistance * groundNormal.normalized * angleSpeed;
            }
            else
                moveDistance = (hit.distance - shellRadius);
            return move.normalized * moveDistance;
        }

        private bool isNegative(float value)
        {
            return (value < 0) ? true : false;
        }

        private bool isPositive(float value)
        {
            return (value > 0) ? true : false;
        }
    }
}