using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraScript : MonoBehaviour
{
private Animator anim;
    public float health = 1;
    public float moveSpeed;
    public float collisionOffset = 0.01f;
    public float checkRadius;
    public float attackRadius;
    public LayerMask whatIsPlayer;
    private SpriteRenderer sprite; 
    private Transform target;
    private Transform treeTarget;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 dir;
    private bool isInChaseRange;
    private bool isInAttackRange;
    private bool alive = true;
    private float scaleX = 1.2f;
    private float scaleY = 1.2f;

    private int attackDamage = 40;

    private bool isMoving = false;
    public Transform attackPoint;
    public int maxHealth = 100;
    int currentHealth;


    public bool IsMoving {
        set{
            isMoving = value; 
            anim.SetBool("isRunning", value);
        }
    }


    public float Health {
        set { 
            health = value;
            print(health);
            if(health<=0){
                alive = false;
                Defeated();
            } else {

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
        isInAttackRange = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsPlayer);

        dir =  target.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg; 
        dir.Normalize();
        movement = dir;

    }

    private void FixedUpdate(){
        if(isInAttackRange){
            IsMoving = false;
            anim.SetTrigger("Attack");
            rb.velocity = Vector2.zero;
        } else {
            if(isInChaseRange && alive){
                rb.velocity = movement * moveSpeed;
                setDirection();
                IsMoving = true;
            } else {
                rb.velocity = Vector2.zero;
                IsMoving = false;
            }
        }
    }

    void setDirection(){
        if(movement.x>0) {
            transform.localScale = new Vector3(scaleX,scaleY,1f);
        } else if (movement.x<0) {
            transform.localScale = new Vector3(-scaleX,scaleY,1f); 
        }
    }

    void Attack()
    {
        // anim.SetTrigger("Attack");
        IsMoving = true;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsPlayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint==null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
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
