using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float lineAttackRange = 5f; // Line attack range distance
    [SerializeField] private float lineAttackWidth = 1f; // Width of the line attack
    [SerializeField] private float aps = 1f;
    [SerializeField] private int damage = 100;

    private Transform target;
    private float timeUntilFire;
    Animator anim;

    private void Update(){
        anim = GetComponent<Animator>();

        if (target == null || !CheckTargetIsInRange()){
            //anim.SetBool("area", false);
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        //anim.SetBool("area", true);
        timeUntilFire += Time.deltaTime;

        if(timeUntilFire >= 1f / aps){
            Shoot();
            LineAttack(); // Perform line attack
            timeUntilFire = 0f;
        }
    }

    private void Shoot(){
        // Instantiate bullet and set direction towards target
        if (target != null) {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();

            // Calculate direction to target
            Vector2 direction = (target.position - firingPoint.position).normalized;
            bulletRb.velocity = direction * 10f; // Adjust bullet speed as needed

            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            if (bulletScript != null) {
                bulletScript.Initialize(targetingRange);
            }
        }
    }

    private void LineAttack(){
        if (target == null) return;

        // Calculate the direction from firing point to the target
        Vector2 directionToTarget = (target.position - firingPoint.position).normalized;

        // Determine the center of the boxcast along this direction
        Vector2 boxCenter = (Vector2)firingPoint.position + directionToTarget * (lineAttackRange / 2);

        // Perform the box cast to detect enemies in line attack range
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            boxCenter, 
            new Vector2(lineAttackRange, lineAttackWidth), 
            Vector2.SignedAngle(Vector2.right, directionToTarget), // Rotate box towards target
            Vector2.zero, 
            0f, 
            enemyMask
        );

        foreach (var hit in hits) {
            // Apply damage to each enemy hit in the line
            Health enemy = hit.transform.GetComponent<Health>(); // Assuming each enemy has a Health component
            if (enemy != null) {
                enemy.TakeDamage(damage); // Call a method to apply damage to the enemy
            }
        }
    }

    private bool CheckTargetIsInRange(){
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        
        // Find the closest enemy within range
        float closestDistance = targetingRange;
        target = null;

        foreach (var hit in hits) {
            float distance = Vector2.Distance(hit.transform.position, transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                target = hit.transform;
            }
        }
    }
    
    private void RotateTowardsTarget(){
        Vector3 scale = heroRotationPoint.localScale;
        if (target.position.x < transform.position.x && scale.x > 0) {
            scale.x = -scale.x;
        } else if (target.position.x > transform.position.x && scale.x < 0) {
            scale.x = -scale.x;
        }
        heroRotationPoint.localScale = scale;
    }   

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

        // Draw line attack range as a box to show width
        if (target != null) {
            Vector2 directionToTarget = (target.position - firingPoint.position).normalized;
            Vector3 lineCenter = firingPoint.position + (Vector3)(directionToTarget * (lineAttackRange / 2));
            Handles.color = Color.red;
            Handles.DrawWireCube(lineCenter, new Vector3(lineAttackRange, lineAttackWidth, 0));
        }
    }
    #endif
}
