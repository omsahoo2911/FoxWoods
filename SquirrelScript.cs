using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelScript : MonoBehaviour
{
    private Animator anim;
    public float health = 1;
    public float moveSpeed;
    public float collisionOffset = 0.01f;
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
    private bool alive = true;
    private float scaleX = 0.8f;
    private float scaleY = 0.8f;

    private bool isMoving = false;

    public bool IsMoving {
        set{
            isMoving = value;
            anim.SetBool("isRunning", value);
        }
    }


    public float Health {
        set { 
            health = value;
            if(health<=0){
                alive = false;
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

    }

    private void FixedUpdate(){
        if(isInChaseRange && alive){
            rb.velocity = movement * moveSpeed;
            setDirection();
            IsMoving = true;
        } else {
            rb.velocity = Vector2.zero;
            IsMoving = false;
        }
    }

    void setDirection(){
        if(movement.x>0) {
            transform.localScale = new Vector3(scaleX,scaleY,1f);
        } else if (movement.x<0) {
            transform.localScale = new Vector3(-scaleX,scaleY,1f);
        }
    }

    public void Defeated(){
        anim.SetTrigger("Defeated");
    }

    public void RemoveEnemy(){
        Destroy(gameObject);
    }


    // private void OnTriggerEnter2D(Collider2D other) {
    //     print("enter");
    //     if(other.tag == "Tree" && isInChaseRange){
    //         print("Hittree");
    //         Destroy(gameObject);
    //     }
    // }

}
