using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform bulletRotationPoint;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;

    private Transform target;
    private Vector3 spawnPosition;
    private float maxRange;
    // เพิ่มตัวแปรสำหรับ Animator
    private Animator anim;

    private int damage = 30;
    private int currentDamage;

    private void Start()
    {
        // รับ Animator จากคอมโพเนนต์ใน Bullet prefab
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            // Debug.Log("isus");
        }
        else
        {
            // Debug.Log("ihere");
        }
    }
    public void SetDamage(int heroDamage)
    {
        damage = heroDamage;
    }
    public void Initialize(float range)
    {
        // Set the initial position and range when the bullet is created
        spawnPosition = transform.position;
        maxRange = range;
    }
    private void Update()
    {
        // Check if the bullet has exceeded its maximum range
        if (Vector3.Distance(spawnPosition, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
        RotateTowardsTarget();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // currentDamage = Mathf.RoundToInt(hero.damage);
        // ลดพลังชีวิตของศัตรูเมื่อชน
        other.gameObject.GetComponent<Health>().TakeDamage(damage);

        // หยุดการเคลื่อนที่ของกระสุน
        rb.velocity = Vector2.zero;  // หยุดการเคลื่อนที่
        rb.isKinematic = true; // ตั้งค่า Rigidbody เป็น Kinematic เพื่อหยุดการฟิสิกส์

        // เล่นแอนิเมชันชน (Hit Animation)
        if (anim != null)
        {
            anim.SetTrigger("hit"); // เปลี่ยน Trigger เป็น "Hit"
                                    // Debug.Log("teedon");
        }
        else
        {
            // Debug.Log("eiei");
        }

        // เรียกใช้ Coroutine เพื่อรอให้แอนิเมชันเล่นเสร็จ
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // รอให้แอนิเมชันเล่นเสร็จ (ปรับเวลาให้ตรงกับความยาวของแอนิเมชัน)
        yield return new WaitForSeconds(0.5f); // ปรับเวลาตามความยาวของแอนิเมชัน Hit

        // ทำลายกระสุน
        Destroy(gameObject);
    }


    private void RotateTowardsTarget()
    {
        Vector3 scale = bulletRotationPoint.localScale;
        if (target.position.x < transform.position.x && scale.x > 0)
        {
            scale.x = -scale.x;
        }
        else if (target.position.x > transform.position.x && scale.x < 0)
        {
            scale.x = -scale.x;
        }
        bulletRotationPoint.localScale = scale;
    }
}
