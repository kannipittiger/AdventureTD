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

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 1f;

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
            // If target is on the left and the character is facing right, flip the character to the left
            scale.x = -scale.x;
        } else if (target.position.x > transform.position.x && scale.x < 0) {
            // If target is on the right and the character is facing left, flip the character to the right
            scale.x = -scale.x;
        }
        heroRotationPoint.localScale = scale;
    }   

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    #endif
}
