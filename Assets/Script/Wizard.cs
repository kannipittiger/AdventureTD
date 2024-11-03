using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Heroes
{
    [Header("References")]
    [SerializeField] private Transform heroRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioClip fireSound;

    private Animator anim;
    private Transform target;
    private float timeUntilFire;


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
        aps = 0.8f; // example value
        damage = 30f; // example value
        upgradeCost = 70f;
        towerIndex = 0;
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
        if (target == null)
        {
            anim.SetBool("area", false);
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            anim.SetBool("area", true);
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / aps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();

        Vector2 direction = (target.position - firingPoint.position).normalized;
        bulletRb.velocity = direction * 10f;

        if (bulletScript != null)
        {
            bulletScript.Initialize(targetingRange);
            bulletScript.SetDamage(Mathf.RoundToInt(CurrentDamage));
        }
        SoundManager.instance.PlaySound(fireSound);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
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
            scale.x = -scale.x;
        }
        else if (target.position.x > transform.position.x && scale.x < 0)
        {
            scale.x = -scale.x;
        }
        heroRotationPoint.localScale = scale;
    }
}
