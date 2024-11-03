using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IceWizard : Heroes
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioClip iceSound;
    // [SerializeField] private Transform rangeObject; // Reference to the Range GameObject

    [Header("Attribute")]
    [SerializeField] private float freezeTime = 1.5f;

    private Transform target;
    private float timeUntilFire;
    Animator anim;
    protected override void Start()
    {
        base.Start(); // Call the base class Start to initialize common properties
        anim = GetComponent<Animator>();

        // Set specific values for Wizard if needed
        aps = 0.5f; // example value
        damage = 20f; // example value
        upgradeCost = 100f;
    }
    
    private void OnMouseDown()
    {
        HeroUpgrade upgradeUI = FindObjectOfType<HeroUpgrade>();
        if (upgradeUI == null)
        {
            Debug.LogError("Upgrade UI not assigned.");
            return;
        }
        upgradeUI.Initialize(this);
        upgradeUI.ToggleUpgradePanel(true);
    }

    private void Update()
    {
        // Set targetingRange based on the x or y scale of the Range object
        // targetingRange = Mathf.Max(rangeObject.localScale.x, rangeObject.localScale.y) / 2f;

        // anim = GetComponent<Animator>();
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
            bulletScript.SetDamage(Mathf.RoundToInt(CurrentDamage));
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

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();

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

}