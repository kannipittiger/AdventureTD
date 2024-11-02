using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform bulletRotationPoint;
    private Animator anim; // Animator reference

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    private Transform target;
    private Vector3 spawnPosition;
    private float maxRange;

    private void Start()
    {
        // Get Animator component from this game object
        anim = GetComponent<Animator>();
    }

    public void Initialize(float range)
    {
        spawnPosition = transform.position;
        maxRange = range;
    }

    public void PlayShootAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("hit"); // ใช้ Trigger เพื่อเล่นแอนิเมชัน
        }
    }

    private void Update()
    {
        if (Vector3.Distance(spawnPosition, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        Destroy(gameObject);
    }
}
