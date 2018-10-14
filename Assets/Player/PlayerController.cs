using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float shellRadius = 0.01f;
    public float groundNormale = 0.65f;
    public float minMoveDistance = 0.01f;
    public float airGravity = 2f;
    public float groundGravity = 8f;
    public float speed = 3f;
    public float jumpForce = 5f;
    public float angleSpeed = 0.15f;


    private bool Grounded
    {
        get { return grounded; }
        set
        {
            grounded = value;
            gravityModifier = (value) ? groundGravity : airGravity;
        }
    }

    private bool grounded;
    private Vector2 velocity;
    private float gravityModifier = 1f;

    private RaycastHit2D[] hits = new RaycastHit2D[8];
    private ContactFilter2D filter2d = new ContactFilter2D();
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rig2d;
    private AudioSource audioSource;

    public AudioClip jumpClip;
    public AudioClip walkClip;
    private float h = 0;
    public float footstepsDelay;
    private bool footstepsEnabled;
    private bool footstepsPlaying = true;

    private void Start()
    {
        rig2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        velocity = Vector2.zero;
        filter2d.layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        filter2d.useTriggers = false;
        filter2d.useLayerMask = true;
    }

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = 0;
        InputMove();
        Vector2 deltaPosition = velocity * Time.deltaTime;
        if (h != 0)
            MoveX(Vector2.right * deltaPosition.x);
        MoveY(Vector2.up * deltaPosition.y);
        UpdateAnimation(h);
    }

    private IEnumerator FootstepsSound()
    {
        for (; ; )
        {
            footstepsPlaying = false;
            audioSource.PlayOneShot(walkClip);
            yield return new WaitForSeconds(footstepsDelay);
            footstepsPlaying = true;
        }
    }

    private void InputMove()
    {
        h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            velocity.x = h * speed;
            spriteRenderer.flipX = (h < 0) ? true : false;
            UpdateAnimation(h);
            if (!footstepsEnabled && Mathf.Abs(h) > 0.1f && grounded && footstepsPlaying)
                StartCoroutine("FootstepsSound");
            footstepsEnabled = true;
        }
        if (Mathf.Abs(h) < 0.1f || !grounded)
        {
            footstepsEnabled = false;
            footstepsPlaying = true;
            StopCoroutine("FootstepsSound");
            UpdateAnimation(0);
        }
        if (Input.GetKey(KeyCode.Space) && Grounded)
        {
            GameManager.Instance.Jump();
            velocity.y = jumpForce;
            Grounded = false;
            animator.SetBool("Jump", true);
            animator.SetFloat("JumpDiraction", 0);
        }

    }

    private void UpdateAnimation(float speed)
    {
        animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    private void MoveX(Vector2 move)
    {
        float moveDistance = move.magnitude;
        int hitCount = rig2d.Cast(move, filter2d, hits, move.magnitude + shellRadius);
        if (moveDistance > minMoveDistance)
        {
            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    switch (hits[i].transform.tag)
                    {
                        case "Platform":
                            {
                                if (hits[i].distance == 0 || hits[i].normal.y != 1f)
                                {
                                    rig2d.position += move.normalized * moveDistance;
                                    velocity.x = 0;
                                    return;
                                }
                                break;
                            }
                        case "Coin":
                            {
                                GameManager.Instance.PickUpCoin();
                                Destroy(hits[i].transform.gameObject);

                                rig2d.position += move.normalized * moveDistance;
                                velocity.x = 0; break;
                            }
                        case "Trap": { SceneManager.LoadScene("SampleScene", LoadSceneMode.Single); break; }
                    }

                    Vector2 normal = hits[i].normal;
                    Vector2 groundNormal = new Vector2(normal.y, -normal.x);
                    if (normal.x >= 0 && normal.x != 1f && normal.y >= 0)
                    {

                        move = -moveDistance * groundNormal.normalized * angleSpeed;
                    }
                    else if (normal.x <= 0 && normal.x != -1f && normal.y >= 0)
                    {
                        move = moveDistance * groundNormal.normalized * angleSpeed;
                    }
                    else
                        moveDistance = hits[i].distance - shellRadius;

                }

            }
            rig2d.position += move.normalized * moveDistance;
            velocity.x = 0;
        }
    }

    private void MoveY(Vector2 move)
    {
        float moveDistance = move.magnitude;
        int hitCount = rig2d.Cast(move, filter2d, hits, move.magnitude + shellRadius);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                string hitTag = hits[i].transform.tag;
                if (hitTag.Equals("Platform"))
                {
                    if (velocity.y > 0 || hits[i].distance == 0)
                    {
                        animator.SetBool("Jump", true);
                        rig2d.position += move.normalized * moveDistance;
                        Grounded = false;
                        if (velocity.y <= 0)
                            animator.SetFloat("JumpDiraction", 1);
                        return;
                    }

                }
                else if (hitTag.Equals("Coin"))
                {
                    GameManager.Instance.PickUpCoin();
                    Destroy(hits[i].transform.gameObject);
                    Vector2 deltaPosition = move.normalized * moveDistance;
                    if (deltaPosition.magnitude > minMoveDistance * 2f)
                        animator.SetBool("Jump", true);
                    if (rig2d.position.y > (rig2d.position + deltaPosition).y)
                        animator.SetFloat("JumpDiraction", 1);
                    rig2d.position += deltaPosition;
                    Grounded = false;
                    return;
                }
                else if (hitTag.Equals("Trap"))
                {
                    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                }
                animator.SetBool("Jump", false);
                moveDistance = (hits[i].normal.x == -1f) ? 0 : hits[i].distance - shellRadius;
                CheckGround(hits[i].normal);
            }
            rig2d.position += move.normalized * moveDistance;
            velocity.y = 0;
        }
        else
        {
            Vector2 deltaPosition = move.normalized * moveDistance;
            if (deltaPosition.magnitude > minMoveDistance * 2f)
                animator.SetBool("Jump", true);
            if (rig2d.position.y > (rig2d.position + deltaPosition).y)
                animator.SetFloat("JumpDiraction", 1);
            rig2d.position += deltaPosition;
            Grounded = false;
        }
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

    private void CheckGround(Vector2 normal)
    {
        Grounded = (normal.y > groundNormale) ? true : false;
    }
}

