using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;
    private float dirX = 0f;
    private float dirY = 0f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 5f;
    private enum MovementState {idle, running, jumping}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(dirX*moveSpeed, dirY*moveSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x*2f,5f);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if(dirX>0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX<0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (dirY!=0)
        {
            state = MovementState.running;  
        }
        else 
        {
            state = MovementState.idle;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            state = MovementState.jumping;
        }
        if ((int)state==2)
        {
            Debug.Log((int)state);
        }
        anim.SetInteger("state", (int)state);
    }
}
