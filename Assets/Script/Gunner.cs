using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gunner : Heroes
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioClip gunSound;
    [SerializeField] private GameObject heroRange;
    // [SerializeField] private Transform rangeObject;

    [Header("Attributes")]
 
    [SerializeField] private float lineAttackRange = 5f;
    [SerializeField] private float lineAttackWidth = 1f;


    [Header("Firing Point Offsets")]
    [SerializeField] private Vector2 offsetLeft;
    [SerializeField] private Vector2 offsetRight;
    [SerializeField] private Vector2 offsetUp;
    [SerializeField] private Vector2 offsetDown;

    private Transform target;
    private float timeUntilFire;

    Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();

        // Check if BuildManager.main is assigned
        if (BuildManager.main == null)
        {
            Debug.LogError("BuildManager.main is not assigned. Please ensure BuildManager is in the scene.");
            return;
        }

        // Get the selected tower, but check if it's null
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild != null)
        {
            orginalCost = towerToBuild.cost;
        }
        else
        {
            Debug.LogWarning("No tower selected when initializing Wizard. orginalCost not set.");
        }

        // Set specific values for Wizard if needed
        aps = 0.5f; // example value
        damage = 40f; // example value
        upgradeCost = 130f;
        towerIndex = 3;
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
        StartCoroutine(setRange());
    }
    
    private IEnumerator setRange(){
        heroRange.SetActive(true);
        yield return new WaitForSeconds(3f);
        heroRange.SetActive(false);
    }
    private void Update()
    {
        // anim = GetComponent<Animator>();
        // targetingRange = Mathf.Max(rangeObject.localScale.x, rangeObject.localScale.y) / 2f;
        if (target == null || !CheckTargetIsInRange()){
            //anim.SetBool("area", false);
            FindTarget();
            return;
        }

        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            Shoot();
            LineAttack(); // Perform line attack
            timeUntilFire = 0f;
        }
    }

    private void Shoot()
    {
        if (target != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            Vector2 direction = (target.position - firingPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bulletObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Rifle bulletScript = bulletObj.GetComponent<Rifle>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(targetingRange);
                // เรียกใช้ PlayShootAnimation เมื่อกระสุนถูกสร้าง
                bulletScript.PlayShootAnimation();
                SoundManager.instance.PlaySound(gunSound);
            }
        }
    }


    private void LineAttack()
    {
        if (target == null) return;

        // Calculate the direction from firing point to the target
        Vector2 directionToTarget = (target.position - firingPoint.position).normalized;

        // Rotate the animation according to the attack direction
        SetAnimationDirection(directionToTarget);

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

        foreach (var hit in hits)
        {
            // Apply damage to each enemy hit in the line
            Health enemy = hit.transform.GetComponent<Health>(); // Assuming each enemy has a Health component
            if (enemy != null)
            {
                enemy.TakeDamage(Mathf.RoundToInt(CurrentDamage)); // Call a method to apply damage to the enemy
            }
        }
    }

    private void SetAnimationDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
            {
                anim.SetTrigger("left");
                firingPoint.localPosition = offsetLeft;
            }
            else
            {
                anim.SetTrigger("right");
                firingPoint.localPosition = offsetRight;
            }
        }
        else
        {
            if (direction.y < 0)
            {
                anim.SetTrigger("down");
                firingPoint.localPosition = offsetDown;
            }
            else
            {
                anim.SetTrigger("up");
                firingPoint.localPosition = offsetUp;
            }
        }
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        float closestDistance = targetingRange;
        target = null;

        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(hit.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = hit.transform;
            }
        }
    }
}
