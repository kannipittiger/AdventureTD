using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Knight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    // [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioClip swordSound;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 1f;
    [SerializeField] private int damage = 50; // Damage dealt per shot

    private List<Transform> targets = new List<Transform>();
    private float timeUntilFire;

    private Animator anim;

    private void Update()
    {
        anim = GetComponent<Animator>();

        FindTargets();  // Check for all enemies within range

        if (targets.Count == 0)
        {
            anim.SetBool("up", false);
            anim.SetBool("down", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
            return;
        }
        
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            foreach (Transform target in targets)
            {
                if (CheckTargetIsInRange(target))
                {
                    
                    Shoot(target);  // Shoot all targets in range
                    RotateTowardsTarget();
                    SoundManager.instance.PlaySound(swordSound);
                }
                
            }
            timeUntilFire = 0f;
        }
    }

    private void Shoot(Transform target)
    {
        // Check if the target has a Health component, and apply damage if it does
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            
            targetHealth.TakeDamage(damage);
        }
    }

    private bool CheckTargetIsInRange(Transform target)
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTargets()
    {
        targets.Clear();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform != null)
            {
                targets.Add(hit.transform);
            }
        }
    }

    private void RotateTowardsTarget()
{
    

    Transform closestTarget = targets[0];

    // รีเซ็ตค่าแอนิเมชันทั้งหมดก่อนตั้งค่าใหม่
    anim.SetBool("up", false);
    anim.SetBool("down", false);
    anim.SetBool("left", false);
    anim.SetBool("right", false);

    // ตรวจสอบตำแหน่งของ enemy เพื่อกำหนดทิศทาง
    if (Mathf.Abs(closestTarget.position.x - transform.position.x) > Mathf.Abs(closestTarget.position.y - transform.position.y))
    {
        // enemy อยู่ในแนวนอน
        if (closestTarget.position.x < transform.position.x)
        {
            // enemy อยู่ด้านซ้าย
            // anim.SetBool("up", false);
            // anim.SetBool("down", false);
            // anim.SetBool("left", true);
            // anim.SetBool("right", false);
            anim.SetTrigger("lleft");
        }
        else
        {
            // enemy อยู่ด้านขวา
            // anim.SetBool("up", false);
            // anim.SetBool("down", false);
            // anim.SetBool("left", false);
            // anim.SetBool("right", true);
            anim.SetTrigger("rright");
        }
    }
    else
    {
        // enemy อยู่ในแนวตั้ง
        if (closestTarget.position.y < transform.position.y)
        {
            // enemy อยู่ด้านล่าง
            // anim.SetBool("up", false);
            // anim.SetBool("down", true);
            // anim.SetBool("left", false);
            // anim.SetBool("right", false);
            anim.SetTrigger("ddown");
        }
        else
        {
            // enemy อยู่ด้านบน
            // anim.SetBool("up", true);
            // anim.SetBool("down", false);
            // anim.SetBool("left", false);
            // anim.SetBool("right", false);
            anim.SetTrigger("aup");
        }
        Debug.Log("ook");
    }
}

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    #endif
}

