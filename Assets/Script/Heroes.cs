using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Heroes : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 1f;//bullet per second

    private Transform target;
    private float timeUntilFire;

    Animator anim;
    

    private void Update(){
        anim = GetComponent<Animator> ();
        if(target == null){
            anim.SetBool ("area", false);
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if(!CheckTargetIsInRange()){
            
            target = null;
        }else{
            timeUntilFire += Time.deltaTime;
            if(timeUntilFire >= 1f/bps){
                Shoot();
                anim.SetBool ("area", true);
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot(){
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }
    private bool CheckTargetIsInRange(){
        return Vector2.Distance(target.position,transform.position) <= targetingRange;
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if(hits.Length > 0){
            target = hits[0].transform;
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



    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward,targetingRange);
    }
}
