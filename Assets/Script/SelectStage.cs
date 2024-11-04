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

    private EnemySpawner enemySpawner;
    private void Start(){
        invalidText.gameObject.SetActive(false);
        secondStageText.text = $"Locked";
    }

    private void Update(){
        if(enemySpawner.isVictory == true){
            secondStageText.text = $"Play";
        }
    }
    public void CheckVictory(){
        if(enemySpawner.isVictory == true){
            
            SceneManager.LoadScene("DemonScene");
        }else{
            invalidText.gameObject.SetActive(true);
            invalidText.text = $"Comeplete stage 1 first!!";
        }
    }

    public void ToStageOne(){
        SceneManager.LoadScene("SampleScene");
    }

    public void ToMenu(){
        SceneManager.LoadScene("start game");
    }


}
