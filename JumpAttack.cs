using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public Collider2D jumpColl;
    Vector2 rightJumpOffset;
    public float damage = 2;
    
    private void Start(){
        rightJumpOffset = transform.localPosition;
    }

    public void JumpRight() {
        jumpColl.enabled = true;
        transform.localPosition = rightJumpOffset;
    }

    public void JumpLeft() {
        jumpColl.enabled = true;
        transform.localPosition = new Vector3(rightJumpOffset.x*-1,rightJumpOffset.y);
    }

    public void StopJump() {
        jumpColl.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            SquirrelScript enemy = other.GetComponent<SquirrelScript>();
            
            if(enemy != null) {
                enemy.Health-=damage;
            }
        }
    }
}
