using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rbody;
	Animator anim;

    public bool facingRight;
    public bool facingLeft;
    public bool facingUp;
    public bool facingDown;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private void Start(){
        target =  LevelManager.main.path[pathIndex];
        rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
    }
    private void Update(){
        if(Vector2.Distance(target.position, transform.position) <= 0.1f){
            pathIndex++;
            if(pathIndex == LevelManager.main.path.Length){
                Destroy(gameObject);
                return;
            }else{
                target =  LevelManager.main.path[pathIndex];
            }
        }


        if(pathIndex < 1){
            anim.SetBool ("left", false);
            anim.SetBool ("right", false);
            anim.SetBool ("up", true);
            anim.SetBool ("down", false);
            Up();
        }else if(pathIndex >= 1 && pathIndex < 2){
            anim.SetBool ("left", false);
            anim.SetBool ("right", true);
            anim.SetBool ("up", false);
            anim.SetBool ("down", false);
            Right();
        }else if(pathIndex >= 2 && pathIndex < 3){
            anim.SetBool ("left", false);
            anim.SetBool ("right", false);
            anim.SetBool ("up", true);
            anim.SetBool ("down", false);
            Up();
        }else if(pathIndex >= 3 && pathIndex < 4){
            anim.SetBool ("left", true);
            anim.SetBool ("right", false);
            anim.SetBool ("up", false);
            anim.SetBool ("down", false);
            Left();
        }else if(pathIndex >= 4 && pathIndex < 5){
            anim.SetBool ("left", false);
            anim.SetBool ("right", false);
            anim.SetBool ("up", false);
            anim.SetBool ("down", true);
            Down();
        }else if(pathIndex >= 5 && pathIndex < 6){
            anim.SetBool ("left", true);
            anim.SetBool ("right", false);
            anim.SetBool ("up", false);
            anim.SetBool ("down", false);
            Left();
        }else if(pathIndex >= 6 && pathIndex < 7){
            anim.SetBool ("left", false);
            anim.SetBool ("right", false);
            anim.SetBool ("up", true);
            anim.SetBool ("down", false);
            Up();
        }else if(pathIndex >= 7 && pathIndex < 8){
            anim.SetBool ("left", false);
            anim.SetBool ("right", true);
            anim.SetBool ("up", false);
            anim.SetBool ("down", false);
            Right();
        }else if(pathIndex >= 8 && pathIndex < 9){
            anim.SetBool ("left", false);
            anim.SetBool ("right", false);
            anim.SetBool ("up", true);
            anim.SetBool ("down", false);
            Up();
        }else if(pathIndex >= 9 && pathIndex < 10){
            anim.SetBool ("left", true);
            anim.SetBool ("right", false);
            anim.SetBool ("up", false);
            anim.SetBool ("down", false);
            Left();
        }else if(pathIndex >= 10){
            anim.SetBool ("left", false);
            anim.SetBool ("right", false);
            anim.SetBool ("up", true);
            anim.SetBool ("down", false);
            Up();
        }
        // if(pathIndex >= 2 && pathIndex < 6){
        //     facingLeft = false;
        //     facingRight = true;
        //     facingUp = false;
        //     facingDown = false;
        // }else if(pathIndex >= 6 && pathIndex < 7){
        //     facingLeft = false;
        //     facingRight = false;
        //     facingUp = true;
        //     facingDown = false;
        // }else if(pathIndex >= 8){
        //     facingLeft = false;
        //     facingRight = true;
        //     facingUp = false;
        //     facingDown = false;
        // }else if(pathIndex < 2){
        //     facingLeft = false;
        //     facingRight = false;
        //     facingUp = true;
        //     facingDown = false;
        // }else if(pathIndex >= 4 && pathIndex < 5){
        //     facingLeft = false;
        //     facingRight = false;
        //     facingUp = false;
        //     facingDown = true;

        // }

        // if(facingRight){
        //     transform.localScale = new Vector2(1,1);
        // }else{
        //     transform.localScale = new Vector2(-1,1);
        // }
        
    }
    private void FixedUpdate(){
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void Up(){
        facingLeft = false;
        facingRight = false;
        facingUp = true;
        facingDown = false;
    }
    private void Down(){
        facingLeft = false;
        facingRight = false;
        facingUp = false;
        facingDown = true;
    }
    private void Right(){
        facingLeft = false;
        facingRight = true;
        facingUp = false;
        facingDown = false;
    }
    private void Left(){
        facingLeft = true;
        facingRight = false;
        facingUp = false;
        facingDown = false;
    }
}
