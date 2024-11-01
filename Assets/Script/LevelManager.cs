using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    [Header("References")]
    [SerializeField] TextMeshProUGUI MoneyText;
    [SerializeField] TextMeshProUGUI MaxPlacedText;
    public Transform startPoint;
    public Transform[] path;
    public int currency;
    private void Awake(){
        main = this;
    }
    private void Start(){
        currency = 100;
        MoneyText.gameObject.SetActive(false); // Hide MoneyText initially
        MaxPlacedText.gameObject.SetActive(false); // Hide MaxPlacedText initially
    }
    public void IncreaseCurrency(int amount){
        currency += amount;
        // Debug.Log("Income : " + amount);
    }
    public bool SpendCurrency(int amount){
        if(amount <= currency){
            currency -= amount;
            return true;
        }else{
            //StartCoroutine(NoMoneyText("You don't have enough money"));
            Debug.Log("You don't have enough money");
            return false;
        }
    }
    public IEnumerator NoMoneyText(string message) {
        MoneyText.text = message; // ตั้งค่าข้อความ
        MoneyText.gameObject.SetActive(true); // แสดงข้อความ
        yield return new WaitForSeconds(2f); // รอ 3 วินาที
        MoneyText.gameObject.SetActive(false); // ซ่อนข้อความ
    }public IEnumerator MaxPlaceText(string message) {
        MaxPlacedText.text = message; // ตั้งค่าข้อความ
        MaxPlacedText.gameObject.SetActive(true); // แสดงข้อความ
        yield return new WaitForSeconds(2f); // รอ 3 วินาที
        MaxPlacedText.gameObject.SetActive(false); // ซ่อนข้อความ
    }
}
