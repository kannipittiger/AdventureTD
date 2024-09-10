using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool facingRight;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private void Start(){
        target =  LevelManager.main.path[pathIndex];
    }
    private void Update(){
        if(Vector2.Distance(target.position, transform.position) <= 0.1f){
            pathIndex++;
            if(pathIndex == LevelManager.main.path.Length){
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }else{
                target =  LevelManager.main.path[pathIndex];
            }
        }
        //Change direction of Enemy
        // if(pathIndex >= 2 && pathIndex < 6){
        //     facingRight = false;
        // }else if(pathIndex >= 6 && pathIndex < 7){
        //     facingRight = true;
        // }else if(pathIndex >= 8){
        //     facingRight = false;
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
}
