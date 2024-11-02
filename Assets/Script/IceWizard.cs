using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IceWizard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioClip iceSound;

    [Header("Attribute")]
    [SerializeField] private float aps = 0.5f;
    [SerializeField] private float freezeTime = 1.5f;

    private Transform target;
    private float timeUntilFire;
    private float targetingRange;

    Animator anim;
    private void Update()
    {
        // Set targetingRange based on the x or y scale of the Range object
        targetingRange = Mathf.Max(rangeObject.localScale.x, rangeObject.localScale.y) / 2f;

        anim = GetComponent<Animator>();
        if (target == null)
        {
            anim.SetBool("area", false);
            FindTarget();
            return;
        }
        RotateTowardsTarget();// remove this if want line attack

        if (!CheckTargetIsInRange())
        {

            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / aps)
            {
                Shoot();
                
                FreezeEnemies();
                anim.SetBool("area", true);
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BlueFireball bulletScript = bulletObj.GetComponent<BlueFireball>();
        bulletScript.SetTarget(target);
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();

        // Calculate direction to target
        Vector2 direction = (target.position - firingPoint.position).normalized;
        bulletRb.velocity = direction * 10f; // Adjust bullet speed as needed

        if (bulletScript != null)
        {
            bulletScript.Initialize(targetingRange);
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);
                StartCoroutine(ResetEnemySpeed(em));
                SoundManager.instance.PlaySound(iceSound);
            }
        }
        
    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 scale = heroRotationPoint.localScale;
        if (target.position.x < transform.position.x && scale.x > 0)
        {
            // If target is on the left and the character is facing right, flip the character to the left
            scale.x = -scale.x;
        }
        else if (target.position.x > transform.position.x && scale.x < 0)
        {
            // If target is on the right and the character is facing left, flip the character to the right
            scale.x = -scale.x;
        }
        heroRotationPoint.localScale = scale;
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
        
    }

    // #if UNITY_EDITOR
    // private void OnDrawGizmosSelected(){
    //     Handles.color = Color.cyan;
    //     Handles.DrawWireDisc(transform.position, transform.forward,targetingRange);
    // }
    // #endif
}