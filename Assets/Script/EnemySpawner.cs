using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] TextMeshProUGUI waveText; // เพิ่มตัวแปรสำหรับข้อความ

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 6;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultlyScalingFactor = 0.75f;
    [SerializeField] private int baseEnemyHealth = 30;
    [SerializeField] private int baseEnemySpeed = 2;
    [SerializeField] private int baseMoney = 50;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private float eps;//enemy per second
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private int currentHP; // ตัวแปรที่จะใช้เก็บค่า hitPoints
    private int currentSpeed;
    private int currentMoney;
    public bool isVictory = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        currentHP = baseEnemyHealth;
        currentSpeed = baseEnemySpeed;
        currentMoney = baseMoney;
        waveText.gameObject.SetActive(false); // ซ่อนข้อความเริ่มต้น
    }

    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        // แสดงข้อความเมื่อเริ่ม wave 1
        if (currentWave == 1)
        {
            StartCoroutine(ShowWaveText("Wave 1"));
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        // แสดงข้อความเมื่อเริ่ม wave ถัดไป
        if (currentWave == 2)
        {
            currentHP = baseEnemyHealth * 2;
            baseEnemyHealth = currentHP;
            StartCoroutine(ShowWaveText("Wave 2 : Enemy HP x 2"));
        }
        else if (currentWave == 3)
        {
            currentSpeed = baseEnemySpeed * 2;
            baseEnemySpeed = currentSpeed;
            StartCoroutine(ShowWaveText("Wave 3 : Enemy Speed x 2"));
        }
        else if (currentWave == 4)
        {
            currentMoney = baseMoney * 3;
            baseMoney = currentMoney;

            StartCoroutine(ShowWaveText("Wave 4 : Bonus wave money x 3"));
        }
        else if (currentWave == 5)
        {

            StartCoroutine(ShowWaveText("Wave 5"));
        }
        else if (currentWave == 6)
        {
            currentSpeed = baseEnemySpeed * 2;
            baseEnemySpeed = currentSpeed;
            StartCoroutine(ShowWaveText("Wave 6 : Enemy Speed x 2"));
        }
        else if (currentWave == 7)
        {
            currentHP = baseEnemyHealth * 3;
            baseEnemyHealth = currentHP;
            StartCoroutine(ShowWaveText("Wave 7 : Enemy HP x 3"));
        }
        else if (currentWave == 8)
        {
            currentHP = baseEnemyHealth * 2;
            baseEnemyHealth = currentHP;
            currentSpeed = baseEnemySpeed * 2;
            baseEnemySpeed = currentSpeed;
            StartCoroutine(ShowWaveText("Wave 8 : Enemy HP & SPD x 2"));
        }
        else if (currentWave > 8)
        {
            StartCoroutine(ShowEndMessageAndLoadScene("You completed Wave 8!\nLoading Next Stage..."));
            isVictory = true;
        }
        StartCoroutine(StartWave());
    }

    private IEnumerator ShowEndMessageAndLoadScene(string message)
    {
        waveText.text = message; // แสดงข้อความ
        waveText.gameObject.SetActive(true); // แสดงข้อความ
        yield return new WaitForSeconds(3f); // รอ 3 วินาที
        SceneManager.LoadScene("Next Stage"); // โหลด Scene ถัดไป
        waveText.gameObject.SetActive(false); // ซ่อนข้อความ
    }
    private IEnumerator ShowWaveText(string message)
    {
        waveText.text = message; // ตั้งค่าข้อความ
        waveText.gameObject.SetActive(true); // แสดงข้อความ

        float countdown = timeBetweenWaves; // เริ่มต้นเวลานับถอยหลัง

        while (countdown > 0)
        {
            // อัปเดตข้อความนับถอยหลัง
            waveText.text = message + "\n" + "start in " + Mathf.Ceil(countdown).ToString(); // แสดงข้อความพร้อมนับถอยหลัง
            yield return new WaitForSeconds(1f); // รอ 1 วินาที
            countdown -= 1f; // ลดเวลานับถอยหลัง
        }

        waveText.gameObject.SetActive(false); // ซ่อนข้อความ
    }


    private void SpawnEnemy()
    {
        // Debug.Log("Spawn Enemy");
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        GameObject enemyInstance = Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
        Health enemyHealth = enemyInstance.GetComponent<Health>();
        EnemyMovement enemySpeed = enemyInstance.GetComponent<EnemyMovement>();
        Health enemyDrop = enemyInstance.GetComponent<Health>();
        if (currentWave == 2)
        {
            enemyHealth.SetHealth(currentHP);
        }
        else if (currentWave == 3)
        {
            enemySpeed.UpdateSpeed(currentSpeed);
        }
        else if (currentWave == 4)
        {
            enemyDrop.SetCurrencyWorth(currentMoney);
        }
        else if (currentWave == 5)
        {
            enemyDrop.ResetCurrencyWorth();

        }
        else if (currentWave == 6)
        {
            enemySpeed.UpdateSpeed(currentSpeed);
        }
        else if (currentWave == 7)
        {
            enemyHealth.SetHealth(currentHP);
        }
        else if (currentWave == 8)
        {
            enemyHealth.SetHealth(currentHP);
            enemySpeed.UpdateSpeed(currentSpeed);
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultlyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultlyScalingFactor), 0f, enemiesPerSecondCap);
    }
}
