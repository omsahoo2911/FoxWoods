using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 700f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public JumpAttack jumpAttack;
    Vector2 movementInput;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;
    private Transform target;

    public AudioSource jumpSE;
    public AudioSource walkSE;

    public Vector2 PlayerInput;
    public int maxHealth = 100;
    int currentHealth;

    private bool isMoving = false;

    public bool IsMoving {
        set{
            isMoving = value;
            anim.SetBool("isMoving", value);
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        target = GameObject.FindWithTag("Player").transform;
    }


    private void FixedUpdate() { 
        if(canMove && PlayerInput != Vector2.zero){
             
            rb.velocity = PlayerInput * moveSpeed;
            IsMoving = true;
            setDirection();

        } else {
            rb.velocity = Vector2.zero;
            IsMoving = false;
        }
    }


    void setDirection(){
        if(PlayerInput.x>0) {
            transform.localScale = new Vector3(1f,1f,1f);
        } else if (PlayerInput.x<0) {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
    }
    
    void OnMove(InputValue movementValue){
        PlayerInput = movementValue.Get<Vector2>();
    }

    void OnFire(){
        anim.SetTrigger("jump");
    }
    public void walkSound(){
        walkSE.Play();
    }

    public void JumpAttack(){
        LockMovement();
        jumpSE.Play();
        if(sprite.flipX == true){
            jumpAttack.JumpLeft();
        } else {
            jumpAttack.JumpRight();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth-=damage;

        if(currentHealth <= 0)
        {
            anim.SetTrigger("isDead");
            Die();
        } else {
            anim.SetTrigger("tookDamage");
        }
    }

    void Die()
    {
        anim.SetTrigger("isDead");
    }

    public void EndAttack(){
        UnlockMovement();
        jumpAttack.StopJump();
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }
}