using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public JumpAttack jumpAttack;
    Vector2 movementInput;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;
    public AudioSource jumpSE;
    public AudioSource walkSE;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate() { 
        if(canMove){
            if(movementInput != Vector2.zero){
            bool success = TryMove(movementInput);

            if (!success) {
                success = TryMove(new Vector2(movementInput.x,0));
            }

            if (!success) {
                success = TryMove(new Vector2(0,movementInput.y));
            }
            anim.SetBool("isMoving", success);
            } else {
                anim.SetBool("isMoving",false);
            }

            setDirection();

        }
    }

    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero){
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);
            
            if(count==0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    void setDirection(){
        if(movementInput.x<0) {
            sprite.flipX = true;
        } else if (movementInput.x>0) {
            sprite.flipX = false;
        }
    }

    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
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
