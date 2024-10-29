using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    
    private int currentHP; // ตัวแปรที่จะใช้เก็บค่า hitPoints
    private int currentSpeed;
    private int currentMoney;

    private void Awake(){
        onEnemyDestroy.AddListener(EnemyDestroyed);
        currentHP = baseEnemyHealth;
        currentSpeed = baseEnemySpeed;
        currentMoney = baseMoney;
        waveText.gameObject.SetActive(false); // ซ่อนข้อความเริ่มต้น
    }
    
    private void Start(){
        StartCoroutine(StartWave());
    }
    
    private void Update(){
        if(!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0){
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        
        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0){
            EndWave();
        }
    }

    private void EnemyDestroyed(){
        enemiesAlive--;
    }

    private IEnumerator StartWave(){
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave(){
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        // แสดงข้อความเมื่อเริ่ม wave 2
        if (currentWave == 1) {
            StartCoroutine(ShowWaveText("Wave 1"));
        }
        else if (currentWave == 2) {
            currentHP = baseEnemyHealth * 2;
            baseEnemyHealth = currentHP;
            StartCoroutine(ShowWaveText("Wave 2 : Enemy HP x 2"));
        }
        else if (currentWave == 3) {
            currentMoney = baseMoney * 3;
            baseMoney = currentMoney;
            StartCoroutine(ShowWaveText("Wave 3 : Bonus wave money x 3"));
        }
        else if (currentWave == 4) {
            currentSpeed = baseEnemySpeed * 2;
            baseEnemySpeed = currentSpeed;
            StartCoroutine(ShowWaveText("Wave 4 : Enemy Speed x 2"));
        }else if (currentWave == 5){
            currentHP = baseEnemyHealth * 3;
            baseEnemyHealth = currentHP;
            StartCoroutine(ShowWaveText("Wave 4 : Enemy HP x 3"));
        }
        StartCoroutine(StartWave());
    }

    private IEnumerator ShowWaveText(string message) {
        waveText.text = message; // ตั้งค่าข้อความ
        waveText.gameObject.SetActive(true); // แสดงข้อความ
        yield return new WaitForSeconds(3f); // รอ 3 วินาที
        waveText.gameObject.SetActive(false); // ซ่อนข้อความ
    }

    private void SpawnEnemy(){
        // Debug.Log("Spawn Enemy");
        GameObject prefabToSpawn = enemyPrefabs[0];
        GameObject enemyInstance = Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
        Health enemyHealth = enemyInstance.GetComponent<Health>();
        EnemyMovement enemySpeed = enemyInstance.GetComponent<EnemyMovement>();
        Health enemyDrop = enemyInstance.GetComponent<Health>();
        if (currentWave == 2)
        {
            enemyHealth.SetHealth(currentHP);            
        }
        else if(currentWave == 3){
            enemyDrop.SetCurrencyWorth(currentMoney);
        }
        else if(currentWave == 4){
            enemyDrop.ResetCurrencyWorth();
            enemySpeed.UpdateSpeed(currentSpeed);
        }
        else if(currentWave == 5){
            Debug.Log("HP & SPD : " + currentHP + " " + currentSpeed);
            enemyHealth.SetHealth(currentHP);
            // enemySpeed.UpdateSpeed(currentSpeed);
        }
    }

    private int EnemiesPerWave(){
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultlyScalingFactor));
    }
}
