using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform bulletRotationPoint;
    
    
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    private Transform target;

    public void SetTarget(Transform _target){
        target = _target;
    }

    public void FixedUpdate(){
        if(!target)return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
        RotateTowardsTarget();
    }
    private void OnCollisionEnter2D(Collision2D other){
        //Take health from enemy
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }

    private void RotateTowardsTarget(){
        Vector3 scale = bulletRotationPoint.localScale;
        if (target.position.x < transform.position.x && scale.x > 0) {
            // If target is on the left and the character is facing right, flip the character to the left
            scale.x = -scale.x;
        } else if (target.position.x > transform.position.x && scale.x < 0) {
            // If target is on the right and the character is facing left, flip the character to the right
            scale.x = -scale.x;
        }
        bulletRotationPoint.localScale = scale;
    }   
}
