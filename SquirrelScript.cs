using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelScript : MonoBehaviour
{
    private Animator anim;
    public float health = 1;
    public float speed;
    public float collisionOffset = 0.01f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float checkRadius;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsTree;
    private SpriteRenderer sprite;
    private Transform target;
    private Transform treeTarget;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 dir;
    private bool isInChaseRange;
    private bool isInTreeRange;
    bool canMove = true;


    public float Health {
        set { 
            health = value;
            if(health<=0){
                LockMovement();
                Defeated();
            }
        }
        get {
            return health;
        }
    }

    private void Start(){
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player").transform;
        treeTarget = GameObject.FindWithTag("Tree").transform;
    }

    private void Update(){
        anim.SetBool("isRunning",isInChaseRange);
        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        // isInTreeRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsTree);

        dir = transform.position - target.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;

        // if(isInChaseRange && isInTreeRange){
        //     dir = treeTarget.position - transform.position;
        //     angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        //     dir.Normalize();
        //     movement = dir;
        // }
    }

    private void FixedUpdate(){
        if(isInChaseRange){
            bool success = TryMove(movement);
            if (!success) {
                success = TryMove(new Vector2(movement.x,0));
            }

            if (!success) {
                success = TryMove(new Vector2(0,movement.y));
            }
            setDirection();
        }
    }

    private bool TryMove(Vector2 dir){
        if(canMove && dir != Vector2.zero){
            int count = rb.Cast(
                dir,
                movementFilter,
                castCollisions,
                speed * Time.fixedDeltaTime + collisionOffset);
            
            if(count==0){
                rb.MovePosition((Vector2)transform.position + (dir*speed*Time.deltaTime));
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    void setDirection(){
        if(movement.x<0) {
            sprite.flipX = true;
        } else if (movement.x>0) {
            sprite.flipX = false;
        }
    }

    public void Defeated(){
        anim.SetTrigger("Defeated");
    }

    public void RemoveEnemy(){
        Destroy(gameObject);
    }

    public void LockMovement(){
        canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print("enter");
        if(other.tag == "Tree" && isInChaseRange){
            print("Hittree");
            Destroy(gameObject);
        }
    }

}
