using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    // [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

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
            anim.SetBool("area", false);
            return;
        }

        anim.SetBool("area", true);
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            foreach (Transform target in targets)
            {
                if (CheckTargetIsInRange(target))
                {
                    Shoot(target);  // Shoot all targets in range
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
        if (targets.Count == 0) return;

        Transform closestTarget = targets[0];
        Vector3 scale = heroRotationPoint.localScale;

        if (closestTarget.position.x < transform.position.x && scale.x > 0)
        {
            scale.x = -scale.x;
        }
        else if (closestTarget.position.x > transform.position.x && scale.x < 0)
        {
            scale.x = -scale.x;
        }

        if (closestTarget.position.y < transform.position.y && scale.y > 0)
        {
            scale.y = -scale.y;
        }
        else if (closestTarget.position.y > transform.position.y && scale.y < 0)
        {
            scale.y = -scale.y;
        }

        heroRotationPoint.localScale = scale;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    #endif
}

