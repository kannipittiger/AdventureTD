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
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private Transform rangeObject; // Reference to the Range GameObject

    [Header("Attribute")]

    [SerializeField] private float aps = 1f;
    [SerializeField] public float damage = 30f;
    private HeroUpgrade upgradeUI;

    public float CurrentDamage => damage;
    public float TargetingRange => targetingRange;
    private Transform target;
    private float timeUntilFire;
    public float targetingRange;
    private SpriteRenderer spriteRenderer;
    Animator anim;

    private void Start()
    {
        // Assuming the HeroUpgradeUI is attached to the Canvas
        upgradeUI = FindObjectOfType<HeroUpgrade>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the hero.");
        }
    }
    public Sprite GetHeroSprite()
    {
        return spriteRenderer != null ? spriteRenderer.sprite : null;
    }

    private void OnMouseDown()
    {
        if (upgradeUI == null)
        {
            Debug.LogError("Upgrade UI not assigned.");
            return;
        }
        upgradeUI.Initialize(this);
        upgradeUI.ToggleUpgradePanel(true);
    }

    public void UpgradeStats()
    {
        damage *= 1.2f;  // Increase damage
        targetingRange *= 1.1f; // Increase range
        rangeObject.localScale = new Vector3(targetingRange * 2, targetingRange * 2, rangeObject.localScale.z);

    }

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
        RotateTowardsTarget(); // remove this if want line attack

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

        // Calculate direction to target
        Vector2 direction = (target.position - firingPoint.position).normalized;
        bulletRb.velocity = direction * 10f; // Adjust bullet speed as needed

        if (bulletScript != null)
        {
            bulletScript.Initialize(targetingRange);
            bulletScript.SetDamage(Mathf.RoundToInt(damage));
        }
        SoundManager.instance.PlaySound(fireSound);
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

    // #if UNITY_EDITOR
    // private void OnDrawGizmosSelected(){
    //     Handles.color = Color.cyan;
    //     Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    // }
    // #endif
}
