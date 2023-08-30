using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    public bool canMove = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 14f;
    [Header("Climb")]
    [SerializeField] private float climbingSpeed = 4f;
    [SerializeField] private float climbingHorizontalSpeed = 2f;

    [Header("Ground Check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask jumpableGround;

    private float horizontalMovement;
    private float verticalMovement;
    private float initialMoveSpeed;
    private bool jump;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool isClimbing;

    private HashSet<GameObject> ladders = new HashSet<GameObject> ();

    Rigidbody2D rigidBody;
    Animator animator;
    BoxCollider2D coll;
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;

    private enum Animations {
        Idle,
        Run,
        Jump,
        Fall,
        Attack,
        Climbing
    };

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        initialMoveSpeed = moveSpeed;
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !isAttacking)
        {
            jump = true;
        }

        if (ladders.Count > 0 && Mathf.Abs(verticalMovement) > 0f && IsGrounded() && !isAttacking)
        {
            isClimbing = true;
            moveSpeed = climbingHorizontalSpeed;
        }
        else if (ladders.Count <= 0 || verticalMovement == 0f)
        {
            isClimbing = false;
            moveSpeed = initialMoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ApplyPotion();
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        MovePlayer();

        if (jump)
        {
            Jump();
        }

        if (isClimbing)
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalMovement * climbingSpeed);
        }
        else
        {
            rigidBody.gravityScale = 3f;
        }
        
    }

    private void MovePlayer()
    {
        if (!isAttacking || (isAttacking && !IsGrounded()))
        {
            rigidBody.velocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.velocity.y);
        }
        else if ((horizontalMovement < 0f || horizontalMovement > 0f) && isAttacking)
        { 
            // Set x velocity to 0 when the player is moving and attacking
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }
    }
    
    private void Jump()
    {
        AudioManager.Instance.PlaySFXSound("jump");
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        jump = false;
    }

    private void UpdateAnimation()
    {
        Animations currentAnimation;

        //Running conditions
        if ((horizontalMovement < 0f || horizontalMovement > 0f) && canMove)
        {
            FlipPlayer();
            currentAnimation = Animations.Run;
        }
        else
        {
            currentAnimation = Animations.Idle;
        }

        //Jumping conditions
        if (rigidBody.velocity.y > 0.1f)
        {
            currentAnimation = Animations.Jump;
        }
        else if (rigidBody.velocity.y < -0.1f)
        {
            currentAnimation = Animations.Fall;

        }

        //Attack conditions
        if (isAttacking)
        {
            currentAnimation = Animations.Attack;
        }

        //Climbing conditions
        if (isClimbing)
        {
            currentAnimation = Animations.Climbing;
        }

        if (animator.GetInteger("currentAnimation") != (int) currentAnimation)
        {
            animator.SetInteger("currentAnimation", (int) currentAnimation);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, jumpableGround);
    }

    private void FlipPlayer()
    {
        if (horizontalMovement < 0f && canMove)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalMovement > 0f && canMove)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    private void ApplyPotion()
    {
        InventoryItem item = InventoryManager.Instance.GetActiveSlotItem();

        if (!item)
        {
            Debug.Log("No item to use");
            return;
        }

        if (item.GetItemType() == ActionType.health)
        {
            bool used = playerHealth.UseExtraHealthPotion(item.GetItemExtraHealth());

            if (used)
            {
                Destroy(item.gameObject);
            }
        }
        else
        {
            playerAttack.UsePotion(item.GetItemType(), item.GetItemDuration());
            Destroy(item.gameObject);
        }
    }

    public void LockPlayer()
    {
        canMove = false;
        rigidBody.velocity = Vector3.zero;
    }
    
    public void UnlockPlayer()
    {
        canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            ladders.Add(collision.gameObject);   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            ladders.Remove(collision.gameObject);
        }
    }
}
