using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button firstStageButton;
    [SerializeField] private Button secondStageButton;
    [SerializeField] private TextMeshProUGUI firstStageText;
    [SerializeField] private TextMeshProUGUI secondStageText;
    [SerializeField] private TextMeshProUGUI invalidText;
    [SerializeField] private Button mainMenu;
    [SerializeField] public GameObject selectStagePanel;

    // private EnemySpawner enemySpawner;
    [SerializeField] private EnemySpawner enemySpawner;

    private void Start(){
    enemySpawner = FindObjectOfType<EnemySpawner>();
    invalidText.gameObject.SetActive(false);
    firstStageText.text = $"Play";
    secondStageText.text = $"Locked";
}


    // private void Update(){
    //     if(enemySpawner.isVictory == true){
    //         secondStageText.text = $"Play";
    //     }
    // }
    public void CheckVictory(){
    if (enemySpawner != null && enemySpawner.isVictory){
        SceneManager.LoadScene("DemonScene");
    } else {
        StartCoroutine(ShowInvalidText("Complete stage 1 first!"));
    }
}


    public void ToStageOne(){
        SceneManager.LoadScene("SampleScene");
    }

    public void ToMenu(){
        selectStagePanel.SetActive(false);
    }

    private IEnumerator ShowInvalidText(string message){
    invalidText.gameObject.SetActive(true);
    invalidText.text = message;
    yield return new WaitForSeconds(1f);  // Wait for 3 seconds before hiding the message
    invalidText.gameObject.SetActive(false);
}


}
